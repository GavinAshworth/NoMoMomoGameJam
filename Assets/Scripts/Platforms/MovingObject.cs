using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Transform spawnPoint; // Spawn point for the platform
    private Transform endPoint; // End point for the platform
    private float speed;
    private SpriteRenderer spriteRenderer;
    private int level;
    private bool isReverse;
    private bool isProjectile;

    public void Initialize(Transform spawn, Transform end, float moveSpeed, Sprite platformSprite, int platformLevel, bool reverse, bool projectile)
    {
        spawnPoint = spawn;
        endPoint = end;
        transform.position = spawnPoint.position; // Start at the spawn point
        speed = moveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = platformSprite;
        level = platformLevel;
        isReverse = reverse;
        isProjectile = projectile;

        if(isReverse){
            spriteRenderer.flipX = true;
        }
    }

    private void Update()
    {
        //If we are no longer on the level associated with this platform we destroy it, this will be set up once the game manager is set up
        // if(GameManager.Instance.level != level){
        //     Destroy(gameObject);
        // }
        // Move the platform towards the end point
        transform.position = Vector2.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);

        // If the platform reaches the end point, reset it to the spawn point
        if (Vector2.Distance(transform.position, endPoint.position) < 0.1f){
            transform.position = spawnPoint.position;
        }
    }

    //This will be for when the earth ability and projectiles are set up
    // private void OnTriggerEnter2D(Collider2D collision){
    //     if(collision.CompareTag("Earth Ability") && isProjectile){
    //         transform.position = spawnPoint.position;
    //     }
    // }
}