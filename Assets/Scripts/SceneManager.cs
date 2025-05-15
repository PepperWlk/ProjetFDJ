using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Méthode pour charger une scène spécifique

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
    }

    public static void LoadGameOver()
    {
        LoadScene("EndScene");
    }

    public static void LoadRuleScene()
    {
        LoadScene("RuleScene");
    }

}
