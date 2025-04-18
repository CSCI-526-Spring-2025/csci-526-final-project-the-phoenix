using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void RestartGame()
    {   
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}