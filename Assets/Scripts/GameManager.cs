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
        // Position des tuiles en fonction des vecteurs
        Vector2[] offsets = new Vector2[]
        {
            new Vector2(0, 2),
            new Vector2(-1, 1), new Vector2(1, 1),
            new Vector2(-2, 0), new Vector2(0, 0), new Vector2(2, 0),
            new Vector2(-1, -1), new Vector2(1, -1),
            new Vector2(0, -2)
        };

        foreach (Vector2 offset in offsets)
        {
            Vector2 position = spawnPosition + offset * gap;

            // spawn tuile
            GameObject tuile = Instantiate(tuilePrefab, position, Quaternion.identity);

            // Gestion spawn planete et récupération pour la comparaison
            GameObject prefabToSpawn = planetePrefab[Random.Range(0, planetePrefab.Length)];
            GameObject planete = Instantiate(prefabToSpawn, position, Quaternion.identity);
            Debug.Log("Spawned planet with ID: " + planete.GetComponent<Planet>().id);
            planete.SetActive(false);
            listPlanet.Add(planete);
            

            // Gestion spawn asteroid et linkage à la planète spécifique
            GameObject asteroidPrefab = Caillouprefab[Random.Range(0, Caillouprefab.Length)];
            GameObject asteroid = Instantiate(asteroidPrefab, position, Quaternion.identity);

            Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
            if (asteroidScript != null){
                asteroidScript.setLinkedPlanet(planete);
                asteroidScript.scoreScript = scoringScript;
            }


        }
        if (scoringScript != null)
        {
            scoringScript.Allplanets = listPlanet;
        }
    }
}
