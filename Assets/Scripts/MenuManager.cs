using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void RestartGame()
    {   
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}