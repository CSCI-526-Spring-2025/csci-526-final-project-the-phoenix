using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 7f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float randomAngle = Random.Range(-15f, 15f);
        Vector2 baseDirection = new Vector2(1, 1).normalized;
        Vector2 initialDirection = Quaternion.Euler(0, 0, randomAngle) * baseDirection;
        rb.velocity = initialDirection * speed;
    }

    void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * speed;
    }
}