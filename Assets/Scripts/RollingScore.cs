using System.Collections;
using TMPro;
using UnityEngine;

public class RollingScore : MonoBehaviour
{
    public TextMeshProUGUI[] digits;
    public int finalScore = 0;
    public float rollDuration = 1.5f;
    public float delayBetweenDigits = 0.2f;

    // ✅ Appel depuis l’extérieur
    public IEnumerator RollScoreRoutine(int score)
    {
        finalScore = score;
        yield return StartCoroutine(RollDigits());
    }

    // ❌ Ne pas appeler ça depuis l’extérieur
    private IEnumerator RollDigits()
    {
        string scoreString = finalScore.ToString("D5");
        Coroutine[] digitCoroutines = new Coroutine[digits.Length];

        // Lance chaque chiffre et garde la coroutine
        for (int i = 0; i < digits.Length; i++)
        {
            digitCoroutines[i] = StartCoroutine(RollSingleDigit(digits[i], scoreString[i], i * delayBetweenDigits));
        }

        // ✅ Attendre la fin de TOUS les chiffres
        foreach (Coroutine coroutine in digitCoroutines)
        {
            yield return coroutine;
        }
    }

    private IEnumerator RollSingleDigit(TextMeshProUGUI digitText, char finalChar, float delay)
    {
        yield return new WaitForSeconds(delay);

        float timer = 0f;
        while (timer < rollDuration)
        {
            digitText.text = Random.Range(0, 10).ToString();
            timer += Time.deltaTime * 10f;
            yield return new WaitForSeconds(0.05f);
        }

        digitText.text = finalChar.ToString();
    }
}
