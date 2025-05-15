using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour
{
    public List<GameObject> Allplanets;
    [SerializeField] private float score = 0;
    public TMP_Text scoreValue;
    private List<Vector2[]> matchedPatterns = new List<Vector2[]>();

    public int totalAsteroid = 9;
    [SerializeField] private int DestroyedAsteroid = 0;

    public void checkPatterns()
    {
        bool  haspattern = false;
        Dictionary<Vector2, Planet> planetMap = new Dictionary<Vector2, Planet>();

        foreach (GameObject planetobj in Allplanets)
        {
            if (planetobj.activeInHierarchy)
            {
                Vector2 pos = RoundPosition(planetobj.transform.position);
                Planet p = planetobj.GetComponent<Planet>();
                if (p != null && !planetMap.ContainsKey(pos))
                {
                    planetMap[pos] = p;
                }
            }
        }

        CombinationLib.PatternCombination bestMatch = null;

        foreach (CombinationLib.PatternCombination pattern in CombinationLib.combinations)
        {
            if (IsMatch(pattern.positions, planetMap))
            {
                if (!HasBeenMatched(pattern.positions))
                {
                    if (bestMatch == null || pattern.value > bestMatch.value)
                    {
                        bestMatch = pattern;
                    }
                }
                haspattern = true;
            }
        }

        if (bestMatch != null)
        {
            matchedPatterns.Add(bestMatch.positions);
            if (score < bestMatch.value)
            {
                score = bestMatch.value;
            }
            Debug.Log($"Nouveau pattern trouvé, +{bestMatch.value} points !");
            UpdateScoreUI();
        }

        if (!haspattern)
        {
            float secondchancescore = CombinationLib.SecondChancePattern();
            if (score < secondchancescore)
            {
                score = secondchancescore;
                Debug.Log($"Chance ! +{secondchancescore} points !");
                UpdateScoreUI();
            }
        }
    }

    private bool IsMatch(Vector2[] pattern, Dictionary<Vector2, Planet> map)
    {
        Planet first = null;
        foreach (Vector2 pos in pattern)
        {
            Vector2 roundedPos = RoundPosition(pos);
            if (!map.TryGetValue(roundedPos, out Planet planet))
            {
                return false;
            }

            if (first == null)
            {
                first = planet;
            }
            else if (planet.id != first.id)
            {
                return false;
            }
        }
        return true;
    }

    private bool HasBeenMatched(Vector2[] pattern)
    {
        foreach (Vector2[] matched in matchedPatterns)
        {
            if (ArePatternsEqual(pattern, matched))
                return true;
        }
        return false;
    }

    private bool ArePatternsEqual(Vector2[] a, Vector2[] b)
    {
        if (a.Length != b.Length) return false;

        HashSet<Vector2> setA = new HashSet<Vector2>(a);
        HashSet<Vector2> setB = new HashSet<Vector2>(b);
        return setA.SetEquals(setB);
    }

    private Vector2 RoundPosition(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x * 100f) / 100f, Mathf.Round(position.y * 100f) / 100f);
    }

    private void UpdateScoreUI()
    {
        if (scoreValue != null)
        {
            scoreValue.text = "Gain : " + score.ToString() +"€";
        }
    }

    public void RegisterDestroyedAsteroid()
    {
        DestroyedAsteroid++;
        if (DestroyedAsteroid >= totalAsteroid)
        {
            Debug.Log("Fin du jeu");
            checkPatterns();
            SceneManagement.LoadBonusScene();
        }
    }
}
