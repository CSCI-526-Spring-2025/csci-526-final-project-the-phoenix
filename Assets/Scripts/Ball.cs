using UnityEngine;

public class WallHugBall : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 100f;
    private Rigidbody2D rb;

    // Directional movement around the room
    private Vector2 currentDirection = Vector2.right;

    public string type;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void FixedUpdate()
    {
        rb.velocity = currentDirection * moveSpeed;
        rb.MoveRotation(rb.rotation - rotateSpeed * Time.fixedDeltaTime); // Roll effect
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Change direction when hitting a wall
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Passway"))
        {
            Vector2 normal = collision.contacts[0].normal;

            if (type == "room")
            {
                // Rotate movement direction clockwise
                if (normal == Vector2.down) currentDirection = Vector2.left;
                else if (normal == Vector2.left) currentDirection = Vector2.up;
                else if (normal == Vector2.up) currentDirection = Vector2.right;
                else if (normal == Vector2.right) currentDirection = Vector2.down;
            }

            else if (type == "linear")
            {
                if (currentDirection == Vector2.left) {
                    currentDirection = Vector2.right;
                } else {
                    currentDirection = Vector2.left;
                }
            }

        }
    }
}