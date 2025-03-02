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
    public Vector2 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player is touching the ground
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}
