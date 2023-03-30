using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintBoxNew : MonoBehaviour
{
    // Add TextMeshPro object in Unity editor
    [SerializeField]
    private TextMeshPro hintDesc;

    private Transform[] questionMark;

    // Start is called before the first frame update
    void Start()
    {
        // Get transforms of children objects
        questionMark = this.gameObject.GetComponentsInChildren<Transform>();
        // Hide text in the beginning
        hintDesc.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RotateHintBox();
    }

/*
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Controller (Left)") || other.CompareTag("Controller (Right)"))
        {
            hintDesc.gameObject.SetActive(true);
            //hintDescription.SetActive(true);
        }
    }*/

    private void ShowText()
    {
        hintDesc.gameObject.SetActive(true);
    }

    private void HideText()
    {
        hintDesc.gameObject.SetActive(false);
    }

    /// <summary>
    /// Method to rotate the question mark and hintbox
    /// </summary>
    private void RotateHintBox() 
    {
        questionMark[0].Rotate(Vector3.up * 20 * Time.deltaTime);
        questionMark[1].Rotate(Vector3.left * 20 * Time.deltaTime);      
    }
}
