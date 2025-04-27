using UnityEngine;

public class BobbingObstacle : MonoBehaviour
{
    public float bobbingAmount = 0.8f;     
    public float bobbingSpeed = 2.5f;      
    public bool invertMovement = false;    

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = invertMovement ? Mathf.PI : 0f; 
        float newY = startPosition.y + Mathf.Sin(Time.time * bobbingSpeed + offset) * bobbingAmount;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
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
