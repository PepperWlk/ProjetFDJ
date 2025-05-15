using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] Caillouprefab;
    public GameObject[] planetePrefab;
    public Scoring scoringScript;

    private Tuile[] toutesLesTuiles;
    private List<GameObject> listPlanet = new List<GameObject>();

    private void Start()
    {
        Debug.Log("GameManager started");  
        toutesLesTuiles = FindObjectsByType<Tuile>(FindObjectsSortMode.None);
        SpawnPlanetsAndAsteroids();
    }

    private void SpawnPlanetsAndAsteroids()
    {
        listPlanet.Clear();

        // Choix du pattern
        CombinationLib.PatternCombination pattern = CombinationLib.ChoosePattern();
        HashSet<Vector2> patternPositions = new HashSet<Vector2>();
        int patternPlanetID = -1;

        if (pattern != null)
        {
            foreach (Vector2 raw in pattern.positions)
            {
                patternPositions.Add(raw);
            }
            patternPlanetID = Random.Range(0, planetePrefab.Length);
            Debug.Log("Pattern choisi : " + pattern.value + "PlanetID : " + patternPlanetID);
        }
        else
        {
            Debug.Log("Aucun pattern choisi");
        }

        foreach (Tuile tuile in toutesLesTuiles)
        {
            Vector2 tilePos = tuile.transform.position;

            int chosenPlanetID;
            Debug.Log("Tuile: " + tilePos);
            if (pattern != null && PositionInPattern(tilePos, patternPositions))
            {
                chosenPlanetID = patternPlanetID;
                Debug.Log($"[Pattern] Planète spéciale sur tuile");
            }
            else
            {
                // Autre planète que celle du pattern
                do
                {
                    Debug.Log("Choix d'une planète random");
                    chosenPlanetID = Random.Range(0, planetePrefab.Length);
                } while (pattern != null && chosenPlanetID == patternPlanetID);
            }

            // Planète
            GameObject planet = Instantiate(planetePrefab[chosenPlanetID], tuile.transform.position, Quaternion.identity);
            planet.SetActive(false);
            tuile.currentPlanet = planet;
            listPlanet.Add(planet);

            // Astéroïde
            GameObject asteroidPrefab = Caillouprefab[Random.Range(0, Caillouprefab.Length)];
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
            scoringScript.Allplanets = listPlanet;
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
