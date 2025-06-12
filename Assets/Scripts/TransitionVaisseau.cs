using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionVaisseau : MonoBehaviour
{
    public string nextSceneName;
    public float speed = 10f;
    public float sceneChangeX = 3.5f;
    public float destroyX = 200f;

    private bool hasLoadedScene = false;

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (!hasLoadedScene && transform.position.x >= sceneChangeX)
        {
            hasLoadedScene = true;
            StartCoroutine(LoadSceneAfterDelay());
        }

        if (transform.position.x >= destroyX)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator LoadSceneAfterDelay()
    {
        DontDestroyOnLoad(gameObject);
        if (nextSceneName == null || nextSceneName != "BonusScene")
        {
            SceneManagement.LoadMainMenu();
        }
        else
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
            yield return new WaitUntil(() => asyncLoad.isDone);
        }
        // Le vaisseau continue naturellement dans la nouvelle scène
    }
}
