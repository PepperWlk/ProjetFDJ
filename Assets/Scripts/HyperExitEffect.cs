using UnityEngine;
using System.Collections;

public class HyperExitEffect2D : MonoBehaviour
{
    private float targetSize = 10f;
    private float zoomInDuration = 0.1f;
    private float zoomOutDuration = 0.3f;

    private float shakeDuration = 0.3f;
    private float shakeMagnitude = 0.3f;

    private Camera cam;
    private float originalSize;
    private Vector3 originalPos;

    void Start()
    {
        cam = Camera.main;
        if (cam == null || !cam.orthographic)
        {
            Debug.LogWarning("HyperExitEffect2D: La caméra doit être orthographique.");
            return;
        }

        originalSize = cam.orthographicSize;
        originalPos = cam.transform.position;

        StartCoroutine(PlayExitEffect());
    }

    IEnumerator PlayExitEffect()
    {
        float elapsed = 0f;

        // ZOOM IN
        while (elapsed < zoomInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomInDuration;
            cam.orthographicSize = Mathf.Lerp(originalSize, targetSize, t);
            yield return null;
        }

        // ZOOM OUT + SHAKE
        elapsed = 0f;
        float overshootSize = originalSize * 1.05f;

        while (elapsed < zoomOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomOutDuration;
            cam.orthographicSize = Mathf.SmoothStep(targetSize, overshootSize, t);

            // Shake
            if (elapsed < shakeDuration)
            {
                Vector2 shakeOffset = Random.insideUnitCircle * shakeMagnitude;
                cam.transform.position = originalPos + new Vector3(shakeOffset.x, shakeOffset.y, 0);
            }

            yield return null;
        }

        // Reset position and size
        cam.orthographicSize = originalSize;
        cam.transform.position = originalPos;
    }
}
