using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class syringeToggleHasSyringeCap : MonoBehaviour
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
    public void setSyringeCap(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.GetComponent<SyringeCap>())
        {
            syringe.HasSyringeCap = true;
        }
    }

    public void removeSyringeCap(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.GetComponent<SyringeCap>())
        {
            syringe.HasSyringeCap = false;
        }
    }
}
