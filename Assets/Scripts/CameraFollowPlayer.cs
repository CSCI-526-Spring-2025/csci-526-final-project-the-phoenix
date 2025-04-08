using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player; // Reference to the player object
    private Vector3 offset; // Offset between the camera and the player
    private float fixedZ; // To store and maintain the camera's Z position
    
    [Tooltip("How quickly the camera smooths towards the target position. Lower values are faster.")]
    [Range(0.01f, 1.0f)]
    public float smoothTime = 0.33f;
    // Internal variable for SmoothDamp function
    private Vector3 velocity = Vector3.zero;


    void Start()
    {
        float yPos = transform.position.y - player.transform.position.y;
        offset = new Vector3(0, yPos, 0);

        fixedZ = transform.position.z; // Store the initial Z position of the camera
    }

    void LateUpdate()
    {
        Vector3 newPosition = player.transform.position + offset;
        newPosition = new Vector3(newPosition.x, newPosition.y, fixedZ); // Maintain the fixed Z position
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }
}
