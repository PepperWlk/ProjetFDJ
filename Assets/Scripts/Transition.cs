using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public float spawnRate = 0.1f;
    public float duration = 3f;
    public float spawnAreaWidth = 20f;
    private float timer;

    public IEnumerator SpawnAsteroidsRain()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(-spawnAreaWidth, spawnAreaWidth),
                Camera.main.transform.position.y + 10,
                0);
            Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
            elapsed += spawnRate;
        }
        yield return new WaitForSeconds(1f); // Wait before ending the transition
        SceneManagement.LoadBonusScene();
    }
}

