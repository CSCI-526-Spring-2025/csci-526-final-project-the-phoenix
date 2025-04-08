using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPageManager : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject modal;
    [SerializeField] private GameObject levelSelector;

    public void OpenInstructionModal()
    {
        buttons.SetActive(false);
        modal.SetActive(true);
    }

    public void OpenLevelSelector()
    {
        buttons.SetActive(false);
        levelSelector.SetActive(true);
    }

    public void CloseInstructionModal()
    {
        modal.SetActive(false);
        buttons.SetActive(true);
    }

    public void CloseLevelSelector()
    {
        levelSelector.SetActive(false);
        buttons.SetActive(true);
    }

    public void startGame()
    {
        SceneManager.LoadScene("New Tutorial");
    }
}