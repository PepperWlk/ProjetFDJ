using UnityEngine;
using DG.Tweening;

public class Planet : MonoBehaviour
{
    public int id;
    public float blinkDuration = 0.5f;
    SpriteRenderer sr;

    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Debug.Log("Planet ID: " + id);
        if (sr == null)
        {
            Debug.LogWarning("SpriteRenderer not found on Planet object.");
            return;
        }
    }

    public void CheckPlanet()
    {
        if (sr != null && checkID() == true)
        {
            ShinePattern();
        }
        else
        {
            Debug.LogError("Planet ID does not match or SpriteRenderer is missing.");
        }
    }
    public void ShinePattern()
    {
        Debug.Log($"[Planet] ShinePattern appelé pour la planète ID={id}, SpriteRenderer existe={sr != null}");

        if (sr == null)
        {
            Debug.LogWarning($"[Planet] sr est null sur {name} (ID={id}).");
            return;
        }

        // On arrête les tweens précédents
        sr.DOKill();

        // Remet l'alpha à 1 pour repartir d’un état opaque
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);

        // Test rapide : un seul fade pour diagnostiquer
        float testDuration = 0.5f;
        sr.DOFade(0.2f, testDuration).OnComplete(() =>
        {
            sr.DOFade(1f, testDuration);
        });

        // Version “clignotement continu” (décommenter après avoir confirmé que le test marche)
        /*
        float fastBlinkDuration = blinkDuration * 0.3f;
        sr.DOFade(0.5f, fastBlinkDuration)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.Linear);
        */
    }


    public bool checkID()
    {
        if (id == GameManager.chosenPlanetID)
        {
            return true;
        }
        Debug.Log("Planet ID does not match the chosen ID.");
        return false;
    }
}
