using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Transition : MonoBehaviour
{
    public string nextSceneName;
    public float speed = 10f;
    public float sceneChangeX = 0f;
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
        Debug.Log($"[Transition] Loading scene: {nextSceneName}");
        DontDestroyOnLoad(gameObject);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        yield return new WaitUntil(() => asyncLoad.isDone);
        // Le vaisseau continue naturellement dans la nouvelle scène
    }
}
