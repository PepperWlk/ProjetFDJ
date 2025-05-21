using System.Collections.Generic;
using UnityEngine;

public static class CombinationLib
{
    public class PatternCombination
    {
        public Vector2[] positions;
        public int value;
        public float chance;

        public PatternCombination(Vector2[] positions, int value, float chance)
        {
            this.positions = positions;
            this.value = value;
            this.chance = chance;
        }
    }

    public static readonly List<PatternCombination> combinations = new List<PatternCombination>
    {
        // Croix : haut, bas, gauche, droite autour de (-3, -1)
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-3, 3), // haut
                new Vector2(-3, -5), // bas
                new Vector2(-9, -1), // gauche
            },
            1,
            0.40f
        ),

        // Ligne horizontale au centre
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-9 , -1),
                new Vector2(-3, -1),
                new Vector2(3, -1)
            },
            5,
            0.10f
        ),

        // carré diagonal
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(0, 1),
                new Vector2(-6, -3),
                new Vector2(0 , -3),
                new Vector2(-6, 1)
            },
            100,
            0.005f
        ),

        // Diagonale haut-gauche → bas-droite
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-3, 3),
                new Vector2(3, -1),
                new Vector2(-3, -5)
            },
            2,
            0.30f
        ),

        // Full centre + croix
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-3, 3),
                new Vector2(-9, -1),
                new Vector2(3, -1),
                new Vector2(-3, -5)
            },
            10,
            0.05f
        ),

        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-9, -1),
                new Vector2(-3, -1),
                new Vector2(3, -1),
                new Vector2(-3, -5)
            },
            1000,
            0.001f
        ),

        // Full centre + croix
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-3, 3),
                new Vector2(-9, -1),
                new Vector2(-3, -1),
                new Vector2(3, -1),
            },
            500,
            0.0005f
        ),

        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-3, 3),
                new Vector2(-9, -1),
                new Vector2(-3, -1),
                new Vector2(3, -1),
                new Vector2(-3, -5)
            },
            10000,
            0.0001f
        )
    };

    public static readonly List<PatternCombination> bonuscombinations = new List<PatternCombination>
    {
        // Bonus simple : paire horizontale
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-9, -1),
                new Vector2(3, -1)
            },
            2,
            0.15f
        ),

        // Bonus diagonale courte
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-3, 3),
                new Vector2(-3, -1),
                new Vector2(-3, -5)
            },
            10,
            0.10f
        ),

        new PatternCombination
        (
            new Vector2[]
            {
                new Vector2 (-3, 3),
                new Vector2 (0, 1),
                new Vector2 (-6, 1)
            },
            1,
            0.70f
        ),

        // carré diagonal
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(0, 1),
                new Vector2(-6, -3),
                new Vector2(0 , -3),
                new Vector2(-6, 1)
            },
            10000,
            0.005f
        ),
    };

    public static PatternCombination ChoosePattern()
    {
        float totalChance = 0f;
        foreach (var comb in combinations)
            totalChance += comb.chance;

        float rand = Random.value;
        float cumulative = 0f;

        foreach (var comb in combinations)
        {
            cumulative += comb.chance;
            if (rand < cumulative)
                return comb;
        }

        return null;
    }

    public static PatternCombination ChooseBonusPattern()
    {
        Debug.Log("ChoosePattern début");
        float totalChance = 0f;
        foreach (var comb in bonuscombinations)
            totalChance += comb.chance;

        float rand = Random.value;
        float cumulative = 0f;

        foreach (var comb in bonuscombinations)
        {
            cumulative += comb.chance;
            if (rand < cumulative)
                Debug.Log("ChoosePattern fin pattern");
                return comb;
        }
        Debug.Log("ChoosePattern fin rien");
        return null;
    }

    public static float SecondChancePattern()
    {
        float score;
        float secondChance4 = 0.03f;
        float secondChance500 = 0.0002f;
        float randomValue = Random.value;

        if (randomValue < secondChance500)
        {
            score = 500;
            Debug.Log("Gain exceptionnel de 500€ !");
        }
        else if (randomValue < secondChance500 + secondChance4)
        {
            score = 4;
            Debug.Log("Gain de 4€ !");
        }
        else
        {
            return 0;
        }

        return score;
    }
}
