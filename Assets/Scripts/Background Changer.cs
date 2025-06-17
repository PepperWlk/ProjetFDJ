using UnityEngine;
using UnityEngine.Video;

public class BackgroundChanger : MonoBehaviour
{
    public VideoPlayer TransitionVideo;

    public void ChangeActivity()
    {
        if (gameObject.activeInHierarchy == true)
        {
            gameObject.SetActive(false);
            ShowTransitionVideo();
        }
        else
        {
            Debug.LogWarning("BackgroundChanger: Le GameObject n'est pas actif.");
        }
    }

    private void ShowTransitionVideo()
    {
        if (TransitionVideo != null)
        {
            TransitionVideo.gameObject.SetActive(true);
            TransitionVideo.Play();
        }
        else
        {
            Debug.LogError("BackgroundChanger: TransitionVideo n'est pas assigné.");
        }
    }
}
