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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGravityInverted = 1;
        isGrounded = true;
        moveSpeed = playerScript.moveSpeed;
        jumpForce = playerScript.jumpForce;

        // Get the starting position of the clone
        initialPosition = transform.position;

        // Disable the clone object temporarily
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement using A and D keys
        float moveDirection = 0;
        if (Input.GetKey(KeyCode.A)) moveDirection = -1;
        if (Input.GetKey(KeyCode.D)) moveDirection = 1;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // Jump with W key
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Debug.Log("Jumping");
            rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player is touching the ground
        if (collision.gameObject.CompareTag("Floor"))
        {

            Debug.Log("heresss");
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if player is no longer touching the ground
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("here");
            isGrounded = false;
        }
    }

    public void resetPosition()
    {
        // Reset the clone's position to the initial position
        transform.position = initialPosition;
    }

    public void invertGravity()
    {
        // Flag as invert gravity when called
        Debug.Log("gravity is inverted");
        Debug.Log(isGrounded);
        isGravityInverted *= -1;
    }
}