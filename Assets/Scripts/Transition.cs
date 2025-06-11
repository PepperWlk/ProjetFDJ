using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour
{
    public GameObject[] fallAsteroidPrefab;
    public int fallAstroidCount = 100;
    public float transitionDuration = 1f;
    public string nextSceneName = "BonusScene";

    public void StartAsteroidTransition()
    {
        StartCoroutine(PlayAsteroidRainThenLoadScene());
    }
    private IEnumerator PlayAsteroidRainThenLoadScene()
    {
        for (int i = 0; i < fallAstroidCount; i++)
        {
            // Choisir une position X aléatoire sur toute la largeur de l'écran
            Vector2 randomScreenPos = new Vector2(Random.Range(0f, Screen.width), Screen.height);
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(randomScreenPos);
            Vector2 spawnPosition = new Vector2(worldPos.x, worldPos.y);
            GameObject asteroid = Instantiate(fallAsteroidPrefab[Random.Range(0, fallAsteroidPrefab.Length)], spawnPosition, Quaternion.identity);
            asteroid.transform.SetParent(transform);
            SpriteRenderer sr = asteroid.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = 1000; // Assurez-vous que les astéroïdes sont au-dessus des autres éléments
            }
        }
        Debug.Log("Test on est là0");
        yield return new WaitForSeconds(transitionDuration);

        SceneManagement.LoadBonusScene();
    }

}
