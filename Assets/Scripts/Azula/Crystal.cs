using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private Sprite brokenSprite;
    private bool isBroken;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem ps;
    
    //[SerializeField] private Azula azula; this is our azula script in the future that we will use to inflcit damage upon her
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();
    }

   private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Fire Ability") && !isBroken){
            //Set the sprites to broken sprite
            spriteRenderer.sprite = brokenSprite;
            ps.Stop(); //Stops the particle system
            //Hurt azula
            //azula.TakeDamage();
            //Set is broken to true so we cant retrigger this
            isBroken = true;
        }
    }
}