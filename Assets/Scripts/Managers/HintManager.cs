using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FarmasiaVR.New;

public class HintManager : MonoBehaviour
{
    private List<HintBoxNew> hintDescriptions;


    private void Awake() 
    {
        hintDescriptions = new List<HintBoxNew>();
        foreach (GameObject descObject in GameObject.FindGameObjectsWithTag("Hint"))
        {
            hintDescriptions.Add(descObject.GetComponent<HintBoxNew>());
        }
    }


    /// <summary>
    /// Updates all hint texts in the scene. Set in Unity editor as a Task Manager Task Event.
    /// </summary>
    public void UpdateHintDescriptions(Task task)
    {
        foreach (HintBoxNew taskHint in hintDescriptions)
        {
            taskHint.HideText();
            taskHint.UpdateText(task);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
