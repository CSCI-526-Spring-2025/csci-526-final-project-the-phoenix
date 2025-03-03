// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Player : MonoBehaviour
// {
//     public float moveSpeed = 5f;
//     public float jumpForce = 5f;

//     private Rigidbody2D rb;
//     private bool isGrounded;
//     private int isGravityInverted;

//     [SerializeField] private Clone cloneScript;
//     public GameObject winText;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         isGravityInverted = 1;
//         isGrounded = true;

//         // Disable the win text object
//         winText.SetActive(false);
//     }

//     void Update()
//     {
//         // Horizontal movement using left and right arrow keys
//         float moveDirection = 0;
//         if (Input.GetKey(KeyCode.LeftArrow)) moveDirection = -1;
//         if (Input.GetKey(KeyCode.RightArrow)) moveDirection = 1;
//         rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

//         // Jump with up arrow key
//         if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
//         {
//             Debug.Log("Jumping");
//             rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
//         }
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         // Check if player is touching the ground
//         if (collision.gameObject.CompareTag("Floor"))
//         {
//             isGrounded = true;
//         }
//     }

//     void OnCollisionExit2D(Collision2D collision)
//     {
//         // Check if player is no longer touching the ground
//         if (collision.gameObject.CompareTag("Floor"))
//         {
//             isGrounded = false;
//         }
//     }

//     void OnTriggerEnter2D(Collider2D collider)
//     {
//         // Check if player has reached the goal
//         if (collider.gameObject.CompareTag("Finish"))
//         {
//             winText.SetActive(true);
//             Debug.Log("You win!");
//         }

//         // Check if player is on cloning platform
//         if (collider.gameObject.CompareTag("PlayerPlatform"))
//         {
//             Debug.Log("Cloning player");
//             cloneScript.gameObject.SetActive(true);
//             cloneScript.resetPosition();
//         }
//     }

//     public void invertGravity()
//     {
//         // Flag as invert gravity when called
//         isGravityInverted *= -1;
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int isGravityInverted;
    private bool canJump; // New flag to control jump availability

    [SerializeField] private Clone cloneScript;
    public GameObject winText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGravityInverted = 1;
        isGrounded = true;
        winText.SetActive(false);

        // Automatically disable jump if the scene is Level1
        string sceneName = SceneManager.GetActiveScene().name;
        canJump = sceneName != "Level1"; // Jump is disabled in Level1, enabled in all other scenes
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
}
