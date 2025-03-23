using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    [SerializeField] private SquareRotator targetSquare;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Something entered the trigger: {other.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log($"✅ Player touched {gameObject.name}");
            targetSquare?.Rotate90Clockwise();
        }
    }
}
