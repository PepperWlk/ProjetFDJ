using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine.InputSystem.iOS;


public static class CombinationLib 
{
    // Gap de 1.8 pour l'espacement des positions (Modifié dans l'éditeur!)
    // l'addition d'un vector2(-3, 1) = la position de spawn c.f gamemanager.cs
    private const float gap = 1.8f;

    public class PatternCombination 
    {
        public int value; // gain
        public Vector2[] positions; // liste des positions 
        public float chance; // probabilité de tomber sur ce pattern

        public PatternCombination(Vector2[] pattern, int score, float pourcentage)
        {
            this.positions = pattern;
            this.value = score;
            this.chance = pourcentage;
        }
    }

    public static readonly List<PatternCombination> combinations = new List<PatternCombination>
    {
        
        // 4 coins = 50e
        new PatternCombination(
            new Vector2[] 
            {
                new Vector2(0, 2) * gap + new Vector2(-3, 1), 
                new Vector2(-2, 0) * gap+ new Vector2(-3, 1), 
                new Vector2(2, 0) * gap+ new Vector2(-3, 1), 
                new Vector2(0, -2) * gap+ new Vector2(-3, 1)
            }, 
            10,
            0.0f / 100 
        ),

        // Coin haut, gauche, bas = 1e
        new PatternCombination(
            new Vector2[] 
            {
                new Vector2(0, 2) * gap+ new Vector2(-3, 1), 
                new Vector2(-2, 0) * gap+ new Vector2(-3, 1), 
                new Vector2(0, -2) * gap+ new Vector2(-3, 1)
            }, 
            1,
            0.0f / 100
        ),

        // Coin haut, droite, bas = 2e
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(0, 2) * gap + new Vector2(-3, 1), 
                new Vector2(2, 0) * gap + new Vector2(-3, 1), 
                new Vector2(0, -2) * gap + new Vector2(-3, 1)
            }, 
            2,
            0.0f / 100
        ),

        // 4 coins + centre = 10000e
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(0, 2) * gap + new Vector2(-3, 1), 
                new Vector2(-2, 0) * gap + new Vector2(-3, 1), 
                new Vector2(2, 0) * gap + new Vector2(-3, 1), 
                new Vector2(0, -2) * gap + new Vector2(-3, 1), 
                new Vector2(0, 0) * gap + new Vector2(-3, 1)
            }, 
            10000,
            0.0f / 100 // valeur changée pour tester à pas oublier de remettre à 0.0004f 
        ),

        // Bas, gauche, droite et centre = 100e
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-2, 0) * gap + new Vector2(-3, 1), 
                new Vector2(2, 0) * gap + new Vector2(-3, 1), 
                new Vector2(0, -2) * gap + new Vector2(-3, 1), 
                new Vector2(0, 0) * gap + new Vector2(-3, 1)
            }, 
            1000,
            0.0f / 100 
        ),

        // Gauche, centre, droite = 10e
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-2, 0) * gap + new Vector2(-3, 1), 
                new Vector2(2, 0) * gap + new Vector2(-3, 1), 
                new Vector2(0, 0) * gap + new Vector2(-3, 1)
            }, 
            5,
            0.0f / 100
        ),

        // Haut, droite, centre, gauche = 500e
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(0,0) * gap + new Vector2(-3, 1),
                new Vector2(-2, 0) * gap + new Vector2(-3, 1),
                new Vector2(2, 0) * gap + new Vector2(-3, 1),
                new Vector2(0, 2) * gap + new Vector2(-3, 1),
            }, 
            500,
            0.0f / 100
        ),

        // diagnonale = 100e
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-1, -1) * gap + new Vector2(-3, 1), 
                new Vector2(1, -1) * gap + new Vector2(-3, 1), 
                new Vector2(-1, 1) * gap + new Vector2(-3, 1),
                new Vector2(1, 1) * gap + new Vector2(-3, 1)
            }, 
            100,
            0.0f / 100
        )
    };

    public static PatternCombination ChoosePattern()
    {
        Debug.Log("Choix du pattern");
        float totalChance = 0f;
        foreach (var comb in combinations)
            totalChance += comb.chance;

        float rand = Random.Range(0f, 1f);
        float cumulative = 0f;

        foreach (var comb in combinations)
        {
            cumulative += comb.chance;
            if (rand < cumulative)
                return comb;
        }

        return null; // Aucun pattern sélectionné
    }

    public static float SecondChancePattern()
    {
        // check si il gagne un gain direct ou pas
        float score;
        float secondChance4 = 1f; // 3 chance sur 100 d'avoir 4e
        float secondChance500 = 0.5f; // 0.002 chance sur 100 d'avoir 500e
        float randomValue = Random.value;
        if (randomValue < secondChance4 && randomValue > secondChance500)
        {
            score = 4;
            Debug.Log("Gain de 4e");
        }
        else if (randomValue < secondChance500)
        {
            score = 500;
            Debug.Log("Gain de 500e");
        }
        else
        {
            return 0;
        }
        return score;
    }
}


    // Disposition des tuiles avec les planetes (donc pos des planetes)
    //                  new Vector2(0, 2),
    //         new Vector2(-1, 1), new Vector2(1, 1),
    //  new Vector2(-2, 0), new Vector2(0, 0), new Vector2(2, 0),
    //       new Vector2(-1, -1), new Vector2(1, -1),
    //                 new Vector2(0, -2)