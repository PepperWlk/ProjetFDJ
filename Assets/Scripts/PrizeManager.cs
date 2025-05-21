using UnityEngine;

public class PrizeManager : MonoBehaviour
{

    public static PrizeManager Instance;
    // Pour avoir un singleton et ne pas détruire l'objet entre les scènes
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Croupier
    public static int DrawPrize()
    {
        float roll = Random.Range(0, 9000000);

        if (roll < 2) { return 10000; } // Probabilité de 2 / 9 000 000 ≈ 0.000022%
        if (roll < 2 + 5) { return 1000; } // Probabilité de 5 / 9 000 000 ≈ 0.000056%
        if (roll < 2 + 5 + 16950) { return 500; } // Probabilité de 16 950 / 9 000 000 ≈ 0.188%
        if (roll < 2 + 5 + 16950 + 100000) { return 100; } // Probabilité de 100 000 / 9 000 000 ≈ 1.11%
        if (roll < 2 + 5 + 16950 + 100000 + 250000) { return 10; } // Probabilité de 250 000 / 9 000 000 ≈ 2.78%
        if (roll < 2 + 5 + 16950 + 100000 + 250000 + 500000) { return 5; } // Probabilité de 500 000 / 9 000 000 ≈ 5.56%
        if (roll < 2 + 5 + 16950 + 100000 + 250000 + 500000 + 1000000) { return 2; } // Probabilité de 1 000 000 / 9 000 000 ≈ 11.11%
        if (roll < 2 + 5 + 16950 + 100000 + 250000 + 500000 + 1000000 + 6000000) { return 1; } // Probabilité de 6 000 000 / 9 000 000 ≈ 66.67%
        return 0;
    }
}
