using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Azula : MonoBehaviour
{
    private int lives = 4;
    private bool isAlive = true;
    private bool isHurt = false;
    private Animator anim;
    private List<GameObject> fireballPool = new List<GameObject>();

    [SerializeField] GameObject fireballPrefab;
    [SerializeField] GameObject lightningPrefab;
    [SerializeField] Tilemap pathToCrystals; //This is the path that allows momo to get to crystals
    [SerializeField] GameObject momo;
    [SerializeField] int poolSize = 100;
    [SerializeField] float fireballSpeed = 5f;
    private bool isAttacking;
    private int attackCount = 0;

    // Attack cycle settings
    [SerializeField] private int attacksPerCycle = 5;
    [SerializeField] private float timeBetweenAttacks = 2f;
    [SerializeField] private float timeBetweenCycles = 9f;

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

        //Remove path to crystals at the start
        pathToCrystals.gameObject.SetActive(false);
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

            //increase boss difficulty
            timeBetweenAttacks = timeBetweenAttacks - (4 - lives) * 0.5f; //Attacks come faster as azula loses lives
            attacksPerCycle = attacksPerCycle + (4 - lives); //1 more attack per cycle per life lost

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

                // Small break between attacks except for last one
                if (attackCount != attacksPerCycle - 1)
                {
                    yield return new WaitForSeconds(timeBetweenAttacks);
                }
            }

            // Break between attack cycles (this is when the player can get to the crystals and hurt azula)
            pathToCrystals.gameObject.SetActive(true);
            anim.SetBool("IsResting", true);
            yield return new WaitForSeconds(timeBetweenCycles);
            pathToCrystals.gameObject.SetActive(false);
            anim.SetBool("IsResting", false);
        }
    }

    // Animation Event - Lightning Strike
    public void OnLightningStrike()
    {
        if (!isAlive || isHurt) return;
        Debug.Log("Lightning Strike Triggered!");

        // Calculate the number of lightning strikes based on lives lost
        int totalStrikes = 1 + (4 - lives) * 2; // 1 strike at 0 lives lost, 3 at 1 life lost, etc.

        // Start the lightning strike sequence
        StartCoroutine(PerformLightningStrikes(totalStrikes));
    }

    private IEnumerator PerformLightningStrikes(int totalStrikes)
    {
        for (int i = 0; i < totalStrikes; i++)
        {
            // Calculate a random offset: -0.5 (left), 0 (center), or 0.5 (right)
            float randomOffset = Random.Range(-1, 2) * 1f; //(makes it so the strikes are a little random and player has to react)
                                                           // First strike is always on top of player
            if (i == 0)
            {
                randomOffset = 0f;
            }
            // Spawn a lightning strike with the random offset
            Vector3 spawnPosition = momo.transform.position + new Vector3(randomOffset, 0, 0);
            GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);

            // Wait for a short delay before the next strike
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.PlaySFX("AzulaLightning");

        }
    }


    // Animation Event - Fire Barrage Event (Called 5 times)
    public void OnFireBarrageEvent()
    {
        if (!isAlive || isHurt) return;
        fireballSpeed = 7f + (4 - lives) * 2; // Fireballs get faster as Azula loses lives
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
        AudioManager.Instance.PlaySFX("Azula Fireball");

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
        if (lives > 0)
        {
            GameManager.Instance.AddScore(500);
            anim.SetTrigger("Hurt");
            anim.SetBool("IsResting", false);
        }
        isHurt = true;

        if (lives <= 0)
        {
            GameManager.Instance.AddScore(3000);
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