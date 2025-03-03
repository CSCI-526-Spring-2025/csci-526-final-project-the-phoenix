using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager

public class Button : MonoBehaviour
{
    // Reference to the Gate game object
    public Transform gate;
    private float openHeight = 3.0f;
    private float speed = 2.0f;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpening = false;
    private bool openUpward = true; // Default to opening upwards

    void Start()
    {
        initialPosition = gate.position;

        // Hardcoded scene-based opening direction
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level1")
        {
            openUpward = false; // Open downward in Level1
        }

        // Determine target position based on openUpward
        if (openUpward)
        {
            targetPosition = initialPosition + new Vector3(0, openHeight, 0); // Move upwards
        }
        else
        {
            targetPosition = initialPosition - new Vector3(0, openHeight, 0); // Move downwards
        }
    }

    void Update()
    {
        if (isOpening)
        {
            // Move towards target position (up or down)
            gate.position = Vector3.MoveTowards(gate.position, targetPosition, speed * Time.deltaTime);
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
            isOpening = true;
            Debug.Log("Gate opened");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Clone"))
        {
            isOpening = false;
            Debug.Log("Gate closed");
        }
    }
}
