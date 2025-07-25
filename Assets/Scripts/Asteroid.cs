using UnityEngine;
using DG.Tweening;

public class Asteroid : MonoBehaviour
{
    private GameObject linkedPlanet;
    public GameObject explosionPrefab;
    public Scoring scoreScript;
    [SerializeField] private float floatAmount;
    [SerializeField] private float floatDuration;

    public void setLinkedPlanet(GameObject planete)
    {
        linkedPlanet = planete;
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Vector3 explosionPosition = transform.position;
            explosionPosition.z = -1;
            GameObject explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
            explosion.GetComponent<SpriteRenderer>().sortingOrder = 100; // Set the explosion to be above the asteroid
            new WaitForSeconds(0.5f);
            Destroy(explosion, 0.5f); // Destroy the explosion after 2 seconds
           
        }
    }

    private void Start()
    {
        floatAmount = Random.Range(0.5f, 1f); // Randomize the floating amount
        floatDuration = Random.Range(3f, 6f); // Randomize the floating duration
    }
    private void Update()
    {
        Floating();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Collider2D>() != null)
        {
            if (linkedPlanet != null)
                linkedPlanet.SetActive(true);

            Destroy(gameObject);
            Destroy(other.gameObject);

            if (scoreScript != null)
            {
                scoreScript.RegisterDestroyedAsteroid();
            }
        }
    }
    
    public void HandleClicked()
    {
        // détruire l’astéroïde
        Destroy(gameObject);
        Explode();

        // révéler la planète liée
        if (linkedPlanet != null)
        {
            linkedPlanet.SetActive(true);
        }
        // informer le scoring
        if (scoreScript != null)
            scoreScript.RegisterDestroyedAsteroid();
    }

    private void Floating()
    {
        // Only apply floating if not already tweening
        if (!DOTween.IsTweening(transform))
        {
            Vector3 targetPosition = transform.localPosition + new Vector3(0, floatAmount, 0);
            transform.DOLocalMoveY(targetPosition.y, floatDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
