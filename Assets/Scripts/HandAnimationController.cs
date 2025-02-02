using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandAnimationController : MonoBehaviour
{
    // Script from Justin P Barnett's tutorial https://www.youtube.com/watch?v=DxKWq7z4Xao
    
    ActionBasedController controller;
    public HandAnimation handObject;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        handObject.SetGrip(controller.selectAction.action.ReadValue<float>());
        handObject.SetTrigger(controller.activateAction.action.ReadValue<float>());
    }
}
