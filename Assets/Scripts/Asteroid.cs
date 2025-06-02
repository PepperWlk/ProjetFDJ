using UnityEngine;
using DG.Tweening;

public class Asteroid : MonoBehaviour
{
    private GameObject linkedPlanet;
    public Scoring scoreScript;
    [SerializeField] private float floatAmount;
    [SerializeField] private float floatDuration = 5f;

    public void setLinkedPlanet(GameObject planete)
    {
        linkedPlanet = planete;
    }

    private void Start()
    {
        floatAmount = Random.Range(0.5f, 1f); // Randomize the floating amount
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

        // révéler la planète liée
        if (linkedPlanet != null)
        {
            linkedPlanet.SetActive(true);
            Planet p = linkedPlanet.GetComponent<Planet>();
            if (p != null)
            {
                p.ShinePattern();
            }
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
