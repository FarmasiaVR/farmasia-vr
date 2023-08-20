using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class BackendCommunicator : MonoBehaviour
{
    // Start is called before the first frame update


    //WARNING this is a temporary backend url, it is never quaranteed to be up and running!
    //only for development testing for now...
    string backendURL = "https://certificate-backend-temporary-0395420348.onrender.com";
    public TMP_Text resultText;


    public class Task
    {
        public string taskName;
        public int points;
        public Task(string taskName, int points)
        {
            this.taskName = taskName;
            this.points = points;
        }
    }
    public class exampleCertificateObject{
        public string user = "hello from unity";
        public Task[] tasks = { new Task("debug task name", 2) };
    };
    void Start()
    {
        
        StartCoroutine(GetRequest(backendURL + "/certificates"));

        exampleCertificateObject newCertificate = new exampleCertificateObject();
        string certificateJson = JsonUtility.ToJson(newCertificate);
        StartCoroutine(PostRequest(backendURL + "/certificates/create", certificateJson));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //source:
    //https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.Get.html 
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    if(resultText){
                        resultText.text = webRequest.downloadHandler.text;
                    }
                    break;
            }
        }
    }


    //source:
    //https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.Post.html 
    IEnumerator PostRequest(string uri, string jsonStringObject)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, jsonStringObject))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    if (resultText)
                    {
                        resultText.text = webRequest.downloadHandler.text;
                    }
                    break;
            }
        }
    }
}
