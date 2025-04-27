using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLivesController : MonoBehaviour
{
    [Header("Settings")]
    public int startingLives = 3;   // How many lives at the start
    private int currentLives;

    [Header("References")]
    public GameObject[] hearts;     // Drag your hearts here manually (in order)

    [Header("Game Over UI")]
    public GameObject dieText;      // Drag "You Died" UI here if you have

    void Start()
    {
        currentLives = startingLives;

        // Disable all hearts first
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);
        }

        // Enable only starting lives
        for (int i = 0; i < startingLives && i < hearts.Length; i++)
        {
            hearts[i].SetActive(true);
        }

        if (dieText != null)
            dieText.SetActive(false);
    }

    public void LoseLife()
    {
        if (currentLives <= 0)
            return;

        currentLives--;

        // Disable corresponding heart
        if (currentLives >= 0 && currentLives < hearts.Length)
        {
            hearts[currentLives].SetActive(false);
        }

        if (currentLives == 0)
        {
            if (dieText != null)
                dieText.SetActive(true);

            StartCoroutine(RestartAfterDelay());
        }

        IEnumerator RestartAfterDelay()
        {
            Debug.Log("DEAD");
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
            RestartGame();
        }
    }


    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
   
}
