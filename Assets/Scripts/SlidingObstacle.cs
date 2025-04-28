using UnityEngine;

public class SlidingObstacle : MonoBehaviour
{
    public float slideAmount = 0.8f;     
    public float slideSpeed = 2.8f;        
    public bool invertMovement = false;    // Invert movement

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = invertMovement ? Mathf.PI : 0f;  // 180 degrees phase shift
        float newX = startPosition.x + Mathf.Sin(Time.time * slideSpeed + offset) * slideAmount;
        transform.position = new Vector3(newX, startPosition.y, startPosition.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}


