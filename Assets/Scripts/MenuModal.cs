using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuModal : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject menuPanel;

    // Start is called before the first frame update
    void Start()
    {
        instructionsPanel.SetActive(false); 
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
    }

    public void OpenModal()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        instructionsPanel.SetActive(true);
    }

    public void CloseModal()
    {
        instructionsPanel.SetActive(false);
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
    }

}
