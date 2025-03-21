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

        // Reset fireballs if they exceed 30 units from the boss
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
        // Add lightning effect or damage logic here
    }

    // Animation Event - Fire Barrage Event (Called 5 times)
    public void OnFireBarrageEvent()
    {
        if (!isAlive || isHurt) return;

        int baseFireCount = 1; // Always 1 fireball at the player
        int additionalFireCount = baseFireCount + (4 - lives) * 2; // Two extra fireballs per lost life
        float spreadDistance = 2f; // Distance between fireballs

        // Spawn the base fireball at the player
        SpawnFireballAtTarget(momo.transform.position);

        // Spawn additional fireballs to the sides based on lives lost
        for (int i = 1; i <= additionalFireCount / 2; i++)
        {
            Vector3 leftOffset = momo.transform.position + Vector3.left * (spreadDistance * i);
            Vector3 rightOffset = momo.transform.position + Vector3.right * (spreadDistance * i);

            SpawnFireballAtTarget(leftOffset);
            SpawnFireballAtTarget(rightOffset);
        }
    }

    private void SpawnFireballAtTarget(Vector3 targetPosition)
    {
        GameObject fireball = GetFireballFromPool();
        if (fireball == null) return;

        fireball.SetActive(true);
        fireball.transform.position = transform.position;

        AzulaFireBall fireballScript = fireball.GetComponent<AzulaFireBall>();
        fireballScript.Initialize(transform, targetPosition, fireballSpeed);
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
            anim.SetBool("IsDead", true);
            StopAllCoroutines();
        }
    }

    private void NotHurt()
    {
        isHurt = false;
    }
}