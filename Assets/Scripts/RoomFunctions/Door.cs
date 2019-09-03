using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    UIWriter writer;


    void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.name == "Player") {
            
            writer.toggleChild("Prompt Field");


        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "Player") {
            
            writer.toggleChild("Prompt Field");


        }
    }

    
    void Start()
    {
        writer = GameObject.Find("Canvas").GetComponent<UIWriter>();
    }

    void Update()
    {
        
    }
}
