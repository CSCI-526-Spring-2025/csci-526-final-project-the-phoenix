using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [HideInInspector] public string firebaseURL = "https://doppledash-2a42c-default-rtdb.firebaseio.com/"; 

    private LevelCompletionTracker levelCompletionTracker;
    private PlayerDeathTracker playerDeathTracker;
    private GravityShiftTracker gravityShiftTracker;

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
    }
    else
    {
        Destroy(gameObject);
    }
}

    public void TrackLevelCompletion(string levelName, float completionTime)
    {
        levelCompletionTracker.TrackLevelCompletion(levelName, completionTime);
    }

    public void TrackPlayerDeath(string levelName, Vector3 deathPosition, string playerType)
    {
        playerDeathTracker.TrackPlayerDeath(levelName, deathPosition, playerType);
    }

    public void TrackGravityCount()
    {
        gravityShiftTracker.TrackGravityCount();
    }
}