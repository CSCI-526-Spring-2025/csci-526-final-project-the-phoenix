using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clone : MonoBehaviour
{
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

        initialPosition = transform.position;

        gameObject.SetActive(false);
    }

    void Update()
    {
        float moveDirection = 0;
        if (Input.GetKey(KeyCode.A)) moveDirection = -1;
        if (Input.GetKey(KeyCode.D)) moveDirection = 1;
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

        if (canJump && Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Debug.Log("Jumping");
            // rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (canJump && Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            Debug.Log("Jumping");
            // rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
            rb.AddForce( Vector2.down * jumpForce, ForceMode2D.Impulse);
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
            canToggleGravity = true;
        }

        if (collider.gameObject.CompareTag("Shock"))
        {
            Debug.Log("CLONE DIED!");
            LevelManager.Instance.TrackPlayerDeath(SceneManager.GetActiveScene().name, transform.position, "clone");
            // Destroy clone
            gameObject.SetActive(false);
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
        transform.position = initialPosition;
    }

    public void invertGravity()
    {
        Debug.Log("Gravity is inverted");
        isGravityInverted *= -1;
    }

    public void resetGravity()
    {
        Debug.Log("Gravity reset");
        isGravityInverted = 1;
        rb.gravityScale = 1;
    }

    public void disableJump()
    {
        canJump = false;
    }
}
