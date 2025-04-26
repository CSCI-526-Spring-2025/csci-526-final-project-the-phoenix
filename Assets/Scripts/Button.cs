using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager

public class Button : MonoBehaviour
{
    // Reference to the Gate game object
    public Transform gate;
    public string direction;
    private float openHeight = 3.0f;
    private float speed = 2.0f;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpening;

    void Start()
    {
        initialPosition = gate.position;
        isOpening = false;

        gate.position = Vector3.MoveTowards(gate.position, targetPosition, speed * Time.deltaTime);
        // Determine target position based on openUpward
        if (direction == "up")
        {   
            targetPosition = initialPosition + new Vector3(0, openHeight, 0); // Move upwards
        }
        else if (direction == "down")
        {
            targetPosition = initialPosition - new Vector3(0, openHeight, 0); // Move downwards
        }
        else if (direction == "left")
        {
            targetPosition = initialPosition - new Vector3(openHeight, 0, 0); // Move left
        }
        else if (direction == "right")
        {
            targetPosition = initialPosition + new Vector3(openHeight, 0, 0); // Move right
        }
    }

    void Update()
    {
        if (isOpening)
        {
            // Move towards target position (up or down)\
            gate.position = Vector3.MoveTowards(gate.position, initialPosition + new Vector3(0, openHeight, 0), speed * Time.deltaTime);
        }
        else
        {
            // Move back to initial position when button is released
            gate.position = Vector3.MoveTowards(gate.position, initialPosition, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)

    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Clone"))
        {
            Debug.Log("Contact with button");
            isOpening = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Clone"))
        {
            Debug.Log("Exit Contact with button");
            isOpening = false;
        }
    }
}
