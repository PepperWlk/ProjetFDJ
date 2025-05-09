using UnityEngine;

public class SpriteMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject Projectile;
    private Vector2 targetPosition;
    private bool hasShoot = false;


    private void Update()
    {
        Vector2 moveDirection = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
            moveDirection = Vector2.left;
        if (Input.GetKey(KeyCode.RightArrow))
            moveDirection = Vector2.right;
        if (Input.GetKey(KeyCode.Space))
            ShootProj();
            hasShoot = true;
        if (Input.GetKey(KeyCode.Space))
            hasShoot = false;

        targetPosition = (Vector2)transform.position + moveDirection * moveSpeed * Time.deltaTime;
        transform.position = targetPosition;
    }

    private void ShootProj(){
        if (Projectile != null && hasShoot)
        {
            Instantiate(Projectile, transform.position, Projectile.transform.rotation);
        }
    }
}
