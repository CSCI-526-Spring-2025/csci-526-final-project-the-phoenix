using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // Reference to the Gate game object
    public Transform gate;
    private float openHeight = 3.0f;
    private float speed = 2.0f;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpening = false;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = gate.position; 
        targetPosition = initialPosition + new Vector3(0, openHeight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening)
        {
            // Move UP
            gate.position = Vector3.MoveTowards(gate.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            // Move BACK DOWN when button is released
            gate.position = Vector3.MoveTowards(gate.position, initialPosition, speed * Time.deltaTime);
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the button is pressed
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Clone"))
        {
            // Open the gate
            isOpening = true;
            Debug.Log("Gate opened");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Clone"))
        {
            // Close the gate
            isOpening = false;
            Debug.Log("Gate closed");
        }
    }
}
