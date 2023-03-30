using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintBoxNew : MonoBehaviour
{
    [SerializeField]
    private GameObject hintDescription;
    [SerializeField]
    private TextMeshPro hintDesc;

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
            hintDesc.canvasRenderer.gameObject.SetActive(true);
            hintDescription.SetActive(true);
        }
    }

    private void RotateQuestionmark() 
    {
        //GameObject questionMark = this.GetComponent();
        Transform[] asd = this.gameObject.GetComponentsInChildren<Transform>();
        //asd[0].rotation = new Vector3(1f,0f,1f);
    }
}
