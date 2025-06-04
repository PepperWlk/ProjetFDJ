using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    public UIAnimator uiAnimator; // à drag & drop dans l'inspecteur
    public CanvasGroup mainMenuCanvas;
    public CanvasGroup rulesCanvas;
    public string gameSceneName = "GameScene"; 

    // Appelé par le bouton "Play"
    public void OnPlayButton()
    {
        uiAnimator.FadeToGameScene(gameSceneName);
    }

    // Appelé par le bouton "Règles"
    public void OnRulesButton()
    {
        uiAnimator.HideCanvas(mainMenuCanvas);
        uiAnimator.ShowCanvas(rulesCanvas);
    }

    // Appelé par un bouton "Retour"
    public void OnBackToMenu()
    {
        uiAnimator.HideCanvas(rulesCanvas);
        uiAnimator.ShowCanvas(mainMenuCanvas);
    }
}
