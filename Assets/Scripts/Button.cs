using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // Reference to the Gate game object
    public GameObject gate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the button is pressed
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Clone"))
        {
            // Open the gate
            gate.SetActive(false);
            Debug.Log("Gate opened");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Clone"))
        {
            // Close the gate
            gate.SetActive(true);
            Debug.Log("Gate closed");
        }
    }
}
