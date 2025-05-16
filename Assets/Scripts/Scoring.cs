using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour
{
    public List<GameObject> Allplanets;
    [SerializeField] private float score = 0;
    public TMP_Text scoreValue;

    public int totalAsteroid = 9;
    [SerializeField] private int DestroyedAsteroid = 0;

    private List<Vector2[]> matchedPatterns = new List<Vector2[]>();

    public void checkPatterns()
    {
        Dictionary<Vector2, Planet> planetMap = new Dictionary<Vector2, Planet>();

        foreach (GameObject planetObj in Allplanets)
        {
            if (planetObj.activeInHierarchy)
            {
                Vector2 pos = RoundPosition(planetObj.transform.position);
                Planet p = planetObj.GetComponent<Planet>();
                if (p != null)
                {
                    planetMap[pos] = p;
                }
            }
        }

        CombinationLib.PatternCombination bestMatch = null;

        foreach (var pattern in CombinationLib.combinations)
        {
            if (!HasBeenMatched(pattern.positions) && IsPatternMatched(pattern.positions, planetMap))
            {
                if (bestMatch == null || pattern.value > bestMatch.value)
                {
                    bestMatch = pattern;
                }
            }
        }

        if (bestMatch != null)
        {
            matchedPatterns.Add(bestMatch.positions);
            score = Mathf.Max(score, bestMatch.value);
            Debug.Log($"✔️ Pattern trouvé: +{bestMatch.value}€");
            UpdateScoreUI();
        }
        else
        {
            float chanceScore = CombinationLib.SecondChancePattern();
            if (chanceScore > score)
            {
                score = chanceScore;
                Debug.Log($"🎲 Bonus aléatoire: +{chanceScore}€");
                UpdateScoreUI();
            }
            else
            {
                Debug.Log("❌ Aucun pattern détecté.");
            }
        }
    }

    private bool IsPatternMatched(Vector2[] pattern, Dictionary<Vector2, Planet> map)
    {
        Planet reference = null;
        foreach (Vector2 rawPos in pattern)
        {
            Vector2 pos = RoundPosition(rawPos);
            if (!map.TryGetValue(pos, out Planet p)) return false;

            if (reference == null) reference = p;
            else if (p.id != reference.id) return false;
        }
        return true;
    }

    private bool HasBeenMatched(Vector2[] pattern)
    {
        foreach (var matched in matchedPatterns)
        {
            if (ArePatternsEqual(matched, pattern)) return true;
        }
        return false;
    }

    private bool ArePatternsEqual(Vector2[] a, Vector2[] b)
    {
        return new HashSet<Vector2>(a).SetEquals(b);
    }

    private Vector2 RoundPosition(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }

    private void UpdateScoreUI()
    {
        if (scoreValue != null)
        {
            scoreValue.text = $"Gain : {score}€";
        }
    }

    public void RegisterDestroyedAsteroid()
    {
        DestroyedAsteroid++;
        if (DestroyedAsteroid >= totalAsteroid)
        {
            Debug.Log("🚀 Fin du niveau - Vérification des patterns...");
            checkPatterns();
            SceneManagement.LoadBonusScene(); // ou ton système de transition
        }
    }
}
