using UnityEngine;

public class CloneUsageTracker : MonoBehaviour
{
    private int cloneUsageCount = 0;
    private float levelStartTime;
    private float firstCloneActivationTime = -1f; 

    private void Start()
    {
        Debug.Log("CloneUsageTracker initialized in level: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        ResetCloneUsageCount();
        levelStartTime = Time.time;
        firstCloneActivationTime = -1f; 
    }

    public void TrackCloneUsage()
    {
        if (firstCloneActivationTime < 0f)
        {
            firstCloneActivationTime = Time.time - levelStartTime;
        }
    }

    public void IncrementCloneUsageCount()
    {
        cloneUsageCount++;
    }

    public void ResetCloneUsageCount()
    {
        cloneUsageCount = 0;
        firstCloneActivationTime = -1f;
        levelStartTime = Time.time;
    }

    public void SendCloneUsageData(string levelName)
    {   
        string playerID = System.Guid.NewGuid().ToString();
        string timestamp = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string json = $"{{" +
            $"\"player_id\":\"{playerID}\"," +
            $"\"level_name\":\"{levelName}\"," +
            $"\"clone_usage_count\":{cloneUsageCount}," +
            $"\"first_activation_time\":{firstCloneActivationTime}," +
            $"\"timestamp\":\"{timestamp}\"" +
            $"}}";

        StartCoroutine(FirebaseUtility.SendDataToFirebase(json, "clone_usage_new"));
    }
}