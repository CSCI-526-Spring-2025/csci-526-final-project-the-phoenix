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
    
    [SerializeField] private Clone cloneScript;
    public GameObject winText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGravityInverted = 1;
        isGrounded = true;
        winText.SetActive(false);

        string sceneName = SceneManager.GetActiveScene().name;
        canJump = sceneName != "Level1"; 
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
            Debug.Log("Jumping");
            rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
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
            winText.SetActive(true);
            Debug.Log("You win!");
            Invoke("LoadNextScene", 1f);
        }

        if (collider.gameObject.CompareTag("PlayerPlatform"))
        {
            Debug.Log("Cloning player");
            cloneScript.gameObject.SetActive(true);
            cloneScript.resetPosition();
        }
    }

    public void invertGravity()
    {
        isGravityInverted *= -1;
    }
    private void LoadNextScene()
    {
        SceneManager.LoadScene("Level1");
    }
}
