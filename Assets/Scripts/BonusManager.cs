using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BonusGameManager : MonoBehaviour
{
    public GameObject[] caillouPrefab;
    public GameObject[] planetePrefab;
    public Scoring scoringScript;
    private Tuile[] toutesLesTuiles;
    private List<GameObject> listPlanets = new List<GameObject>();

    private void Start()
    {
        
        Debug.Log("Phase actuelle (PatternManager) : " + PatternManager.Instance.CurrentPhase);

        scoringScript.currentPhase = PatternManager.Instance.CurrentPhase;

        Debug.Log("Phase actuelle (scoring) : " + scoringScript.currentPhase);
        
        PatternManager.Instance.CurrentPhase = Scoring.Phase.Bonus;

        toutesLesTuiles = FindObjectsByType<Tuile>(FindObjectsSortMode.None);

        CombinationLib.PatternCombination pattern = PatternManager.Instance.GetCurrentPattern();
        if (pattern == null || pattern.positions == null)
        {
            Debug.LogWarning("❌ Pattern invalide (null) pour le bonus !");
            return;
        }


        SpawnPlanetsAndAsteroids(pattern);
    }

    private void SpawnPlanetsAndAsteroids(CombinationLib.PatternCombination pattern)
    {
        listPlanets.Clear();

        HashSet<Vector2> patternPositions = new HashSet<Vector2>(pattern.positions);
        int patternPlanetID = Random.Range(0, planetePrefab.Length);

        Debug.Log("Pattern bonus choisi : " + pattern.value);

        foreach (Tuile tuile in toutesLesTuiles)
        {
            Vector2 tilePos = tuile.transform.position;
            int chosenPlanetID;

            if (PositionInPattern(tilePos, patternPositions))
            {
                chosenPlanetID = patternPlanetID;
                Debug.Log("[Pattern Bonus] Planète spéciale sur cette tuile");
            }
            else
            {
                do
                {
                    chosenPlanetID = Random.Range(0, planetePrefab.Length);
                } while (chosenPlanetID == patternPlanetID);
            }

            // Planète
            GameObject planet = Instantiate(planetePrefab[chosenPlanetID], tuile.transform.position, Quaternion.identity);
            planet.SetActive(false);
            tuile.currentPlanet = planet;
            listPlanets.Add(planet);

            // Astéroïde
            GameObject asteroidPrefab = caillouPrefab[Random.Range(0, caillouPrefab.Length)];
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
            scoringScript.Allplanets = listPlanets;
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
}
