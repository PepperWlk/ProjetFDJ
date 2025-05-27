using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private RectTransform rt;
    private Canvas canva;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        canva = GetComponentInParent<Canvas>();
        if (canva == null)
        {
            Debug.LogError("Reticle: No Canvas found in parent hierarchy.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = Input.mousePosition;
        rt.anchoredPosition = screenPos - (Vector2)canva.pixelRect.size * 0.5f;
    }
}
