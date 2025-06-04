using UnityEngine;

public class GameInput : MonoBehaviour
{   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Toucher TOUT collider2D (même triggers)
            Collider2D col = Physics2D.OverlapPoint(worldPoint);
            if (col != null)
            {
                Asteroid ast = col.GetComponent<Asteroid>();
                if (ast != null)
                    ast.HandleClicked();
            }
        }
    }

}
