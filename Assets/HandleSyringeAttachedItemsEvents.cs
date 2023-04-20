using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandleSyringeAttachedItemsEvents : MonoBehaviour
{

    public Syringe syringe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void handleAttachEvents(SelectEnterEventArgs args)
    {
        //WE MIGHT NEED THIS
    }

    public void handleDetachEvents(SelectExitEventArgs args)
    {
        Needle needle = args.interactableObject.transform.GetComponent<Needle>();
        if (needle)
        {
            needle.needleDetachedEvent(syringe);
        }

        
    }
}
