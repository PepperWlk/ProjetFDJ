using UnityEngine;
using DG.Tweening;

public class Planet : MonoBehaviour
{
    public int id;
    public float blinkDuration = 0.5f;
    SpriteRenderer sr;

    [SerializeField] private float floatAmount; // Amount to float up and down
    [SerializeField] private float floatDuration = 5f; // Duration of the floating animation

    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Debug.Log("Planet ID: " + id);
        floatAmount = Random.Range(0.5f, 1f);
        if (sr == null)
        {
            Debug.LogWarning("SpriteRenderer not found on Planet object.");
            return;
        }
    }

    public void Update()
    {
        // if (isActiveAndEnabled)
        // {
        //     Floating();
        // }
    }

    private void Floating()
    {
        // Only apply floating if not already tweening
        if (!DOTween.IsTweening(transform))
        {
            Vector3 targetPosition = transform.localPosition + new Vector3(0, floatAmount, 0);
            transform.DOLocalMoveY(targetPosition.y, floatDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }

}
