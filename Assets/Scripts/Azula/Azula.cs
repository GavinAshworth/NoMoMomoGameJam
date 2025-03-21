using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Azula : MonoBehaviour
{
    private int lives = 4;
    private bool isAlive = true;
    private bool isHurt = false;
    private Animator anim;
    private List<GameObject> fireballPool = new List<GameObject>();

    [SerializeField] GameObject fireballPrefab;
    [SerializeField] GameObject lightningPrefab;
    [SerializeField] int numberOfLightningStrikes = 5; // Number of lightning strikes to spawn
    [SerializeField] float spawnRadius = 5f; // Radius around Momo to spawn lightning strikes
    [SerializeField] GameObject momo;
    [SerializeField] int poolSize = 100;
    [SerializeField] float fireballSpeed = 5f;
    private bool isAttacking;
    private int attackCount = 0;

    // Attack cycle settings
    [SerializeField] private int attacksPerCycle = 5;
    [SerializeField] private float timeBetweenAttacks = 2f;
    [SerializeField] private float timeBetweenCycles = 5f;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Initialize fireball pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            fireball.SetActive(false);
            fireballPool.Add(fireball);
        }

        StartCoroutine(BossBehavior());
    }

    void Update()
    {
        if (!isAlive) return;

        // Reset fireballs if they exceed 30 units from azula
        foreach (GameObject fireball in fireballPool)
        {
            if (fireball.activeInHierarchy && Vector3.Distance(fireball.transform.position, transform.position) > 30f)
            {
                ResetFireball(fireball);
            }
        }
    }

    private IEnumerator BossBehavior()
    {
        while (isAlive)
        {
            // Wait if the boss is hurt
            while (isHurt)
            {
                yield return null;
            }

            // Perform attack cycle
            for (attackCount = 0; attackCount < attacksPerCycle; attackCount++)
            {
                if (!isAlive) yield break;

                // Choose attack
                int attackChoice = Random.Range(0, 2); // 0 = Lightning, 1 = Fire
                isAttacking = true;

                if (attackChoice == 0)
                {
                    anim.SetTrigger("LightningAttack");
                }
                else
                {
                    anim.SetTrigger("FireBarrage");
                }

                // Wait for the attack to finish
                yield return new WaitUntil(() => !isAttacking);

                // Small break between attacks
                yield return new WaitForSeconds(timeBetweenAttacks);
            }

            // Break between attack cycles
            yield return new WaitForSeconds(timeBetweenCycles);
        }
    }

    // Animation Event - Lightning Strike
    public void OnLightningStrike()
{
    if (!isAlive || isHurt) return;
    Debug.Log("Lightning Strike Triggered!");

    for (int i = 0; i < numberOfLightningStrikes; i++)
    {
        // Generate a random position within the spawn radius
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = momo.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        // Spawn our lightning attack at the random position's
        GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);
    }
}


    // Animation Event - Fire Barrage Event (Called 5 times)
    public void OnFireBarrageEvent()
    {
        if (!isAlive || isHurt) return;

        int totalFireballs = 1 + (4 - lives) * 2; // Base fireball + two extra per lost life
        float coneAngle = 30f + (4 - lives) * 15; //cone gets slightly bigger as azula loses lives
        float angleStep = (totalFireballs > 1) ? coneAngle / (totalFireballs - 1) : 0; // Handle division by zero

        // Calculate the direction to the player
        Vector3 directionToMomo = (momo.transform.position - transform.position);
        if (directionToMomo == Vector3.zero)
        {
            directionToMomo = Vector3.right; // Default direction if player is too close
        }
        directionToMomo = directionToMomo.normalized;

        // Handle single fireball case
        if (totalFireballs == 1)
        {
            SpawnFireballInDirection(directionToMomo);
            return;
        }

        // Spawn fireballs in a cone
        for (int i = 0; i < totalFireballs; i++)
        {
            // Calculate the angle for this fireball
            float angle = -coneAngle / 2 + angleStep * i; // Spread from -coneAngle/2 to +coneAngle/2

            // Rotate the direction to the player by the calculated angle
            Vector3 fireballDirection = (Quaternion.Euler(0, 0, angle) * directionToMomo).normalized;

            // Spawn the fireball in the calculated direction
            SpawnFireballInDirection(fireballDirection);
        }
    }

    private void SpawnFireballInDirection(Vector3 direction)
    {
        GameObject fireball = GetFireballFromPool();
        if (fireball == null) return;

        fireball.SetActive(true);
        fireball.transform.position = transform.position;

        AzulaFireBall fireballScript = fireball.GetComponent<AzulaFireBall>();
        fireballScript.Initialize(transform, direction, fireballSpeed);
    }

    // Animation Event - End Attack
    public void OnAttackEnd()
    {
        if (!isAlive) return;
        Debug.Log("Attack Ended");
        isAttacking = false; // Reset the attacking flag
    }


    private GameObject GetFireballFromPool()
    {
        foreach (GameObject fireball in fireballPool)
        {
            if (!fireball.activeInHierarchy) return fireball;
        }
        return null;
    }

    // Reset fireball
    private void ResetFireball(GameObject fireball)
    {
        fireball.SetActive(false);
        fireball.transform.position = transform.position;
    }

    // Handle Boss Taking Damage
    public void TakeDamage()
    {
        lives--;
        anim.SetTrigger("Hurt");
        isHurt = true;

        if (lives <= 0)
        {
            isAlive = false;
            anim.SetTrigger("Death");
            StopAllCoroutines();
        }
    }

    private void NotHurt()
    {
        isHurt = false;
    }
}