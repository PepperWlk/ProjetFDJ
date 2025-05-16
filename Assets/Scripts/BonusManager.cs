using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public GameObject[] bonusPlanetePrefab;
    public GameObject[] bonusCaillouPrefab;
    public Scoring scoringScript;

    private Tuile[] toutesLesTuiles;
    private List<GameObject> listBonusPlanets = new List<GameObject>();

    private void Start()
    {
        toutesLesTuiles = FindObjectsByType<Tuile>(FindObjectsSortMode.None);
        SpawnBonusPlanetsAndAsteroids();
    }

    private void SpawnBonusPlanetsAndAsteroids()
    {
        listBonusPlanets.Clear();

        CombinationLib.PatternCombination pattern = ChooseBonusPattern();
        HashSet<Vector2> patternPositions = new HashSet<Vector2>();
        int patternPlanetID = -1;

        if (pattern != null)
        {
            foreach (Vector2 pos in pattern.positions)
                patternPositions.Add(pos);

            patternPlanetID = Random.Range(0, bonusPlanetePrefab.Length);
            Debug.Log("Bonus Pattern choisi : " + pattern.value);
        }

        foreach (Tuile tuile in toutesLesTuiles)
        {
            Vector2 tilePos = tuile.transform.position;

            int chosenPlanetID;
            if (pattern != null && PositionInPattern(tilePos, patternPositions))
            {
                chosenPlanetID = patternPlanetID;
            }
            else
            {
                do
                {
                    chosenPlanetID = Random.Range(0, bonusPlanetePrefab.Length);
                } while (pattern != null && chosenPlanetID == patternPlanetID);
            }

            GameObject planet = Instantiate(bonusPlanetePrefab[chosenPlanetID], tuile.transform.position, Quaternion.identity);
            planet.SetActive(false);
            tuile.currentPlanet = planet;
            listBonusPlanets.Add(planet);

            GameObject asteroidPrefab = bonusCaillouPrefab[Random.Range(0, bonusCaillouPrefab.Length)];
            GameObject asteroid = Instantiate(asteroidPrefab, tuile.transform.position, Quaternion.identity);

            Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
            if (asteroidScript != null)
            {
                asteroidScript.setLinkedPlanet(planet);
                asteroidScript.scoreScript = scoringScript;
            }
        }

        if (scoringScript != null)
        {
            scoringScript.Allplanets = listBonusPlanets;
        }
    }

    private bool PositionInPattern(Vector2 tilePos, HashSet<Vector2> patternPositions, float tolerance = 0.5f)
    {
        foreach (Vector2 patternPos in patternPositions)
        {
            if (Vector2.Distance(tilePos, patternPos) < tolerance)
                return true;
        }
        return false;
    }

    private CombinationLib.PatternCombination ChooseBonusPattern()
    {
        float totalChance = 0f;
        foreach (var comb in CombinationLib.bonuscombinations)
            totalChance += comb.chance;

        float rand = Random.Range(0f, 1f);
        float cumulative = 0f;

        foreach (var comb in CombinationLib.bonuscombinations)
        {
            cumulative += comb.chance;
            if (rand < cumulative)
                return comb;
        }

        return null;
    }
}
