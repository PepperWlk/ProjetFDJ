using UnityEngine;

public class AsteroidFall : MonoBehaviour
{
    public float fallSpeed;

    void Start()
    {
        fallSpeed = Random.Range(2f, 6f);
    }
    void Update()
    {
        Vector2 movement = (Vector2.down + new Vector2(-0.05f, 0f)) * fallSpeed * Time.deltaTime;
        transform.Translate(movement);

    }
}
