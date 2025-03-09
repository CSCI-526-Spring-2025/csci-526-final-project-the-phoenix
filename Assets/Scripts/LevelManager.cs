using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance; // Singleton instance

    private string firebaseURL = "https://doppledash-2a42c-default-rtdb.firebaseio.com/"; // Replace with your Firebase URL

    void Awake()
    {
        // Ensure there's only one instance of LevelManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TrackLevelCompletion(string levelName, float completionTime)
    {
        string playerID = SystemInfo.deviceUniqueIdentifier; // Unique identifier for player
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        // Create JSON payload
        string json = $"{{\"player_id\":\"{playerID}\", \"level_name\":\"{levelName}\", \"completion_time\":{completionTime}, \"timestamp\":\"{timestamp}\"}}";

        // Start the coroutine to send data
        StartCoroutine(SendDataToFirebase(json));
    }

    IEnumerator SendDataToFirebase(string json)
    {
        using (UnityWebRequest uwr = new UnityWebRequest(firebaseURL + "level_completion.json", "POST"))
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
