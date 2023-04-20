using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Valve.VR;
//this is crazy prototyping code from every software devs nightmares, do not try this at home
public class XRJoinSyringeToLuerlock : MonoBehaviour
{
    public XRSocketInteractor attachSocket;
    public XRGrabInteractable parent;
    public Collider colliderToToggle;

   
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
       
        //set syringe mask so that the player can't interact with it
        args.interactableObject.transform.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("CanAttachToLuerlock");

        colliderToToggle.enabled = true;
    }

    IEnumerator disableSocketFor(float seconds)
    {
        Debug.Log("disabled socket");
        attachSocket.GetComponent<XRSocketInteractor>().socketActive = false;
        yield return new WaitForSeconds(seconds);
        Debug.Log("enabled socket");
        attachSocket.GetComponent<XRSocketInteractor>().socketActive = true;
        yield return true;
    }


    public void detachSyringe()
    {
        IXRSelectInteractable attachedSyringe = attachSocket.firstInteractableSelected;
        if (attachedSyringe != null)
        {
            colliderToToggle.enabled = false;

            //set syringe layer back to normal
            attachedSyringe.transform.gameObject.GetComponent<Rigidbody>().useGravity = true;
            attachedSyringe.transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            string[] masks = { "CanAttachToLuerlock", "InteractableByPlayer" };
            attachedSyringe.transform.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask(masks);

            //detach syringe from luerlock   
            XRInteractionManager manager = parent.interactionManager;
            manager.SelectCancel(attachSocket, attachedSyringe);
            attachedSyringe.transform.parent = null;

            //prevent socket from immediately re selecting the interactable
            StartCoroutine(disableSocketFor(3.0f));
        }
    }
}
