using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipDeparture : MonoBehaviour
{
    public float flySpeed = 7f;
    private bool shouldFly = false;

    public GameObject launchEffectPrefab;       // Drag Explosion prefab here
    public Transform[] effectSpawnPoints;       // Drag all 3 EffectSpawnPoints here

    public GameObject winText;

    public void StartFlyAndLoad(GameObject player)
    {   
        Debug.Log("Starting fly and load sequence");
        if (launchEffectPrefab != null && effectSpawnPoints != null && effectSpawnPoints.Length > 0)
        {
            // for (int i = 0; i < effectSpawnPoints.Length; i++)
            // {
            //     Debug.Log("Spawning explosion at index " + i + ": " + effectSpawnPoints[i].name);
            //     Instantiate(launchEffectPrefab, effectSpawnPoints[i].position, Quaternion.identity);
            // }

        }
        StartCoroutine(FlyThenLoad(player));
    }

    IEnumerator FlyThenLoad(GameObject player)
    {
        shouldFly = true;

        yield return new WaitForSeconds(0.01f);    // Slight delay before disappearing
        player.SetActive(false);                 // Hide player

        yield return new WaitForSeconds(3f);    // Let ship fly visibly

        // Track level progress before loading
        LevelManager.Instance.TrackLevelCompletion(SceneManager.GetActiveScene().name, Time.timeSinceLevelLoad);
        LevelManager.Instance.TrackGravityCount();
        LevelManager.Instance.TrackCloneUsageData(SceneManager.GetActiveScene().name);

        // Load next level
        LoadNextLevel();
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            winText.SetActive(true);
            StartCoroutine(ReturnToStartScreen());
        }
    }

    IEnumerator ReturnToStartScreen()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (shouldFly)
        {
            transform.Translate(Vector3.up * flySpeed * Time.deltaTime);
        }
    }
}
