using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.forward;  // z-axis rotation
    public float rotationSpeed = 30f;               // degrees per second

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
