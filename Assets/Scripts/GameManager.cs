using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] Caillouprefab;
    public GameObject[] planetePrefab;
    public Scoring scoringScript;

    private Tuile[] toutesLesTuiles;
    private List<GameObject> listPlanet = new List<GameObject>();
    public static int chosenPlanetID;

    private void Start()
    {
        Debug.Log("GameManager started");
        toutesLesTuiles = FindObjectsByType<Tuile>(FindObjectsSortMode.None);

        if (PatternManager.Instance == null)
        {
            Debug.LogError("PatternManager non trouvé !");
            return;
        }
        PrizeManager prizeManager = FindFirstObjectByType<PrizeManager>();
        if (prizeManager == null)
        {
            Debug.LogError("PrizeManager non trouvé !");
            return;
        }
        float targetValue = PrizeManager.DrawPrize(); // À adapter selon ton tirage de lot
        Debug.Log("targetValue :" + targetValue);
        if (PatternManager.Instance != null)
        {
            PatternManager.Instance.SelectPatterns(targetValue);
            PatternManager.Instance.CurrentPhase = Scoring.Phase.Normal;
            Debug.Log("Phase Normal");
        }

        // Spécifie quelle phase est en cours (Base ou Bonus)
        var phase = PatternManager.Instance.CurrentPhase;

        SpawnPlanetsAndAsteroids(
            phase == Scoring.Phase.Normal ? PatternManager.Instance.SelectedBasePattern
                                        : PatternManager.Instance.SelectedBonusPattern
                                        );
        Debug.Log("Planètes et astéroïdes générés");
    }

    private void SpawnPlanetsAndAsteroids(CombinationLib.PatternCombination pattern)
    {
        listPlanet.Clear();

        HashSet<Vector2> patternPositions = new HashSet<Vector2>();
        int patternPlanetID = -1;

        if (pattern != null)
        {
            foreach (Vector2 raw in pattern.positions)
                patternPositions.Add(raw);

            patternPlanetID = Random.Range(0, planetePrefab.Length);
        }
        else
        {
            Debug.Log("Aucun pattern spécifique sélectionné.");
        }

        foreach (Tuile tuile in toutesLesTuiles)
        {
            Vector2 tilePos = tuile.transform.position;

            if (pattern != null && PositionInPattern(tilePos, patternPositions))
            {
                chosenPlanetID = patternPlanetID;
                
            }
            else
            {
                // Choix d'une autre planète (différente de celle du pattern si défini)
                do
                {
                    chosenPlanetID = Random.Range(0, planetePrefab.Length);
                } while (pattern != null && chosenPlanetID == patternPlanetID);
            }

            // Instanciation planète
            GameObject planet = Instantiate(planetePrefab[chosenPlanetID], tuile.transform.position, Quaternion.identity);
            planet.SetActive(false);
            tuile.currentPlanet = planet;
            listPlanet.Add(planet);

            // Instanciation astéroïde
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
