using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Momo : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1f; // Set movement step size
    [SerializeField] private float moveTime = 0.2f;  // Time to complete movement
    [SerializeField] private GameObject deathSprite;
    [SerializeField] private Vector3 respawnPoint; // Momo's current reset position
    [SerializeField] private GameObject homeSpritePrefab; // sprite we want to place on the home once we reach it
    [SerializeField] private Tilemap homeTilemap; // the tile map for the homes
    [SerializeField] private bool isGodMode;
    private Rigidbody2D rb;
    private float constMoveTime;
    private Vector2 moveDirection;
    private bool isMoving = false; // Prevent multiple inputs before finishing move
    private Animator animator; //Our animator used for sprite animations
    private SpriteRenderer spriteRenderer; //Using this to flip the sprite when moving to the left as I didnt make a move left animation
    private float lastInputX;  //These keep track of where momo should be facing
    private float lastInputY;
    private bool isDead;
    private Abilities abilities;
    private int numberAtHome = 0; // keep track of how many checkpoints have been reached
    public TimerBarUI timerBarUI;
    private Collider2D myCollider2D;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        abilities = GetComponent<Abilities>();
        myCollider2D = GetComponent<Collider2D>(); 
        constMoveTime = moveTime;
    }

    private void Update()
    {
        if (isMoving) return; // Prevent movement spam

        if (moveDirection != Vector2.zero) //When user moves
        {
            //If momo is using the air ability he goes twice as fast
            if(abilities.GetIsFlying()){
                moveTime = constMoveTime / 2f; //double momo's speed
            }else{
                moveTime = constMoveTime; //reset his speed
            }
            Vector2 targetPosition = (Vector2)transform.position + moveDirection * moveDistance;

            Collider2D platform = Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
            Collider2D cantGoHere = Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, LayerMask.GetMask("CantGoHere"));
            Collider2D projectile = Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, LayerMask.GetMask("Projectile"));
            Collider2D abyss = Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, LayerMask.GetMask("Abyss"));
            Collider2D home = Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, LayerMask.GetMask("Home")); //These are where momo is trying to get to in order to progress
            Collider2D ground = Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, LayerMask.GetMask("Ground")); //non moving ground that momo is safe on
            // If momo lands on a platfrom we attach him to it
            if (platform != null) {
                transform.SetParent(platform.transform);
            } else {
                transform.SetParent(null);
            }
            //some logic here for when momo sucessfully gets to a home
            if (home != null && projectile == null) {
                ReachHome(targetPosition);
                return;
            } 
            // Momo dies when he lands in the abyss (loses a life and gets reset). 
            if (abyss != null && platform == null && ground == null && home == null && !isGodMode)
            {
                //Call our death function. Currently everything just does 1 damage for now
                Death(targetPosition, 1);
            }else{
                if(cantGoHere == null){ //if we are not jumping out of frame
                StopAllCoroutines();
                StartCoroutine(MoveToPosition(targetPosition)); // This allows us to move smoothly instead of just teleporting, looks nicer
                }else{
                    animator.SetBool("isJumping", false); //Exit jumping animation
                }  
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!context.performed || isMoving || isDead){
            return; // Ignore input while moving or dead
        } 

        animator.SetBool("isJumping", true); //Enter jumping animation

        Vector2 input = context.ReadValue<Vector2>(); //Get move direction

        // Prevent diagonal movement: Prioritize horizontal or vertical
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            moveDirection = new Vector2(Mathf.Sign(input.x), 0);
        else
            moveDirection = new Vector2(0, Mathf.Sign(input.y));

        //Set our animator variables
        lastInputX = moveDirection.x;
        lastInputY = moveDirection.y;

         // Flip the sprite if moving left
        if (moveDirection.x < 0)
            spriteRenderer.flipX = true; // Flip the sprite to face left
        else if (moveDirection.x > 0)
            spriteRenderer.flipX = false; // Reset the sprite to face right

        animator.SetFloat("InputX", moveDirection.x);
        animator.SetFloat("InputY", moveDirection.y);
        animator.SetFloat("LastInputX", lastInputX);
        animator.SetFloat("LastInputY", lastInputY);
        AudioManager.Instance.PlaySFX("Move");
    }

    private System.Collections.IEnumerator MoveToPosition(Vector2 target)
    {
        isMoving = true;
        Vector2 startPosition = rb.position;
        float elapsedTime = 0f;

        if (target.y > transform.position.y)
            GameManager.Instance.AddScore(10);

        while (elapsedTime < moveTime)
        {
            rb.linearVelocity = (target - startPosition) / moveTime; // Smooth transition
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.position = target; // Snap to final position
        rb.linearVelocity = Vector2.zero; // Stop movement
        isMoving = false; //allows us to move again
        moveDirection = Vector2.zero; // Reset input

        //We snap Y to the nearest whole number to prevent some stupid bugs. When parenting momo to the platforms this will become important
        Vector3 pos = transform.position;
        pos.y = Mathf.Round(pos.y);
        transform.position = pos;

        animator.SetBool("isJumping", false); //stops the jumping animation and returns to idle
    }

    public void Death(Vector2 target, int damage){
        AudioManager.Instance.PlaySFX("Death");
        //God Mode for testing purposes
        if(isGodMode){
            return;
        }
        StopAllCoroutines();    
        moveDirection = Vector2.zero;  //reset movement
        isMoving = true; // prevent movement bug
        isDead = true; //momo is currently dead
        animator.SetBool("isJumping", false); //exit jump animation
        transform.SetParent(null);
        //Show hurt sprite at the destination tile
        if(deathSprite!=null){
            GameObject deathFeedback = Instantiate(deathSprite, target, Quaternion.identity);
            //We will have it last for a couple seconds and then destroy it. Maybe later we will have it fade out
            Destroy(deathFeedback, 2f);
        }else{
            Debug.Log("No death sprite attached to momo script");
        }

        //Reset Momo's abilities here in the future
        abilities.StopAbility();
        //Disable control of this script so momo cant move during death
        enabled = false;

        //Disable momo so he cannot do anything while dead

        gameObject.SetActive(false);

        //Call our death function in the game manager once its set up
        GameManager.Instance.HasDied(damage);

    }

    //Called when momo reaches a home
    private void ReachHome(Vector2 target){
        AudioManager.Instance.PlaySFX("Checkpoint");
        StopAllCoroutines();    
        moveDirection = Vector2.zero;  //reset movement
        isMoving = true; // prevent movement bug
        animator.SetBool("isJumping", false); //exit jump animation
        transform.SetParent(null);
        GameManager.Instance.MadeItHome();

        //set Home Sprite to the location

         // find the tile center using Tilemap
        if (homeTilemap != null && homeSpritePrefab != null)
        {
            Vector3Int cellPosition = homeTilemap.WorldToCell(target); // convert world position to tilemap cell
            Vector3 tileCenter = homeTilemap.GetCellCenterWorld(cellPosition); // get the center of the tile
            tileCenter.y += 0.25f; // Raise it by 0.25 units to center the momo sprite in the home tile
            Instantiate(homeSpritePrefab, tileCenter, Quaternion.identity);
        }
        //Increment home score here in the future once game manager is set up (once home score gets to 5 we move to next)
        numberAtHome++;
        if (numberAtHome == 5) 
        {
            gameObject.SetActive(false);
            GameManager.Instance.LevelUp(); // LevelUp could also implement UI elements/animation showcasing going to next level
            numberAtHome = 0;
        } else
        {

            //Reset Momo's abilities here in the future
            abilities.StopAbility();
            //Disable control of this script so momo cant move 
            enabled = false;

            //Disable momo so he cannot do anything while dead

            gameObject.SetActive(false);

            //Call our death function in the game manager once its set up
            Invoke(nameof(Respawn), 1f);
        }
    }

    public void Respawn(){
        //This is called by our game manager after death, if we have more lives left. Right now we just call it in this file
        StopAllCoroutines(); 
        isMoving = false; //reset movement
        //Re enable momo
        gameObject.SetActive(true);

        //Re enable control
        Invoke(nameof(reEnableControl), 0.25f);

        //spawn momo at respawn point
        transform.position = respawnPoint;
    }

    private void reEnableControl(){
        //re-enable control
        enabled = true;

        //Set momo to alive
        isDead = false;
    }

    public bool getIsDead(){
        return isDead;
    }

     private void OnTriggerEnter2D(Collider2D collision){
        //This is for when momo gets hit by a projectile
        bool isProjectile = collision.gameObject.layer == LayerMask.NameToLayer("Projectile");
        if(isProjectile && !abilities.GetIsShielded()) { //IF momo gets hit by a projectile and he is not shielded by the rock ability he dies
            Death(transform.position,1);
            return;
        }

        //This is for when momo tries to escape the game boundaries
        bool isDeathWall = collision.gameObject.layer == LayerMask.NameToLayer("DeathWall");
        if(isDeathWall && !abilities.GetIsFire()){ //this prevents bug where fire collider triggered death
            Death(transform.position,1);
            return;
        }
    }
    


}