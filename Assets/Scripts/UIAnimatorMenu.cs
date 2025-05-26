using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIAnimator : MonoBehaviour
{

    [Header("🚀 Vaisseau flottant")]
    public RectTransform circle;
    public float floatSpeedRotation;
    public float floatDurationCircle;

    [Header("🚀 Vaisseau flottant")]
    public RectTransform shipTransform;
    public float floatAmount = 20f;
    public float floatDuration = 2f;

    [Header("🎛️ Canvas Menus")]
    public CanvasGroup mainMenuCanvas;
    public CanvasGroup rulesCanvas1;
    public CanvasGroup rulesCanvas2;
    public CanvasGroup rulesCanvas3;

    [Header("🕹️ Transition Panel (noir)")]
    public CanvasGroup fadePanel;

    private void Start()
    {
        AnimateShip();
        SpinningCircle();
    }

    private void AnimateShip()
    {
        if (shipTransform == null) return;

        shipTransform.DOAnchorPosY(shipTransform.anchoredPosition.y + floatAmount, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void SpinningCircle()
    {
        if (circle == null) return;

        circle.DORotate(new Vector3(0, 0, 360), floatDurationCircle, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    } 

    public void ShowCanvas(CanvasGroup canvas)
    {
        canvas.gameObject.SetActive(true);
        canvas.alpha = 0f;
        canvas.transform.localScale = Vector3.zero;

        canvas.DOFade(1f, 0.4f);
        canvas.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
    }

    public void HideCanvas(CanvasGroup canvas)
    {
        canvas.DOFade(0f, 0.3f);
        canvas.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() => canvas.gameObject.SetActive(false));
    }

    // ✅ Unifie les deux fonctions
    public void FadeToGameScene(string sceneName)
    {
        fadePanel.gameObject.SetActive(true);
        fadePanel.alpha = 0f;
        fadePanel.blocksRaycasts = true;

        fadePanel.DOFade(1f, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
}
