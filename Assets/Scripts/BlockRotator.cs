using UnityEngine;

public class SquareRotator : MonoBehaviour
{
    private float currentAngle = 0f;

    public void Rotate90Clockwise()
    {
        currentAngle += 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        Debug.Log($"{gameObject.name} rotated to {currentAngle}°");
    }
}
