using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintBoxNew : MonoBehaviour
{
    // Add TextMeshPro object in Unity editor
    [SerializeField]
    private TextMeshPro hintDesc;

    private Transform[] hintBoxObjects;

    // Start is called before the first frame update
    void Start()
    {
        // Get transforms of children objects
        hintBoxObjects = this.gameObject.GetComponentsInChildren<Transform>();
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

    public void ShowText()
    {
        hintDesc.gameObject.SetActive(true);
    }

    public void HideText()
    {
        hintDesc.gameObject.SetActive(false);
    }

    /// <summary>
    /// Method to rotate the question mark and hintbox
    /// </summary>
    private void RotateHintBox() 
    {
        // Index 0 rotates the whole prefab, index 1 the first child. 
        // Due to not-so-nice default rotation and origin position, 
        // objects movement/animation rely on each other object (cannot be rotated individually)
        hintBoxObjects[0].Rotate(Vector3.up * 20 * Time.deltaTime);   
        hintBoxObjects[1].Rotate(Vector3.left * 20 * Time.deltaTime);
           
   
    }
}
