using UnityEngine;
using System.Collections.Generic;

public class BonusManager : MonoBehaviour
{
    public GameObject[] bonusPlanets;
    public GameObject[] bonusAsteroids;
    public Scoring scoringScript;

    private Tuile[] toutesLesTuiles;
    private List<GameObject> listPlanet = new List<GameObject>();

    private void Start()
    {
        toutesLesTuiles = FindObjectsByType<Tuile>(FindObjectsSortMode.None);
        SpawnBonus();
    }

    private void SpawnBonus()
    {
        listPlanet.Clear();

        var pattern = CombinationLib.ChooseBonusPattern();
        var patternPos = new HashSet<Vector2>();
        int planetID = Random.Range(0, bonusPlanets.Length);

        foreach (var pos in pattern.positions)
            patternPos.Add(pos);

        foreach (Tuile tuile in toutesLesTuiles)
        {
            Vector2 pos = tuile.GetGridPosition();
            int chosenID = patternPos.Contains(pos) ? planetID : Random.Range(0, bonusPlanets.Length);

            GameObject planet = Instantiate(bonusPlanets[chosenID], tuile.transform.position, Quaternion.identity);
            planet.SetActive(false);
            tuile.currentPlanet = planet;
            listPlanet.Add(planet);

            GameObject asteroid = Instantiate(bonusAsteroids[Random.Range(0, bonusAsteroids.Length)], tuile.transform.position, Quaternion.identity);
            asteroid.GetComponent<Asteroid>().setLinkedPlanet(planet);
            asteroid.GetComponent<Asteroid>().scoreScript = scoringScript;
        }

        if (scoringScript != null)
        {
            scoringScript.Allplanets = listPlanet;
            scoringScript.currentPhase = Scoring.Phase.Bonus;
        }
    }
}
