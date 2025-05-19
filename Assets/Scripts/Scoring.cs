using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour
{
    public enum Phase
    {
        Normal,
        Bonus,
    }
    public Phase currentPhase = Phase.Normal;
    public List<GameObject> Allplanets;
    public TMP_Text scoreValue;
    private List<Vector2[]> matchedPatterns = new List<Vector2[]>();

    public int totalAsteroid = 9;
    [SerializeField] private int DestroyedAsteroid = 0;

    private void Start()
    {
        UpdateScoreUI(ScoreManager.Instance.GetScore());
    }

    public void checkPatterns()
{
    bool hasPattern = false;
    Dictionary<Vector2, Planet> planetMap = new Dictionary<Vector2, Planet>();

    // Construction du planetMap
    foreach (GameObject planetObj in Allplanets)
    {
        if (planetObj.activeInHierarchy)
        {
            Vector2 pos = RoundPosition(planetObj.transform.position);
            Planet p = planetObj.GetComponent<Planet>();
            if (p != null && !planetMap.ContainsKey(pos))
            {
                planetMap[pos] = p;
            }
        }
    }

    CombinationLib.PatternCombination bestMatch = null;

    // Choisir la bonne liste de patterns selon la phase
    List<CombinationLib.PatternCombination> patternList = currentPhase == Phase.Normal
        ? CombinationLib.combinations
        : CombinationLib.bonuscombinations;

    // Recherche du meilleur pattern correspondant
    foreach (CombinationLib.PatternCombination pattern in patternList)
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
            hasPattern = true;
        }
    }

    // Appliquer le meilleur score s’il y a match
    if (bestMatch != null)
    {
        matchedPatterns.Add(bestMatch.positions);

        if (ScoreManager.Instance.GetScore() < bestMatch.value)
        {
            ScoreManager.Instance.SetScore(bestMatch.value);
            Debug.Log($"Nouveau pattern trouvé, +{bestMatch.value} points !");
            UpdateScoreUI(bestMatch.value);
        }
    }

    // Sinon, chance secondaire
    if (!hasPattern)
    {
        float secondChanceScore = CombinationLib.SecondChancePattern();
        if (ScoreManager.Instance.GetScore() < secondChanceScore)
        {
            ScoreManager.Instance.SetScore(secondChanceScore);
            Debug.Log($"Chance ! +{secondChanceScore} points !");
            UpdateScoreUI(secondChanceScore);
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

    private void UpdateScoreUI(float score)
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
        Debug.Log("Fin de phase détectée");
        checkPatterns();

        if (currentPhase == Phase.Normal)
        {
            // Lancer la phase bonus
            currentPhase = Phase.Bonus;
            Debug.Log("Chargement de la scène Bonus");
            SceneManagement.LoadBonusScene(); // ou SceneManager.LoadScene("BonusScene")
        }
        else
        {
            Debug.Log("Fin du Bonus : retour au menu ou fin de jeu");
            SceneManagement.LoadGameOver(); // À adapter selon ce que tu veux après la phase bonus
        }
    }
}

}
