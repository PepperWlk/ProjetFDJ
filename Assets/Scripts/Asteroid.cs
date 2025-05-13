using UnityEngine;

public class Asteroid : MonoBehaviour
{   
    private GameObject linkedPlanet;
    public Scoring scoreScript;
    private void Start()
    {

    }

    private void Update()
    {
        
    }

    public void setLinkedPlanet (GameObject planete)
    {
        linkedPlanet = planete;
        Debug.Log("Planete liée");
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
}
