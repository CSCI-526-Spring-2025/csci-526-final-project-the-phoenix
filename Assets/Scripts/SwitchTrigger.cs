using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    [SerializeField] private SquareRotator targetSquare;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            targetSquare?.Rotate90Clockwise();
        }
    }
}
