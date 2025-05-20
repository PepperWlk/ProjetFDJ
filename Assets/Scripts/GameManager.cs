using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using System.Linq;

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

    private CombinationLib.PatternCombination GetRandomPattern(List<CombinationLib.PatternCombination> patterns)
    {
        return patterns[Random.Range(0, patterns.Count)];
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

    private List<List<CombinationLib.PatternCombination>> GetMatchingPatternSets(float targetValue)
    {
        var basePatterns = CombinationLib.combinations;
        var bonusPatterns = CombinationLib.bonuscombinations;

        CombinationLib.PatternCombination selectedBase = null;
        CombinationLib.PatternCombination selectedBonus = null;

        foreach (var basePattern in basePatterns)
        {
            if (Mathf.Approximately(basePattern.value, targetValue))
            {
                selectedBase = basePattern;
                selectedBonus = GetRandomPattern(bonusPatterns);
                return new List<List<CombinationLib.PatternCombination>> { new List<CombinationLib.PatternCombination> { selectedBase, selectedBonus } };
            }
        }

        foreach (var bonusPattern in bonusPatterns)
        {
            if (Mathf.Approximately(bonusPattern.value, targetValue))
            {
                selectedBonus = bonusPattern;
                selectedBase = GetRandomPattern(basePatterns);
                return new List<List<CombinationLib.PatternCombination>> { new List<CombinationLib.PatternCombination> { selectedBase, selectedBonus } };
            }
        }

        foreach (var basePattern in basePatterns)
        {
            foreach (var bonusPattern in bonusPatterns)
            {
                float total = basePattern.value + bonusPattern.value;
                if (Mathf.Approximately(total, targetValue))
                {
                    selectedBase = basePattern;
                    selectedBonus = bonusPattern;
                    return new List<List<CombinationLib.PatternCombination>> { new List<CombinationLib.PatternCombination> { selectedBase, selectedBonus } };
                }
            }
        }

        if (targetValue == 0f)
        {
            Debug.Log("Score ciblé = 0€, génération manuelle de planètes non matchées.");

            CombinationLib.PatternCombination fakeBase = new CombinationLib.PatternCombination(
                GenerateNonMatchingPattern(9, CombinationLib.combinations.Select(p => p.positions).ToList()).ToArray(),
                -1,
                0f
            );

            CombinationLib.PatternCombination fakeBonus = new CombinationLib.PatternCombination(
                GenerateNonMatchingPattern(9, CombinationLib.bonuscombinations.Select(p => p.positions).ToList()).ToArray(),
                -2,
                0f
            );

            return new List<List<CombinationLib.PatternCombination>> { new List<CombinationLib.PatternCombination> { fakeBase, fakeBonus } };
        }
        
        return new List<List<CombinationLib.PatternCombination>> { new List<CombinationLib.PatternCombination> { null, null } };    

    }

    public static List<Vector2> GenerateNonMatchingPattern(int count, List<Vector2[]> knownPatterns)
    {
        var result = new List<Vector2>();
        int attempts = 0;
        int maxAttempts = 100;

        while (result.Count < count && attempts < maxAttempts)
        {
            attempts++;
            Vector2 randomPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));

            // Round comme dans le reste du jeu pour rester cohérent
            randomPos = new Vector2(Mathf.Round(randomPos.x * 100f) / 100f, Mathf.Round(randomPos.y * 100f) / 100f);

            // Évite les doublons
            if (result.Contains(randomPos)) continue;

            result.Add(randomPos);

            // Vérifie si le nouveau set matche un pattern connu
            foreach (var pattern in knownPatterns)
            {
                if (IsSubset(result, pattern))
                {
                    result.Remove(randomPos); // Invalide, recommence
                    break;
                }
            }
        }

        return result;
    }

    // Compare si tous les points d’un pattern sont dans le résultat
    private static bool IsSubset(List<Vector2> positions, Vector2[] pattern)
    {
        foreach (var pos in pattern)
        {
            if (!positions.Contains(pos))
            {
                return false;
            }
        }
        return true;
    }

}
