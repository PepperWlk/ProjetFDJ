using UnityEngine;
using DG.Tweening;

public class Planet : MonoBehaviour
{
    public int id;
    public float blinkDuration = 5f;
    SpriteRenderer sr;

    [SerializeField] private float floatDuration = 5f; // Duration of the floating animation

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

    public void Blink()
    {
        if (sr == null)
        {
            Debug.LogWarning("SpriteRenderer not found on Planet object.");
            return;
        }

        sr.DOFade(0f, blinkDuration / 2f)
            .SetLoops(-1, LoopType.Yoyo)
            .From(1f);
    }

    public void Scale()
    {
        if (sr == null)
        {
            Debug.LogWarning("SpriteRenderer not found on Planet object.");
            return;
        }

        // Scale the planet up and down
        transform.DOScale(2.6f, floatDuration / 2f)
            .SetLoops(-1, LoopType.Yoyo)
            .From(2.3f);
    }

}
