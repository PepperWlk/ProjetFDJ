using UnityEngine;

public class Tuile : MonoBehaviour
{
    public GameObject currentPlanet;
    public bool isOccupied => currentPlanet != null;

    public Vector2 GetGridPosition()
    {
        // Convertir la position du GameObject en coordonnées de grille
        Vector2 gridPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        return gridPos;
    }
}
