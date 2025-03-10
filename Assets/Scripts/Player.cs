// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement; 

// public class Player : MonoBehaviour
// {
//     public float moveSpeed = 5f;
//     public float jumpForce = 5f;

//     private Rigidbody2D rb;
//     private bool isGrounded;
//     private int isGravityInverted;
//     private bool canJump;

//     [SerializeField] private Clone cloneScript;
//     public GameObject winText;
//     public GameObject arrow1;
//     public GameObject arrow2;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         isGravityInverted = 1;
//         isGrounded = true;
//         winText.SetActive(false);
//         canJump = true;
//     }

//     void Update()
//     {
//         // Horizontal movement using left and right arrow keys
//         float moveDirection = 0;
//         if (Input.GetKey(KeyCode.LeftArrow)) moveDirection = -1;
//         if (Input.GetKey(KeyCode.RightArrow)) moveDirection = 1;
//         rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

//         // Jump only if it's allowed in the scene
//         if (canJump && Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
//         {
//             Debug.Log("Jumping");
//             rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
//         }
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Floor"))
//         {
//             isGrounded = true;
//         }
//     }

//     void OnCollisionExit2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Floor"))
//         {
//             isGrounded = false;
//         }
//     }

//     void OnTriggerEnter2D(Collider2D collider)
//     {
//         if (collider.gameObject.CompareTag("Finish"))
//         {
//             LevelManager.Instance.TrackLevelCompletion(SceneManager.GetActiveScene().name, Time.timeSinceLevelLoad);
//             winText.SetActive(true);
//             Debug.Log("You win!");
//             Invoke("LoadNextScene", 1f);
//         }

//         if (collider.gameObject.CompareTag("PlayerPlatform"))
//         {
//             Debug.Log("Cloning player");
//             cloneScript.gameObject.SetActive(true);
//             arrow2.SetActive(true);
//             arrow1.SetActive(false);
//             cloneScript.resetPosition();
//         }
//     }

//     public void invertGravity()
//     {
//         isGravityInverted *= -1;
//     }

//     public void disableJump()
//     {
//         canJump = false;
//     }

//     private void LoadNextScene()
//     {
//         SceneManager.LoadScene("Level1");
//     }
// }


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
            Debug.Log("Jumping");
            rb.AddForce(isGravityInverted * Vector2.up * jumpForce, ForceMode2D.Impulse);
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
            LevelManager.Instance.TrackLevelCompletion(SceneManager.GetActiveScene().name, Time.timeSinceLevelLoad);
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

        if (collider.gameObject.CompareTag("GravityPortal"))
        {
            Debug.Log("Entered Gravity Portal");
            canToggleGravity = true;
            
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

    public void invertGravity()
    {
        isGravityInverted *= -1;
    }

    public void disableJump()
    {
        canJump = false;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("Level1");
    }
}
