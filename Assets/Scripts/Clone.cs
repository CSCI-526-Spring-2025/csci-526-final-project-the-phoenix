// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Clone : MonoBehaviour
// {
//     // Player script reference
//     [SerializeField] private Player playerScript;

//     private float moveSpeed = 5f;
//     private float jumpForce = 5f;

//     private Rigidbody2D rb;
//     private bool isGrounded;
//     private int isGravityInverted;
//     private Vector2 initialPosition;
//     private bool canJump;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         isGravityInverted = 1;
//         isGrounded = true;
//         moveSpeed = playerScript.moveSpeed;
//         jumpForce = playerScript.jumpForce;
//         canJump = true;

//         // Get the starting position of the clone
//         initialPosition = transform.position;

//         // Disable the clone object temporarily
//         gameObject.SetActive(false);
//     }

//     void Update()
//     {
//         // Horizontal movement using A and D keys
//         float moveDirection = 0;
//         if (Input.GetKey(KeyCode.A)) moveDirection = -1;
//         if (Input.GetKey(KeyCode.D)) moveDirection = 1;
//         rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

//         // Jump only if it's allowed in the scene
//         if (canJump && Input.GetKeyDown(KeyCode.W) && isGrounded)
//         {
//             Debug.Log("Jumping");
//             rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
//         }
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Floor"))
//         {
//             isGrounded = true;
//         }
//     }

//     void OnCollisionExit2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Floor"))
//         {
//             isGrounded = false;
//         }
//     }

//     public void resetPosition()
//     {
//         // Reset the clone's position to the initial position
//         transform.position = initialPosition;
//     }

//     public void invertGravity()
//     {
//         Debug.Log("Gravity is inverted");
//         isGravityInverted *= -1;
//     }

//     public void disableJump()
//     {
//         canJump = false;
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    // Player script reference
    [SerializeField] private Player playerScript;

    private float moveSpeed = 5f;
    private float jumpForce = 5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int isGravityInverted;
    private Vector2 initialPosition;
    private bool canJump;
    private bool canToggleGravity = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGravityInverted = 1;
        isGrounded = true;
        moveSpeed = playerScript.moveSpeed;
        jumpForce = playerScript.jumpForce;
        canJump = true;

        // Get the starting position of the clone
        initialPosition = transform.position;

        // Disable the clone object temporarily
        gameObject.SetActive(false);
    }

    void Update()
    {
        // Horizontal movement using A and D keys
        float moveDirection = 0;
        if (Input.GetKey(KeyCode.A)) moveDirection = -1;
        if (Input.GetKey(KeyCode.D)) moveDirection = 1;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // Jump only if it's allowed in the scene
        if (canJump && Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Debug.Log("Jumping");
            rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (canToggleGravity && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Gravity Shifted");
            invertGravity();
            rb.gravityScale *= -1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("GravityPortal"))
        {
            Debug.Log("Entered Gravity Portal");
            canToggleGravity = true; // Allow gravity toggle while inside portal
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("GravityPortal"))
        {
            Debug.Log("Exited Gravity Portal");
            canToggleGravity = false; 
        }
    }

    public void resetPosition()
    {
        // Reset the clone's position to the initial position
        transform.position = initialPosition;
    }

    public void invertGravity()
    {
        Debug.Log("Gravity is inverted");
        isGravityInverted *= -1;
    }

    public void disableJump()
    {
        canJump = false;
    }
}
