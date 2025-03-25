using UnityEngine;

public class SquareRotator : MonoBehaviour
{
    private float currentAngle = 0f;

    public void Rotate90Clockwise()
    {
        currentAngle += 90f;
        currentAngle %= 360f;
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
