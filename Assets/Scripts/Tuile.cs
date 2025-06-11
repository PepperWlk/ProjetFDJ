using UnityEngine;

public class Tuile : MonoBehaviour
{
    public GameObject currentPlanet;
    public bool isOccupied => currentPlanet != null;

    private SpriteRenderer sr;

    public Vector2 GetGridPosition()
    {
        // Convertir la position du GameObject en coordonnées de grille
        Vector2 gridPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        return gridPos;
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer component is missing on the Tuile GameObject.");
        }
        else 
        {
            sr.color = new Color32(255, 255, 255, 0); // Set the default color to white with 0 alpha
        }
    }
}
