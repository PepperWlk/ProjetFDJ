using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] Caillouprefab;
    public GameObject tuilePrefab;
    public GameObject[] planetePrefab;
    public Vector2 spawnPosition = new Vector2(-3, 1);
    public float gap = 0f;
    public List<GameObject> listPlanet;
    public Scoring scoringScript;

    private void Start()
    {
        SpawnDiamond();
    }

    private void SpawnDiamond()
    {
        Vector2[] offsets = new Vector2[] 
        {
            new Vector2(0, 2),
            new Vector2(-1, 1), new Vector2(1, 1),
            new Vector2(-2, 0), new Vector2(0, 0), new Vector2(2, 0),
            new Vector2(-1, -1), new Vector2(1, -1),
            new Vector2(0, -2)
        };

        // Préparer positions absolues (après application de gap et spawnPosition)
        List<Vector2> worldPositions = new List<Vector2>();
        foreach (Vector2 offset in offsets)
        {
            worldPositions.Add(spawnPosition + offset * gap);
        }

        // Choisir un pattern (ou null)
        CombinationLib.PatternCombination pattern = CombinationLib.ChoosePattern();
        Debug.Log("Pattern choisi : " + pattern.value);
        HashSet<Vector2> patternPositions = new HashSet<Vector2>();
        int patternPlanetID = -1;

        if (pattern != null)
        {
            // Préparer les positions exactes du pattern (après application de gap + spawnPosition)
            foreach (Vector2 rawPos in pattern.positions)
            {
                patternPositions.Add(rawPos);
            }

            // Choisir un type de planète pour le pattern
            patternPlanetID = Random.Range(0, planetePrefab.Length);
        }

        // Nettoyer ancienne liste
        listPlanet.Clear();

        // Boucle principale de spawn
        for (int i = 0; i < worldPositions.Count; i++)
        {
            Vector2 position = worldPositions[i];

            // 1. Spawn de la tuile
            GameObject tuile = Instantiate(tuilePrefab, position, Quaternion.identity);

            // 2. Déterminer quelle planète à instancier
            int chosenPlanetID;
            if (pattern != null && patternPositions.Contains(position))
            {
                chosenPlanetID = patternPlanetID; // planète du pattern
            }
            else
            {
                // Choisir une planète au hasard sauf celle du pattern
                do
                {
                    chosenPlanetID = Random.Range(0, planetePrefab.Length);
                } while (pattern != null && chosenPlanetID == patternPlanetID);
            }

            GameObject planet = Instantiate(planetePrefab[chosenPlanetID], position, Quaternion.identity);
            planet.SetActive(false);
            listPlanet.Add(planet);

            // 3. Spawn d’un astéroïde et lien
            GameObject asteroidPrefab = Caillouprefab[Random.Range(0, Caillouprefab.Length)];
            GameObject asteroid = Instantiate(asteroidPrefab, position, Quaternion.identity);

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



    private void SpawnTuileEtPlanete(Vector2 position, GameObject planetPrefabToUse)
    {
        // Spawn tuile
        Instantiate(tuilePrefab, position, Quaternion.identity);

        // Spawn planète
        GameObject planet = Instantiate(planetPrefabToUse, position, Quaternion.identity);
        planet.SetActive(false);
        listPlanet.Add(planet);

        // Spawn astéroïde
        GameObject asteroidPrefab = Caillouprefab[Random.Range(0, Caillouprefab.Length)];
        GameObject asteroid = Instantiate(asteroidPrefab, position, Quaternion.identity);

        Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
        if (asteroidScript != null)
        {
            asteroidScript.setLinkedPlanet(planet);
            asteroidScript.scoreScript = scoringScript;
        }
    }

}
