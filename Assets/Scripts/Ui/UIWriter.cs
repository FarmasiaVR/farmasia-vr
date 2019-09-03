using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWriter : MonoBehaviour
{
    Canvas canvas;

    public String toggleChild(String childName) {
        GameObject child = returnChild(childName);
    }

    public String writeToName(String childName) {
        
    }

    private GameObject returnChild(String childName) {
        GameObject child = canvas.gameObject.transform.Find("childName");
        return child;
    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    void Update()
    {
        
    }
}
