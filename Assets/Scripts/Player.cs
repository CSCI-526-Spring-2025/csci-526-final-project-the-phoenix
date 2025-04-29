using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
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
    [SerializeField] private Clone cloneScript;
    public GameObject winText;
    public GameObject dieText;
    public GameObject arrow1;
    public GameObject arrow2;
    [SerializeField] private Transform spriteTransform;
    public PlayerLivesController livesController;

    void Start()
    {
        gameObject.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        isGravityInverted = 1;
        winText.SetActive(false);
        // dieText.SetActive(false);
        LevelManager.Instance.TrackPlayerStart(SceneManager.GetActiveScene().name);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        // Horizontal movement using A and D keys
        float moveDirection = 0;
        if (Input.GetKey(KeyCode.A) && !Physics2D.OverlapBox(leftWallCheck.position, wallCheckSize, 0f, groundLayer)) moveDirection = -1;
        if (Input.GetKey(KeyCode.D) && !Physics2D.OverlapBox(rightWallCheck.position, wallCheckSize, 0f, groundLayer)) moveDirection = 1;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (moveDirection != 0)
        {
            Vector3 newScale = spriteTransform.localScale;

            if (isGravityInverted == 1)
            {
                newScale.x = Mathf.Abs(newScale.x) * (moveDirection > 0 ? -1 : 1);
            }
            else
            {
                newScale.x = Mathf.Abs(newScale.x) * (moveDirection > 0 ? 1 : -1);
            }

            spriteTransform.localScale = newScale;
        }

        // Ground check
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer) ||
                     Physics2D.OverlapBox(groundCheckReverse.position, groundCheckSize, 0f, groundLayer);

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (canToggleGravity && Input.GetKeyDown(KeyCode.Space))
        {
            invertGravity();
            rb.gravityScale *= -1;
            SpaceBarLogger.LogSpacePress("Player", transform.position);

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

        // Reset Z-Axis to 0
        Vector3 playerTransform = transform.position;
        transform.position = new Vector3(playerTransform.x, playerTransform.y, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boulder"))
        {
            livesController.LoseLife();
            LevelManager.Instance.TrackPlayerDeath(SceneManager.GetActiveScene().name, transform.position, "player", GetComponent<Collider>().gameObject.tag);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Finish"))
        {   
            ShipDeparture ship = collider.GetComponent<ShipDeparture>();
            if (ship != null)
            {
                ship.StartFlyAndLoad(gameObject);
            }
            LevelManager.Instance.TrackLevelCompletion(SceneManager.GetActiveScene().name, Time.timeSinceLevelLoad);
            LevelManager.Instance.TrackGravityCount();
            LevelManager.Instance.TrackCloneUsageData(SceneManager.GetActiveScene().name);
        }

        if (collider.gameObject.CompareTag("PlayerPlatform"))
        {
            cloneScript.gameObject.SetActive(true);
            LevelManager.Instance.TrackCloneUsage();
            LevelManager.Instance.TrackCloneUsageCount();

            cloneScript.resetPosition();
            cloneScript.resetGravity();
            if (arrow1 != null)
            {
                arrow1.SetActive(false);
                arrow2.SetActive(true);
            }
        }

        if (collider.gameObject.CompareTag("GravityPortal"))
        {
            canToggleGravity = true;

        }

        if (collider.gameObject.CompareTag("Shocks") || collider.gameObject.CompareTag("Laser"))
        {
            livesController.LoseLife();
            LevelManager.Instance.TrackPlayerDeath(SceneManager.GetActiveScene().name, transform.position, "player", collider.gameObject.tag);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("GravityPortal"))
        {
            canToggleGravity = false;
        }
    }

    public void invertGravity()
    {
        isGravityInverted *= -1;

        if (isGravityInverted == -1)
            spriteTransform.rotation = Quaternion.Euler(0f, 0f, 180f);
        else
            spriteTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            winText.SetActive(true);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public Transform getGroundCheck()
    {
        return groundCheck;
    }

    public Vector2 getGroundCheckSize()
    {
        return groundCheckSize;
    }

    public LayerMask getGroundLayer()
    {
        return groundLayer;
    }
}
