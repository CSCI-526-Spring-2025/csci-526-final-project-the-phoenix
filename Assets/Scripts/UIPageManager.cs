using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPageManager : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject modal;
    [SerializeField] private GameObject levelSelector;
    [SerializeField] private GameObject[] titleObjects;

    public void Start(){
        GameObject[] titleObjects = GameObject.FindGameObjectsWithTag("Titles");
        Debug.Log("Title Objects: " + titleObjects.Length);
    }

    public void OpenInstructionModal()
    {   
        for (int i = 0; i < titleObjects.Length; i++)
        {
            titleObjects[i].SetActive(false);
        }
        buttons.SetActive(false);
        modal.SetActive(true);
    }

    public void OpenLevelSelector()
    {
        for (int i = 0; i < titleObjects.Length; i++)
        {
            titleObjects[i].SetActive(false);
        }
        buttons.SetActive(false);
        levelSelector.SetActive(true);
    }

    public void CloseInstructionModal()
    {
        for (int i = 0; i < titleObjects.Length; i++)
        {
            titleObjects[i].SetActive(true);
        }
        modal.SetActive(false);
        buttons.SetActive(true);
    }

    public void CloseLevelSelector()
    {
        for (int i = 0; i < titleObjects.Length; i++)
        {
            titleObjects[i].SetActive(true);
        }
        levelSelector.SetActive(false);
        buttons.SetActive(true);
    }

    public void startGame()
    {
        SceneManager.LoadScene("New Tutorial");
    }
}