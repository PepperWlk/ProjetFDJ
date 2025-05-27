using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void QuitGame()
    {
        // If the game is running in the editor, stop play mode
        // Otherwise, quit the application
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
            Debug.Log("Game is exiting...");
        }
    }

    // Reload the current scene pour refaire les tests c'est carré
    public static void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Debug.Log("Reloading scene: " + currentScene.name);
    }


// Méthodes pour charger les scènes spécifiques


    public static void LoadGameScene()
    {
        LoadScene("GameScene");
    }

    // Plus explicite pour moi, load le menu principal
    public static void LoadMainMenu()
    {
        LoadScene("MainMenu");
        ScoreManager.Instance.SetScore(0);
    }

    public static void LoadBonusScene()
    {
        Debug.Log("[SceneManagement] Préparation chargement scène bonus");

        var score = ScoreManager.Instance.GetScore();
        Debug.Log($"[SceneManagement] Score actuel : {score}");

        PatternManager.Instance.CurrentPhase = Scoring.Phase.Bonus;

        LoadScene("BonusScene");
    }




    public static void LoadRuleScene()
    {
        LoadScene("RuleScene");
    }
    
    public static void LoadGameOver()
    {
        Debug.Log("[SceneManagement] Chargement de l'écran de fin de jeu");
        
        // 🔄 Reset de l’état
        PatternManager.Instance.CurrentPhase = Scoring.Phase.Normal;
        LoadScene("GameOver");
    }


}
