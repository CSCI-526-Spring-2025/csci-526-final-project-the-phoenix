using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private bool isGrounded;

    [SerializeField] private Clone cloneScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal movement using left and right arrow keys
        float moveDirection = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) moveDirection = -1;
        if (Input.GetKey(KeyCode.RightArrow)) moveDirection = 1;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // Jump with up arrow key
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if player has reached the goal
        if (collider.gameObject.CompareTag("Finish"))
        {
            Debug.Log("You win!");
        }

        // Check if player is on cloning platform
        if (collider.gameObject.CompareTag("PlayerPlatform"))
        {
            Debug.Log("Cloning player");
            cloneScript.gameObject.SetActive(true);
            cloneScript.resetPosition();
        }
    }
}
