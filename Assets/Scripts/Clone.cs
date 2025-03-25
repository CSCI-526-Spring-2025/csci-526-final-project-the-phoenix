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
    private bool canToggleGravity = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGravityInverted = 1;
        isGrounded = true;
        moveSpeed = playerScript.moveSpeed;
        jumpForce = playerScript.jumpForce;

        initialPosition = transform.position;

        gameObject.SetActive(false);
    }

    void Update()
    {
        float moveDirection = 0;
        if (Input.GetKey(KeyCode.A)) moveDirection = -1;
        if (Input.GetKey(KeyCode.D)) moveDirection = 1;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (canToggleGravity && Input.GetKeyDown(KeyCode.Space))
        {
            invertGravity();
            rb.gravityScale *= -1;
            SpaceBarLogger.LogSpacePress("Clone", transform.position);
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
            canToggleGravity = true;
        }

        if (collider.gameObject.CompareTag("Shock") || collider.gameObject.CompareTag("Laser"))
        {
            LevelManager.Instance.TrackPlayerDeath(SceneManager.GetActiveScene().name, transform.position, "clone");
            // Destroy clone
            gameObject.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("GravityPortal"))
        {
            canToggleGravity = false;
        }
    }

    public void resetPosition()
    {
        transform.position = initialPosition;
    }

    public void invertGravity()
    {
 
        isGravityInverted *= -1;
    }

    public void resetGravity()
    {
        isGravityInverted = 1;
        rb.gravityScale = 1;
    }
}
