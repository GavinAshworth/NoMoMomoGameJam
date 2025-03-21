using UnityEngine;

public class LightningAttack : MonoBehaviour
{
    private Collider2D lightningCollider;
    private Azula azula;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightningCollider = gameObject.GetComponent<Collider2D>();
        lightningCollider.enabled = false;
        azula = GameObject.FindWithTag("Azula").GetComponent<Azula>();
    }

    private void Damage(){
        lightningCollider.enabled = true;
    }
    private void Reset(){
        lightningCollider.enabled = false;
    }
    private void endOfAnimation(){
        Destroy(gameObject);
    }

}
