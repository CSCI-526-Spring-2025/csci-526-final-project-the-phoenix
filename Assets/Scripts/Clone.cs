using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clone : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float maxFallSpeed = 20f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform groundCheckReverse;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.95f, 0.03f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall Detection")]
    [SerializeField] private Transform leftWallCheck;
    [SerializeField] private Transform rightWallCheck;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.03f, 0.95f);

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isWallSliding;
    private int isGravityInverted;
    private bool canToggleGravity = false;
    private Vector2 initialPosition;

    [Header("Miscellaneous")]
    [SerializeField] private Player playerScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGravityInverted = 1;

        moveSpeed = playerScript.moveSpeed;
        jumpForce = playerScript.jumpForce;

        initialPosition = transform.position;

        gameObject.SetActive(false);
    }

    void Update()
    {
        // Horizontal movement using arrow keys
        float moveDirection = 0;
        if (Input.GetKey(KeyCode.LeftArrow) && !Physics2D.OverlapBox(leftWallCheck.position, wallCheckSize, 0f, groundLayer)) moveDirection = -1;
        if (Input.GetKey(KeyCode.RightArrow) && !Physics2D.OverlapBox(rightWallCheck.position, wallCheckSize, 0f, groundLayer)) moveDirection = 1;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // Ground check
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer) ||
                     Physics2D.OverlapBox(groundCheckReverse.position, groundCheckSize, 0f, groundLayer);

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
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

    void LateUpdate()
    {
        // Better feeling gravity (fall faster)

        // Normal Gravity
        if (isGravityInverted == 1)
        {
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = fallMultiplier;
                // Limit max fall speed
                if (rb.velocity.y < -maxFallSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
                }
            }
            else
            {
                rb.gravityScale = 1f;
            }
        }

        // Inverted Gravity
        else 
        {
            if (rb.velocity.y > 0)
            {
                rb.gravityScale = -fallMultiplier;
                // Limit max fall speed
                if (rb.velocity.y > maxFallSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
                }
            }
            else
            {
                rb.gravityScale = -1f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boulder"))
        {
            LevelManager.Instance.TrackPlayerDeath(SceneManager.GetActiveScene().name, transform.position, "clone");
            // Destroy clone
            gameObject.SetActive(false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("GravityPortal"))
        {
            canToggleGravity = true;
        }

        if (collider.gameObject.CompareTag("Laser") || collider.gameObject.CompareTag("Shock"))
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

    public void changeInitialPosition(Vector2 newPosition)
    {
        initialPosition = newPosition;
    }

    public void invertGravity()
    {

        isGravityInverted *= -1;
        if (isGravityInverted == -1)
        {
            float currentY = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, currentY - 180f, 180f);
        }
        else
        {   
            float currentY = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, currentY + 180f, 0f);
        }
    }

    public void resetGravity()
    {
        isGravityInverted = 1;
        rb.gravityScale = 1;
    }
}
