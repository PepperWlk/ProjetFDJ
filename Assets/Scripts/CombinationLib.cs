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
        public int value;
        public Vector2[] positions;

        public float droprate;

        public PatternCombination(Vector2[] pattern, int score, float pourcentage)
        {
            this.positions = pattern;
            this.value = score;
            this.droprate = pourcentage;
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
            50,
            0.1f
        ),

        // Coin haut, droite, bas = 1e
        new PatternCombination(
            new Vector2[] 
            {
                new Vector2(0, 2) * gap+ new Vector2(-3, 1), 
                new Vector2(-2, 0) * gap+ new Vector2(-3, 1), 
                new Vector2(0, -2) * gap+ new Vector2(-3, 1)
            }, 
            1,
            20.0f
        ),

        // Coin haut, gauche, bas = 2e
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(0, 2) * gap + new Vector2(-3, 1), 
                new Vector2(2, 0) * gap + new Vector2(-3, 1), 
                new Vector2(0, -2) * gap + new Vector2(-3, 1)
            }, 
            2,
            8.0f
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
            0.0004f
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
            100,
            0.02f
        ),

        // Gauche, centre, droite = 10e
        new PatternCombination(
            new Vector2[]
            {
                new Vector2(-2, 0) * gap + new Vector2(-3, 1), 
                new Vector2(2, 0) * gap + new Vector2(-3, 1), 
                new Vector2(0, 0) * gap + new Vector2(-3, 1)
            }, 
            10,
            1.0f
        )
    };
}


    // Disposition des tuiles avec les planetes (donc pos des planetes)
    //                  new Vector2(0, 2),
    //         new Vector2(-1, 1), new Vector2(1, 1),
    //  new Vector2(-2, 0), new Vector2(0, 0), new Vector2(2, 0),
    //       new Vector2(-1, -1), new Vector2(1, -1),
    //                 new Vector2(0, -2)