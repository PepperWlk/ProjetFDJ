using UnityEngine;

public class Projectile : MonoBehaviour {  
    public float speed = 50.0f;
    public float lifetime = 3.0f;  
    void Start()
    {
       // jouez un son au spawn ? 
    }
    private void Update()
    {
        transform.position = transform.position + new Vector3(0f, 1f, 0f) * speed * Time.deltaTime;

        lifetime -= Time.deltaTime;
        if (lifetime == 0){
            Destroy(gameObject);
        }
    }
}
