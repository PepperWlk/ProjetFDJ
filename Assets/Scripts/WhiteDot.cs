using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WhiteDot : MonoBehaviour
{
    public GameObject greydotPrefab;
    public List<Vector2> SpawnPositions = new List<Vector2>();
    private List<GameObject> grilled = new List<GameObject>();


    public void SpawnWhiteDot(List<Vector2> positions)
    {
        foreach (Vector2 position in positions)
        {
            GameObject whitedot = Instantiate(greydotPrefab, position, Quaternion.identity);
            whitedot.transform.SetParent(transform);
            whitedot.GetComponent<RectTransform>().anchoredPosition = position;
            whitedot.GetComponent<Image>().color = new Color(131, 134, 159, 0.5f);
        }

    }

    private void setUpSpawnPosition()
    {
        HashSet<Vector2> positions = new HashSet<Vector2>();
        
        // Définir les offsets pour dessiner un pattern autour du point central
        Vector2[] offsets = new Vector2[]
        {
        new Vector2(0, 0),      // centre
        new Vector2(20, 0),     // droite
        new Vector2(-20, 0),    // gauche
        new Vector2(0, 20),     // haut
        new Vector2(0, -20),    // bas
        new Vector2(20, 20),    // haut droite
        new Vector2(-20, 20),   // haut gauche
        new Vector2(20, -20),   // bas droite
        new Vector2(-20, -20)   // bas gauche
        };
        foreach (GameObject dot in grilled)
        {
            Vector2 centerPos = dot.GetComponent<RectTransform>().anchoredPosition;
            
            foreach (var offset in offsets)
            {
            Vector2 spawnPos = centerPos + offset;
            GameObject greydot = Instantiate(greydotPrefab, Vector3.zero, Quaternion.identity);
            greydot.transform.SetParent(transform);
            greydot.GetComponent<RectTransform>().anchoredPosition = spawnPos;
            }
        }
    }


    private void drawWhiteDot(GameObject whitedot)
    {
        whitedot.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
    }
}

