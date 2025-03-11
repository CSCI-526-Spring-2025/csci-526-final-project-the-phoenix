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
    private bool canJump;
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
        canJump = true;
    }

    void Update()
    {
        // Horizontal movement using left and right arrow keys
        float moveDirection = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) moveDirection = -1;
        if (Input.GetKey(KeyCode.RightArrow)) moveDirection = 1;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // Jump only if it's allowed in the scene
        if (canJump && Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            // rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (canJump && Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)
        {
            // rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
            rb.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
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
        if (collider.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Player Won!");
            LevelManager.Instance.TrackLevelCompletion(SceneManager.GetActiveScene().name, Time.timeSinceLevelLoad);
            LoadNextLevel();
        }

        if (collider.gameObject.CompareTag("PlayerPlatform"))
        {
            Debug.Log("Cloning player");
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

        if (collider.gameObject.CompareTag("Shock"))
        {
            Debug.Log("PLAYER DIED!");
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

    public void disableJump()
    {
        canJump = false;
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
            Debug.Log("Game Completed!");
            winText.SetActive(true);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
