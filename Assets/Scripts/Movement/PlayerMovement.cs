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
    public bool playerInteractE;//Is my key being pressed?
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("WE PRESS E!");
            playerInteractE = true;
        }
        else if (context.canceled)
        {
            playerInteractE = false;
        }
    }

    public bool playerInteractTab;//Is my key being pressed?
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.started)
        {       
            Debug.Log("WE PRESS TAB!");
            playerInteractTab = true;
        }
        else if (context.canceled)
        {
            playerInteractTab = false;
        }
    }

    public bool playerInteractF;//Is my key being pressed?
    public void Throw(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("WE PRESS F!");
            playerInteractF = true;
        }
        else if (context.canceled)
        {
            playerInteractF = false;
        }
    }

    public bool playerInteract1;//Is my key being pressed?
    public void Previous(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("WE PRESS 1!");
            playerInteract1 = true;
        }
        else if (context.canceled)
        {
            playerInteract1 = false;
        }
    }

    public bool playerInteract2;//Is my key being pressed?

    public static object Instance { get; internal set; }

    public void Next(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("WE PRESS 2!");
            playerInteract2 = true;
        }
        else if (context.canceled)
        {
            playerInteract2 = false;
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
