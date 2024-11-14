using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ServerPostRequest : MonoBehaviour
{
    // URL of the server, testing with a GKE Cluster
    public string serverUrl = "http://35.228.153.79/certificates/create";

    public void SendPostRequest()
    {
        string jsonData = @"
        {
            ""user"": ""John Doe"",
            ""tasks"": [
                { ""taskName"": ""task1"", ""points"": 10 },
                { ""taskName"": ""task2"", ""points"": 5 }
            ]
        }";

        StartCoroutine(PostRequest(serverUrl, jsonData));
    }

    private IEnumerator PostRequest(string url, string jsonData)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for a response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Post request succeeded. Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Post request failed: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class UserTaskData
    {
        public string User;
        public int Task1;
        public int Task2;
    }
}
