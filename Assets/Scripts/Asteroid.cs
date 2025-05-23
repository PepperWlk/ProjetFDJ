using UnityEngine;

public class Asteroid : MonoBehaviour
{   
    private GameObject linkedPlanet;
    public Scoring scoreScript;

    public void setLinkedPlanet (GameObject planete)
    {
        linkedPlanet = planete;
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
