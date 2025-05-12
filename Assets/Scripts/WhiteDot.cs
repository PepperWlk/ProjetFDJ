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
        foreach (GameObject dot in grilled)
        {
            Vector2 pos = dot.GetComponent<Transform>().position;
            
        }
    }


    private void drawWhiteDot(GameObject whitedot)
    {
        whitedot.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
    }
}

