using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance; 

    private string firebaseURL = "https://doppledash-2a42c-default-rtdb.firebaseio.com/"; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TrackLevelCompletion(string levelName, float completionTime)
    {
        string playerID = Guid.NewGuid().ToString();
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string json = $"{{\"player_id\":\"{playerID}\", \"level_name\":\"{levelName}\", \"completion_time\":{completionTime}, \"timestamp\":\"{timestamp}\"}}";

        StartCoroutine(SendDataToFirebase(json, "level_completion"));
    }

    public void TrackPlayerDeath(string levelName, Vector3 deathPosition)
    {
        string playerID = Guid.NewGuid().ToString();
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string json = $"{{\"player_id\":\"{playerID}\", \"level_name\":\"{levelName}\", \"death_x\":{deathPosition.x}, \"death_y\":{deathPosition.y}, \"timestamp\":\"{timestamp}\"}}";

        StartCoroutine(SendDataToFirebase(json, "player_deaths"));
    }

    IEnumerator SendDataToFirebase(string json, string endpoint)
    {
        using (UnityWebRequest uwr = new UnityWebRequest(firebaseURL + endpoint + ".json", "POST"))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error While Sending: " + uwr.error);
            }
            else
            {
                Debug.Log("Data Received: " + uwr.downloadHandler.text);
            }
        }
    }
}
