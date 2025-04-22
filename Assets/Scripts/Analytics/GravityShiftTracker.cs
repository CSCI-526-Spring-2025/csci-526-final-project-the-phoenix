using UnityEngine;
using System.Collections.Generic;

public class GravityShiftTracker : MonoBehaviour
{
    [System.Serializable]
    public class GravityEventList
    {
        public List<SpaceBarLogger.GravityEvent> player;
        public List<SpaceBarLogger.GravityEvent> clone;
    }

    public void TrackGravityCount()
    {
        List<SpaceBarLogger.GravityEvent> playerList = new List<SpaceBarLogger.GravityEvent>(SpaceBarLogger.playerLogs.Values);
        List<SpaceBarLogger.GravityEvent> cloneList = new List<SpaceBarLogger.GravityEvent>(SpaceBarLogger.cloneLogs.Values);

        GravityEventList eventList = new GravityEventList
        {
            player = playerList,
            clone = cloneList
        };

        string eventsJson = JsonUtility.ToJson(eventList);
        string json = "{" +
            $"\"player_id\":\"{SpaceBarLogger.playerId}\"," +
            $"\"level_name\":\"{SpaceBarLogger.levelName}\"," +
            $"\"timestamp\":\"{System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}\"," +
            $"\"totalCount\":{SpaceBarLogger.totalCount}," +
            eventsJson.TrimStart('{');

        StartCoroutine(FirebaseUtility.SendDataToFirebase(json, "gravity_logs_new"));
        StartCoroutine(FirebaseUtility.SendDataToFirebase(json, "gravity_logs"));
        SpaceBarLogger.ResetLogger();
    }
}