using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintBoxNew : MonoBehaviour
{
    private TextMeshPro hintDescription;
    private void Awake() 
    {
        hintDescription = GetComponent<TextMeshPro>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Controller (Left)") || other.CompareTag("Controller (Right)"))
        {
            //hintDescription.setActive(true);
        }
    }
}
