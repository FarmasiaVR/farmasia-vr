using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Text.RegularExpressions; 
using Newtonsoft.Json;
using UnityEngine.Localization;
using UnityEngine.Events;

public class ServerPostRequest : MonoBehaviour
{
    // This is all a test setup to test communication between Openshift FarmasiaVr backend and FarmasiaVr game 
    // When ready switch the SendPostRequest function to receive the jsonData as a parameter from the summary script
    // The jsonData should be the players email address, and the scene summary. Also an authToken password is to be inserted
    // by the player so the backend approves the POST request, the password can be changed in the backend UI if needed.

    public string serverUrl = "https://opetushallinto.cs.helsinki.fi/farmasiavr-backend/api/certificates/create";
    public string authToken;
    public string emailAccount;

    public TaskManager taskManager;
    private bool isEmailValid = false;
    private bool isTokenValid = false;

    public UnityEvent<string> notifyPlayer;
    public void SendPostRequest()
    {
        if (!isEmailValid || !isTokenValid) {
            Debug.Log("Invalid");
            NotifyPlayer("SubmissionFailed");
            return;
        }
        Dictionary<string, dynamic> jsonDict = taskManager.GetJSONData();
        jsonDict.Add("email", emailAccount);
        string json = JsonConvert.SerializeObject(jsonDict);
        Debug.Log(json);

        StartCoroutine(PostRequest(serverUrl, json));
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
    public void GetEmail(string email)
    {
        Regex validator = new Regex(@"\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}\b");
        Debug.Log(email);
        string result = validator.Match(email).Value;
        if (email != result)
        {
            Debug.Log("Invalid email");
            NotifyPlayer("InvalidEmail");
            isEmailValid = false;
            return;
        }
        isEmailValid = true;
        emailAccount = email;
        Debug.Log($"Valid: {emailAccount}");
    }

    public void GetAuthorisationToken(string token)
    {
        Regex anyCharacter = new Regex(@"\S");
        Regex whitespace = new Regex(@"\s");
        if (anyCharacter.Match(token).Length < 1)
        {
            Debug.Log("Invalid auth token");
            NotifyPlayer("InvalidToken");
            isTokenValid = false;
            return;
        }
        if (whitespace.Match(token).Length > 0)
        {
            Debug.Log("Auth token can't contain whitespace");
            NotifyPlayer("TokenWhitespace");
            isTokenValid = false;
            return;
        }
        authToken = token;
        isTokenValid = true;
        Debug.Log($"valid: {authToken}");
    }
    public void NotifyPlayer(string key)
    {
        var localizedString = new LocalizedString("CertificateSystem", key);
        localizedString.StringChanged += (localizedText) => {
            notifyPlayer.Invoke(localizedText);
        };
    }
}
