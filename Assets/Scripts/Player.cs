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
    public GameObject arrow3;

    void Start()
    {
        gameObject.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        isGravityInverted = 1;
        winText.SetActive(false);
        dieText.SetActive(false);
        LevelManager.Instance.TrackPlayerStart(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        // Horizontal movement using A and D keys
        float moveDirection = 0;
        if (Input.GetKey(KeyCode.A) && !Physics2D.OverlapBox(leftWallCheck.position, wallCheckSize, 0f, groundLayer)) moveDirection = -1;
        if (Input.GetKey(KeyCode.D) && !Physics2D.OverlapBox(rightWallCheck.position, wallCheckSize, 0f, groundLayer)) moveDirection = 1;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

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
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Finish"))
        {   
            ShipDeparture ship = collider.GetComponent<ShipDeparture>();
            Debug.Log(ship);
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
            cloneScript.resetPosition();
            cloneScript.resetGravity();
            if (arrow1 != null)
            {
                arrow1.SetActive(false);
                arrow2.SetActive(true);
                arrow3.SetActive(true);
            }
        }

        if (collider.gameObject.CompareTag("GravityPortal"))
        {
            canToggleGravity = true;

        }

        if (collider.gameObject.CompareTag("Shock") || collider.gameObject.CompareTag("Laser"))
        {
            LevelManager.Instance.TrackPlayerDeath(SceneManager.GetActiveScene().name, transform.position, "player", collider.gameObject.tag);
            StartCoroutine(RestartAfterDelay());
            dieText.SetActive(true);
        }

        IEnumerator RestartAfterDelay()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
            RestartGame();
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
}
