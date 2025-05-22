using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public static PatternManager Instance;

    public float TargetValue { get; private set; } = 0f;
    public int? ticketValue { get; private set; } = null;

    public CombinationLib.PatternCombination SelectedBasePattern { get; private set; }
    public CombinationLib.PatternCombination SelectedBonusPattern { get; private set; }

    public PrizeManager prizeManager;

    public Scoring.Phase CurrentPhase { get; set; } = Scoring.Phase.Normal; // Utilise l’enum de Scoring

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (prizeManager == null)
        {
            prizeManager = FindFirstObjectByType<PrizeManager>();
            if (prizeManager == null)
                Debug.LogError("PrizeManager introuvable après chargement !");
        }
    }

    public CombinationLib.PatternCombination GetCurrentPattern()
    {
        return CurrentPhase == Scoring.Phase.Normal ? SelectedBasePattern : SelectedBonusPattern;
    }

    public void SelectPatterns(float targetValue)
    {
        Debug.Log($"[PatternManager] Sélection pour valeur cible : {targetValue}");

        TargetValue = targetValue;

        if (prizeManager != null && (targetValue == 1 || targetValue == 2 || targetValue == 10 || targetValue == 10000) && PatternManager.Instance.CurrentPhase == Scoring.Phase.Normal)
        {
            ticketValue = prizeManager.DrawTicket(targetValue);
        }
        else
        {
            Debug.LogError("Either PrizeManager is null or targetValue is not valid.");
        }

        var basePatterns = CombinationLib.combinations;
        var bonusPatterns = CombinationLib.bonuscombinations;

        SelectedBasePattern = null;
        SelectedBonusPattern = null;

        if (ticketValue == 0)
        {
            // Cas 4 : Score = 0, on force des patterns qui ne matchent pas
            if (Mathf.Approximately(targetValue, 0f))
            {
                var baseFake = GenerateNonMatchingPattern(9, basePatterns);
                var bonusFake = GenerateNonMatchingPattern(9, bonusPatterns);

                if (baseFake == null || bonusFake == null)
                {
                    Debug.LogError("❌ Impossible de générer des patterns non matchés !");
                    return; // Ou gérer autrement, mais NE PAS continuer
                }

                SelectedBasePattern = new CombinationLib.PatternCombination(baseFake, -1, 0f);
                SelectedBonusPattern = new CombinationLib.PatternCombination(bonusFake, -2, 0f);

                Debug.Log("[PatternManager] Cas 4 (0) sélectionné");
                return;
            }
            else
            {
                Debug.Log("[PatternManager] Cas 4 (0) sélectionné");
                SelectedBasePattern = new CombinationLib.PatternCombination(
                    GenerateNonMatchingPattern(9, basePatterns), -1, 0f);
                SelectedBonusPattern = new CombinationLib.PatternCombination(
                    GenerateNonMatchingPattern(9, bonusPatterns), -2, 0f);
                return;
            }
        }

        if (ticketValue == 1 || ticketValue == null)
        {
            // Cas 1 : Base = Target
            Debug.Log("Cas 1");
            foreach (var basePattern in basePatterns)
            {
                if (Mathf.Approximately(basePattern.value, targetValue))
                {
                    SelectedBasePattern = basePattern;
                    SelectedBonusPattern = new CombinationLib.PatternCombination(
                    GenerateNonMatchingPattern(9, bonusPatterns), -1, 0f);
                    return;
                }
            }
        }

        if (ticketValue == 2)
        {
            // Cas 2 : Bonus = Target
            Debug.Log("Cas 2");
            foreach (var bonusPattern in bonusPatterns)
            {
                if (Mathf.Approximately(bonusPattern.value, targetValue))
                {
                    SelectedBonusPattern = bonusPattern;
                    SelectedBasePattern = new CombinationLib.PatternCombination(
                    GenerateNonMatchingPattern(9, basePatterns), -1, 0f);
                    return;
                }
            }
        }


        if (ticketValue == 3)
        {
            // Cas 3 : Base + Bonus = Target
            Debug.Log("Cas 3");
            foreach (var basePattern in basePatterns)
            {
                foreach (var bonusPattern in bonusPatterns)
                {
                    if (Mathf.Approximately(basePattern.value + bonusPattern.value, targetValue))
                    {
                        SelectedBasePattern = basePattern;
                        SelectedBonusPattern = bonusPattern;
                        return;
                    }
                }
            }
        }
        Debug.LogWarning("⚠️ Aucun pattern trouvé avec la stratégie tirée — fallback vers 0€ patterns.");
        SelectedBasePattern = new CombinationLib.PatternCombination(GenerateNonMatchingPattern(9, basePatterns), -1, 0f);
        SelectedBonusPattern = new CombinationLib.PatternCombination(GenerateNonMatchingPattern(9, bonusPatterns), -2, 0f);
    }

    private CombinationLib.PatternCombination GetRandom(List<CombinationLib.PatternCombination> list)
    {
        if (list == null || list.Count == 0) return null;
        return list[Random.Range(0, list.Count)];
    }

    private Vector2[] GenerateNonMatchingPattern(int count, List<CombinationLib.PatternCombination> known)
        {
            var positions = new List<Vector2>();
            int attempts = 0;
            int maxAttempts = 200; // Limite pour éviter le gel
            var knownPosSets = known.Select(p => p.positions).ToList();

            while (positions.Count < count && attempts < maxAttempts)
            {
                attempts++;
                Vector2 pos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                pos = new Vector2(Mathf.Round(pos.x * 100f) / 100f, Mathf.Round(pos.y * 100f) / 100f);

                if (positions.Contains(pos)) continue;
                positions.Add(pos);

                foreach (var knownSet in knownPosSets)
                {
                    if (IsSubset(positions, knownSet))
                    {
                        positions.Remove(pos);
                        break;
                    }
                }
            }

            if (positions.Count < count)
            {
                Debug.LogWarning("⛔ Trop d'essais pour générer un pattern non-matché !");
                return null;
            }

            return positions.ToArray();
        }



    private bool IsSubset(List<Vector2> sub, Vector2[] full)
    {
        var fullSet = new HashSet<Vector2>(full);
        foreach (var pos in sub)
        {
            if (!fullSet.Contains(pos)) return false;
        }
        return true;
    }



}

