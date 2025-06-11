using System.Collections;
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
    public Phase currentPhase;
    public GameObject linePrefab;
    public List<GameObject> Allplanets;
    public TMP_Text scoreValue;
    [SerializeField] private RollingScore rollingScore;
    [SerializeField] private GroupColorChanger[] groupColorChanger;

    private List<Vector2[]> matchedPatterns = new List<Vector2[]>();

    public int totalAsteroid = 9;
    [SerializeField] private int DestroyedAsteroid = 0;

    private void Start()
    {
        currentPhase = Phase.Normal;
        UpdateScoreUI(ScoreManager.Instance.GetScore());
    }

    public void checkPatterns()
    {
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
        List<CombinationLib.PatternCombination> patternList = PatternManager.Instance.CurrentPhase == Phase.Normal
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
            }
        }

        // Appliquer le meilleur score s’il y a match
        Debug.Log("=== Contenu de planetMap ===");
        foreach (var kv in planetMap)
            Debug.Log($" key={kv.Key}  → planetID={kv.Value.id}");

        if (bestMatch != null)
        {
            DrawLineBetweenPattern(bestMatch.positions);

            int currentScore = ScoreManager.Instance.GetScore();
            int newScore;

            if (currentPhase == PatternManager.Instance.CurrentPhase)
            {
                newScore = Mathf.Max(currentScore, bestMatch.value); // garder le plus élevé
            }
            else // Phase.Bonus
            {
                newScore = currentScore + bestMatch.value; // additionner
            }

            foreach (GroupColorChanger group in groupColorChanger)
            {
                if (group.patternValue == bestMatch.value)
                {
                    group.ApplyWinningColor();
                }
                else
                {
                    group.ApplyLosingColor();
                }
            }
            HashSet<Vector2> matchedPositions = new HashSet<Vector2>();
            foreach (Vector2 pos in bestMatch.positions)
            {
                matchedPositions.Add(RoundPosition(pos));
            }

            foreach (Planet p in planetMap.Values)
            {
                Vector2 roundedPos = RoundPosition(p.transform.position);
                if (matchedPositions.Contains(roundedPos))
                {
                    p.Scale();
                }
                else
                {
                    p.Blink();
                }
            }

            

            ScoreManager.Instance.SetScore(newScore);
            Debug.Log($"Pattern trouvé ({currentPhase}) : +{bestMatch.value} points !");
            UpdateScoreUI(newScore);
        }
        else
        {
            Debug.Log("Aucun pattern trouvé.");
            foreach (GroupColorChanger group in groupColorChanger)
            {
                group.ApplyColor(Color.grey, group.parent);
            }
            foreach (GameObject planetObj in Allplanets)
            {
                Planet p = planetObj.GetComponent<Planet>();
                if (p != null)
                {
                    p.Blink(); // Faire clignoter les planètes
                }
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

    public void UpdateScoreUI(int score)
    {
        if (scoreValue != null && rollingScore != null)
        {
            StartCoroutine(rollingScore.RollScoreRoutine(score));
        }
        if (scoreValue != null)
        {
            scoreValue.text = score.ToString("D5"); // Afficher le score avec 5 chiffres
        }
        else
        {
            Debug.LogWarning("scoreValue est null, impossible de mettre à jour l'UI du score.");
        }
    }

    public void RegisterDestroyedAsteroid()
    {
        DestroyedAsteroid++;
        Debug.Log("Astéroïde détruit : " + DestroyedAsteroid + "check si fin de phase");
        if (DestroyedAsteroid >= totalAsteroid)
        {
            Debug.Log("Fin de phase détectée");
            StartCoroutine(DelayedPhaseTransition());
            Debug.Log("Vérification des patterns terminé");
            Debug.Log("Changement de phase");
        }
    }

    private IEnumerator DelayedPhaseTransition()
    {
        Debug.Log("⏳ Attente avant changement de phase...");
        yield return new WaitForSeconds(0.25f);

        Debug.Log("✅ Vérification des patterns");
        checkPatterns();

        if (rollingScore != null)
        {
            int score = (int)ScoreManager.Instance.GetScore();
            yield return StartCoroutine(rollingScore.RollScoreRoutine(score));  // ✅ attendre correctement
        }
        else
        {
            yield return new WaitForSeconds(2f);  // fallback si le script est manquant
        }

        // ✅ Changement de scène après animation terminée
        if (PatternManager.Instance.CurrentPhase == Phase.Normal)
        {
            PatternManager.Instance.CurrentPhase = Phase.Bonus;
            FindFirstObjectByType<Transition>().StartAsteroidTransition();
        }
        else
        {
            SceneManagement.LoadGameOver();
        }
    }

    private void DrawLineBetweenPattern(Vector2[] positions)
    {
        GameObject lineObj = Instantiate(linePrefab);
        lineObj.GetComponent<LineRenderer>().sortingOrder = 8;
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();
        lr.positionCount = positions.Length;
        for (int i = 0; i < positions.Length; i++)
        {
            Vector2 pos = RoundPosition(positions[i]);
            GameObject planetGO = Allplanets.Find(p => RoundPosition(p.transform.position) == pos);

            if (planetGO != null)
            {
                lr.SetPosition(i, planetGO.transform.position);
            }
            else
            {
                Debug.LogWarning($"Aucun GameObject trouvé pour la position {pos}. Assurez-vous que les planètes sont correctement positionnées.");
            }
        }
    }

}
