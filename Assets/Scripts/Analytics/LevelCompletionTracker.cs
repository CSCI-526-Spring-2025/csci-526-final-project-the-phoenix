using UnityEngine;
using System;

public class LevelCompletionTracker : MonoBehaviour
{
    public void TrackLevelCompletion(string levelName, float completionTime)
    {
        string playerID = Guid.NewGuid().ToString();
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string json = $"{{\"player_id\":\"{playerID}\", \"level_name\":\"{levelName}\", \"completion_time\":{completionTime}, \"timestamp\":\"{timestamp}\"}}";

        StartCoroutine(FirebaseUtility.SendDataToFirebase(json, "level_completion_new"));
    }
}