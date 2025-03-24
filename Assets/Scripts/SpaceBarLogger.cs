using UnityEngine;
using System.Collections.Generic;

public static class SpaceBarLogger
{
    public static string playerId = System.Guid.NewGuid().ToString();
    public static string levelName = "";

    public static Dictionary<string, GravityEvent> playerLogs = new Dictionary<string, GravityEvent>();
    public static Dictionary<string, GravityEvent> cloneLogs = new Dictionary<string, GravityEvent>();

    public static int playerGravityCount = 0;
    public static int cloneGravityCount = 0;
    public static int totalCount = 0;

    public static void LogSpacePress(string source, Vector2 position)
    {
        if (string.IsNullOrEmpty(levelName))
        {
            levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        string key = position.x.ToString("F2") + "," + position.y.ToString("F2");

        if (source == "Player")
        {
            if (playerLogs.ContainsKey(key))
                playerLogs[key].gravity_count++;
            else
                playerLogs[key] = new GravityEvent(position);

            playerGravityCount++;
        }
        else if (source == "Clone")
        {
            if (cloneLogs.ContainsKey(key))
                cloneLogs[key].gravity_count++;
            else
                cloneLogs[key] = new GravityEvent(position);

            cloneGravityCount++;
        }

        totalCount++;
    }

    public static void ResetLogger()
    {
        playerLogs.Clear();
        cloneLogs.Clear();
        playerGravityCount = 0;
        cloneGravityCount = 0;
        totalCount = 0;
        levelName = "";
        playerId = System.Guid.NewGuid().ToString();
    }

    [System.Serializable]
    public class GravityEvent
    {
        public float x;
        public float y;
        public int gravity_count = 1;

        public GravityEvent(Vector2 position)
        {
            x = position.x;
            y = position.y;
        }
    }

    // [System.Serializable]
    // public class GravityWrapper
    // {
    //     public List<GravityEvent> logs;

    //     public GravityWrapper(Dictionary<string, GravityEvent> logDict)
    //     {
    //         logs = new List<GravityEvent>(logDict.Values);
    //     }
    // }
}
