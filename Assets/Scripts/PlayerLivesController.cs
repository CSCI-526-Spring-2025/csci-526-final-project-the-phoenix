using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public PlayerLivesController livesController;  // Drag your PlayerLivesController in Inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shocks") || other.CompareTag("Laser") || other.CompareTag("Boulder"))
        {
            livesController.LoseLife();
        }
    }
}
