using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public static class FirebaseUtility
{
    public static IEnumerator SendDataToFirebase(string json, string endpoint)
    {
        using (UnityWebRequest uwr = new UnityWebRequest(LevelManager.Instance.firebaseURL + endpoint + ".json", "POST"))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
                Debug.LogError("Error While Sending: " + uwr.error);
            else
                Debug.Log("Data Received: " + uwr.downloadHandler.text);
        }
    }
}