using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Text.RegularExpressions; 

public class ServerPostRequest : MonoBehaviour
{
    // This is all a test setup to test communication between Openshift FarmasiaVr backend and FarmasiaVr game 
    // When ready switch the SendPostRequest function to receive the jsonData as a parameter from the summary script
    // The jsonData should be the players email address, and the scene summary. Also an authToken password is to be inserted
    // by the player so the backend approves the POST request, the password can be changed in the backend UI if needed.

    public string serverUrl = "https://shibboleth.ext.ocp-test-0.k8s.it.helsinki.fi/farmasiavr-backend/api/certificates/create";
    public string authToken ;
    public string emailAccount ;

    public void SendPostRequest()
    {
        Debug.Log("We got here");
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
    public void GetEmail(string email) {
        Regex validator = new Regex(@"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}\b");
        Debug.Log(email);
        string result = validator.Match(email).Value;
        if (email!=result) {
            Debug.Log("Invalid email");
            return;
        }
        emailAccount = email;
        Debug.Log($"Valid: {emailAccount}");
    }

    public void GetAuthorisationToken(string token) {
        Regex anyCharacter = new Regex(@"\S");
        Regex whitespace = new Regex(@"\s");
        if (anyCharacter.Match(token).Length < 1) {
            Debug.Log("Invalid auth token");
            return;
        }
        if (whitespace.Match(token).Length > 0) {
            Debug.Log("Auth token can't contain whitespace");
            return;
        }
        authToken = token;
        Debug.Log($"valid: {authToken}");
    }
}
