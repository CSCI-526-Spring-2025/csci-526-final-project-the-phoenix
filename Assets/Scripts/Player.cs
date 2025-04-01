using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int isGravityInverted;
    private bool canToggleGravity = false;

    [SerializeField] private Clone cloneScript;
    public GameObject winText;
    public GameObject arrow1;
    public GameObject arrow2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGravityInverted = 1;
        isGrounded = true;
        winText.SetActive(false);
    }

    void Update()
    {
        // Horizontal movement using A and D keys
        float moveDirection = 0;
        if (Input.GetKey(KeyCode.A)) moveDirection = -1;
        if (Input.GetKey(KeyCode.D)) moveDirection = 1;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // Jump only if it's allowed in the scene
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            // rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
            rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (canToggleGravity && Input.GetKeyDown(KeyCode.Space))
        {
            invertGravity();
            rb.gravityScale *= -1;
            SpaceBarLogger.LogSpacePress("Player", transform.position);

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Shock") || collision.gameObject.CompareTag("Laser"))
        {
            LevelManager.Instance.TrackPlayerDeath(SceneManager.GetActiveScene().name, transform.position, "player");
            RestartGame();
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
        if (collider.gameObject.CompareTag("Finish"))
        {
            LevelManager.Instance.TrackLevelCompletion(SceneManager.GetActiveScene().name, Time.timeSinceLevelLoad);
            LevelManager.Instance.TrackGravityCount();
            LoadNextLevel();

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
            }
        }

        if (collider.gameObject.CompareTag("GravityPortal"))
        {
            canToggleGravity = true;

        }

        if (collider.gameObject.CompareTag("Shock") || collider.gameObject.CompareTag("Laser"))
        {
            LevelManager.Instance.TrackPlayerDeath(SceneManager.GetActiveScene().name, transform.position, "player");
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
