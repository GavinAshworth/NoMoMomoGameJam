using UnityEngine;

public class AzulaFireBall : MonoBehaviour
{
    private Transform spawnPoint; // Spawn point for the fireball which is azula
    private Vector3 direction; // Direction the fireball is moving
    private float speed;

    public void Initialize(Transform spawn, Vector3 moveDirection, float moveSpeed)
    {
        spawnPoint = spawn;
        direction = moveDirection.normalized; // Ensure the direction is normalized
        speed = moveSpeed;

        // Rotate the fireball to face the movement direction
        RotateTowardsDirection();
    }

    private void Update()
    {
        // Move the fireball in the specified direction
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Earth Ability"))
        {
            ResetFireball(); //this is if the fireball hits momos earth ability (shield)
        }
    }

    private void RotateTowardsDirection() // Makes sure the fireball is pointed towards where its going
    {
        // Calculate the angle in degrees based on the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the fireball
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void ResetFireball()
    {
        gameObject.SetActive(false);
        transform.position = spawnPoint.position;
    }
}