using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ServerPostRequest : MonoBehaviour
{
    // This is all a test setup to test communication between Openshift FarmasiaVr backend and FarmasiaVr game 
    // When ready switch the SendPostRequest function to receive the jsonData as a parameter from the summary script
    // The jsonData should be the players email address, and the scene summary. Also an authToken password is to be inserted
    // by the player so the backend approves the POST request, the password can be changed in the backend UI if needed.

    public string serverUrl = "https://shibboleth.ext.ocp-test-0.k8s.it.helsinki.fi/farmasiavr-backend/api/certificates/create";
    public string authToken = "12345";
    public string emailAccount = "";

    public void SendPostRequest()
    {
        string jsonData = @"
        {
            ""email"": ""test@email.com"",
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
            request.SetRequestHeader("Authorization", authToken);

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
        public string EmailAccount;
        public int Task1;
        public int Task2;
    }
}
