using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [HideInInspector] public string firebaseURL = "https://doppledash-2a42c-default-rtdb.firebaseio.com/"; 

    private LevelCompletionTracker levelCompletionTracker;
    private PlayerDeathTracker playerDeathTracker;
    private GravityShiftTracker gravityShiftTracker;
    private CloneUsageTracker cloneUsageTracker;

    void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        levelCompletionTracker = GetComponent<LevelCompletionTracker>();
        if (levelCompletionTracker == null)
            levelCompletionTracker = gameObject.AddComponent<LevelCompletionTracker>();

        playerDeathTracker = GetComponent<PlayerDeathTracker>();
        if (playerDeathTracker == null)
            playerDeathTracker = gameObject.AddComponent<PlayerDeathTracker>();

        gravityShiftTracker = GetComponent<GravityShiftTracker>();
        if (gravityShiftTracker == null)
            gravityShiftTracker = gameObject.AddComponent<GravityShiftTracker>();

        cloneUsageTracker = GetComponent<CloneUsageTracker>();
        if (cloneUsageTracker == null)
            cloneUsageTracker = gameObject.AddComponent<CloneUsageTracker>();
    }
    else
    {
        Destroy(gameObject);
    }
}

    public void TrackPlayerStart(string levelName)
    {
        string playerID = Guid.NewGuid().ToString();
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string json = $"{{\"player_id\":\"{playerID}\", \"level_name\":\"{levelName}\", \"timestamp\":\"{timestamp}\"}}";

        StartCoroutine(FirebaseUtility.SendDataToFirebase(json, "players"));

    }

    public void TrackLevelCompletion(string levelName, float completionTime)
    {
        levelCompletionTracker.TrackLevelCompletion(levelName, completionTime);
    }

    public void TrackPlayerDeath(string levelName, Vector3 deathPosition, string playerType, string obstacleType)
    {
        playerDeathTracker.TrackPlayerDeath(levelName, deathPosition, playerType, obstacleType);
    }

    public void TrackGravityCount()
    {
        gravityShiftTracker.TrackGravityCount();
    }

    public void TrackCloneUsage()
    {
        cloneUsageTracker.TrackCloneUsage();
    }

    public void TrackCloneUsageData(string levelName)
    {
        cloneUsageTracker.SendCloneUsageData(levelName);
    }
}