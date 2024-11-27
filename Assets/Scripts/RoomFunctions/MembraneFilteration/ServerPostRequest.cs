using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ServerPostRequest : MonoBehaviour
{
    // This is all a test setup to test communication between Openshift FarmasiaVr backend and FarmasiaVr game 
    // When ready switch the SendPostRequest function to receive the jsonData as a parameter
    // The jsonData should be the players email address, and the scene summary.

    public string serverUrl = "https://farmasiavr-backend-ohtuprojekti-staging.ext.ocp-test-0.k8s.it.helsinki.fi/certificates/create";

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
