using UnityEngine;
using UnityEngine.InputSystem;

public class Momo : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1f; // Set movement step size
    [SerializeField] private float moveTime = 0.2f;  // Time to complete movement
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isMoving = false; // Prevent multiple inputs before finishing move
    private Animator animator; //Our animator used for sprite animations
    private SpriteRenderer spriteRenderer; //Using this to flip the sprite when moving to the left as I didnt make a move left animation
    private float lastInputX;  //These keep track of where momo should be facing
    private float lastInputY;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isMoving) return; // Prevent movement spam

        if (moveDirection != Vector2.zero) //When user moves
        {
            Vector2 targetPosition = (Vector2)transform.position + moveDirection * moveDistance;
            StartCoroutine(MoveToPosition(targetPosition));//This allows us to move smoothly instead of just teleporting, looks nicer
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!context.performed || isMoving) return; // Ignore input while moving

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
    }

    private System.Collections.IEnumerator MoveToPosition(Vector2 target)
    {
        isMoving = true;
        Vector2 startPosition = rb.position;
        float elapsedTime = 0f;

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
}