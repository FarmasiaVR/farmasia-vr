using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
        private List<TextMeshPro> hintDescriptions;

    private void Awake() 
    {
        hintDescriptions = new List<TextMeshPro>();
        foreach (GameObject descObject in GameObject.FindGameObjectsWithTag("HintDescription"))
        {
            hintDescriptions.Add(descObject.GetComponent<TextMeshPro>());
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
