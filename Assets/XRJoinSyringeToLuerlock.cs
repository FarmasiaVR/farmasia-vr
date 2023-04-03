using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//this is crazy prototyping code from every software devs nightmares, do not try this at home
public class XRJoinSyringeToLuerlock : MonoBehaviour
{
    public XRSocketInteractor attachSocket;
    public XRGrabInteractable parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void joinSyringe(SelectEnterEventArgs args)
    {
        args.interactableObject.transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
        args.interactableObject.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        args.interactableObject.transform.parent = parent.transform;
        args.interactableObject.transform.localScale= parent.transform.localScale;
       
       
        foreach (Collider coll in args.interactableObject.colliders)
        {
            parent.colliders.Add(coll);
        }

        args.interactableObject.transform.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("CanAttachToLuerlock");
       

       

    }

    public void detachSyringe()
    {
        IXRSelectInteractable attachedSyringe = attachSocket.firstInteractableSelected;
        attachedSyringe.transform.gameObject.GetComponent<Rigidbody>().useGravity = true;
        attachedSyringe.transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        string[] masks = { "CanAttachToLuerlock", "InteractableByPlayer" };
        attachedSyringe.transform.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask(masks);

        XRInteractionManager manager = attachSocket.interactionManager;
        manager.SelectCancel(attachSocket, attachedSyringe);

        //set syringe transform to scene
        attachedSyringe.transform.parent = null;

        //for dev prototyping:
        attachSocket.GetComponent<XRSocketInteractor>().socketActive = false;
        
    }
}
