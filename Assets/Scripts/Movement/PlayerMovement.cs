using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    public int playerMoney;
    public Vector2 movementInput;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    // Section About Hiding
    private bool canHide = false;
    private bool hiding = false;





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

        //If I want the Player to Stop Moving When in the Shadow
        //if (!hiding)
        //{
        //    rb.linearVelocity = new Vector2(movementInput, rb.linearVelocity.y);
        //}
        //else
        //{
        //    rb.linearVelocity = Vector2.zero;
        //}
    }

    private void Update()
    {
        if (canHide == true && Input.GetKey("Shift"))
        {
            Debug.Log("WE HIDE!");
            Physics2D.IgnoreLayerCollision(8, 9, true);
            spriteRenderer.sortingOrder = 0;
            hiding = true;
        }
        else
        {
            Physics2D.IgnoreLayerCollision(8, 9, false);
            spriteRenderer.sortingOrder = 2;
            hiding = false;
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Equals("HideElement"))
        {
            canHide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("HideElement"))
        {
            canHide = false;
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    //Interact
    public bool playerInteractShift;//Is my key being pressed?
    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("WE PRESS Shift!");
            playerInteractShift = true;
        }
        else if (context.canceled)
        {
            playerInteractShift = false;
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
