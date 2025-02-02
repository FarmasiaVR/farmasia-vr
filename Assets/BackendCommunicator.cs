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


    [Serializable]
    public class exampleTask
    {
        public string taskName;
        public int points;
        public exampleTask(string taskName, int points)
        {
            this.taskName = taskName;
            this.points = points;
        }
    }
    [Serializable]
    public class exampleCertificate
    {
        public string user;
        public List<exampleTask> tasks = new List<exampleTask>();
    }

    void Start()
    {

        StartCoroutine(GetRequest(backendURL + "/certificates"));

        exampleCertificate exampleTask = new exampleCertificate();
        exampleTask.user = "sample user";
        exampleTask.tasks.Add(new exampleTask("fill bottles", 3));
        string serializedCertificate = JsonUtility.ToJson(exampleTask);
        Debug.Log(serializedCertificate);

        StartCoroutine(PutRequest(backendURL + "/certificates/create_put", serializedCertificate));

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
    IEnumerator PostRequest(string uri)
    {
        
        WWWForm testForm = new WWWForm();
        testForm.AddField("user", "hello from unity");
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, testForm))
        {
            
           // webRequest.SetRequestHeader("Content-Type", "application/json");
            Debug.Log(webRequest.uploadHandler.data);

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

    IEnumerator PutRequest(string uri, string jsonString)
    {

      
        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, jsonString))
        {

            webRequest.SetRequestHeader("Content-Type", "application/json");
            Debug.Log(webRequest.uploadHandler.data);

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
                    break;
            }
        }
    }
}
