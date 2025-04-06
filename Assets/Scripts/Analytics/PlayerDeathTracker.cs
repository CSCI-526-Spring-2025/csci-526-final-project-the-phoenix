using UnityEngine;
using System;

public class PlayerDeathTracker : MonoBehaviour
{
    public void TrackPlayerDeath(string levelName, Vector3 deathPosition, string playerType)
    {
        string playerID = Guid.NewGuid().ToString();
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string json = $"{{\"player_id\":\"{playerID}\", \"level_name\":\"{levelName}\", \"player_type\":\"{playerType}\", \"death_x\":{deathPosition.x}, \"death_y\":{deathPosition.y}, \"timestamp\":\"{timestamp}\"}}";

        StartCoroutine(FirebaseUtility.SendDataToFirebase(json, "player_deaths"));
    }
}