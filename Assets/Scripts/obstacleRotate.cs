using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.forward;  // z-axis rotation
    public float rotationSpeed = 30f;               // degrees per second
    public bool isInverted = false;                 // if true, rotate opposite

    void Update()
    {
        float finalSpeed = isInverted ? -rotationSpeed : rotationSpeed;
        transform.Rotate(rotationAxis * finalSpeed * Time.deltaTime);
    }
}