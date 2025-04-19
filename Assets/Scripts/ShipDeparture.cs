using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipDeparture : MonoBehaviour
{
    public float flySpeed = 7f;
    private bool shouldFly = false;

    public GameObject launchEffectPrefab;
    public Transform effectSpawnPoint;

    public void StartFlyAndLoad(GameObject player)
    {
        if (launchEffectPrefab && effectSpawnPoint)
        {
            Instantiate(launchEffectPrefab, effectSpawnPoint.position, Quaternion.identity);
        }

        StartCoroutine(FlyThenLoad(player));
    }

    IEnumerator FlyThenLoad(GameObject player)
    {
        shouldFly = true;

        yield return new WaitForSeconds(0.01f); // Let player be visible momentarily
        player.SetActive(false);               // Player disappears

        yield return new WaitForSeconds(3f); // Ship flies visibly

        // Track level stats
        LevelManager.Instance.TrackLevelCompletion(SceneManager.GetActiveScene().name, Time.timeSinceLevelLoad);
        LevelManager.Instance.TrackGravityCount();
        LevelManager.Instance.TrackCloneUsageData(SceneManager.GetActiveScene().name);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Update()
    {
        if (shouldFly)
        {
            transform.Translate(Vector3.up * flySpeed * Time.deltaTime);
        }
    }
}
