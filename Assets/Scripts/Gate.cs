using UnityEngine;

public class Gate : MonoBehaviour
{
    public Transform gate;  
    public float openHeight = 3f; 
    public float speed = 2f; 

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpening = false;

    void Start()
    {
        initialPosition = gate.position; 
        targetPosition = initialPosition + new Vector3(0, openHeight, 0); 
    }

    void Update()
    {
        if (isOpening)
        {
            gate.position = Vector3.MoveTowards(gate.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            gate.position = Vector3.MoveTowards(gate.position, initialPosition, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Button"))
        {
            isOpening = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Button"))
        {
            isOpening = false;
        }
    }
}