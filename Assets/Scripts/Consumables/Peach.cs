using UnityEngine;
using System.Collections;

public class Peach : MonoBehaviour
{
    private GameObject[] platforms;
    private Collider2D peachCollider;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        //Get platforms
        StartCoroutine(GetPlatforms());

        peachCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start hidden and inactive
        SetActiveState(false);
    }

    private IEnumerator MovePeachRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(7f);
            PlacePeachOnRandomPlatform();
        }
    }

       private IEnumerator GetPlatforms()
    {
        yield return new WaitForSeconds(5f);

        // Find all platforms with the "Platform" tag, this is where we will spawn the peach
        platforms = GameObject.FindGameObjectsWithTag("Platform");

        if (platforms.Length == 0)
        {
            Debug.LogError("No platforms found with the 'Platform' tag.");
            yield return null;
        }

        // Start moving the peach every 10 seconds to a random platform
        StartCoroutine(MovePeachRoutine());

    }

    private void PlacePeachOnRandomPlatform()
    {
        if (platforms.Length == 0) return;

        // Choose a random platform
        GameObject randomPlatform = platforms[Random.Range(0, platforms.Length)];

        // Place the peach on the platform and parent it
        transform.position = randomPlatform.transform.position + Vector3.up * 0.25f; 
        transform.SetParent(randomPlatform.transform);

        // Make it visible and active
        SetActiveState(true);
    }

    private void SetActiveState(bool state)
    {
        spriteRenderer.enabled = state;
        peachCollider.enabled = state;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Peach collected!"); 
            //Add score
            GameManager.Instance.AddScore(1000);

            StopAllCoroutines();

            //Sound here
            AudioManager.Instance.PlaySFX("Peach");

            Destroy(gameObject);

            
        }
    }
}
