using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public int playerMoney;
    public Vector2 movementInput;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    public float moveSpeed = 5;
    public float jumpHeight = 10;

    public bool isGrounded;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movementInput.x * moveSpeed, rb.linearVelocity.y);
        FlipSprite();

        Debug.DrawRay(transform.position, Vector2.down * 1, Color.purple);
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1, groundLayer);
    }
    
    void FlipSprite()
    {
        if(movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    //Interact
    public bool playerInteracting;//Is my key being pressed?
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerInteracting = true;
        }
        else if (context.canceled)
        {
            playerInteracting = false;
        }
    }

    public bool playerInteractingTab;//Is my key being pressed?
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.started)
        {       
            Debug.Log("WE PRESS TAB!");
            playerInteractingTab = true;
        }
        else if (context.canceled)
        {
            playerInteractingTab = false;
        }
    }



    //JUMP
    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded == true)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        }
    }
}
