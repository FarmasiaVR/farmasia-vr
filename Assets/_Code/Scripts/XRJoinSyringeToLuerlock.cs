
using System.Collections;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//this is crazy prototyping code from every software devs nightmares, do not try this at home
public class XRJoinSyringeToLuerlock : MonoBehaviour
{
    public bool objAttached;
    public XRSocketInteractor attachSocket;
    public XRGrabInteractable parent;
    public Collider colliderToToggle;

    InteractionLayerMask attachedObjecInteractionMask;
   
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

        Debug.Log("SENDING LUERLOCK ATTACH EVENT");
        Events.FireEvent(EventType.AttachLuerlock, CallbackData.Object(args.interactableObject.transform.gameObject));
        Events.FireEvent(EventType.SyringeToLuerlock, CallbackData.Object(args.interactableObject.transform.gameObject));

        args.interactableObject.transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
        args.interactableObject.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        args.interactableObject.transform.gameObject.GetComponent<CustomTakeMedicineButtonActionsXR>().enabled = false;


        args.interactableObject.transform.parent = parent.transform;
        args.interactableObject.transform.localScale= parent.transform.localScale;

        //set syringe mask so that the player can't interact with it
        attachedObjecInteractionMask = args.interactableObject.transform.GetComponent<XRGrabInteractable>().interactionLayers;
        args.interactableObject.transform.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("CanAttachToLuerlock");
        colliderToToggle.enabled = true;
        objAttached = true;
    }

    public IEnumerator disableSocketFor(float seconds)
    {
        Debug.Log("disabled socket");
        attachSocket.GetComponent<XRSocketInteractor>().socketActive = false;
        yield return new WaitForSeconds(seconds);
        Debug.Log("enabled socket");
        attachSocket.GetComponent<XRSocketInteractor>().socketActive = true;
        yield return true;
    }


    public IXRSelectInteractable detachSyringe()
    {
        IXRSelectInteractable attachedSyringe = attachSocket.firstInteractableSelected;
        if (attachedSyringe != null)
        {
            colliderToToggle.enabled = false;

            

            Debug.Log("SENDING LUERLOCK DETACH EVENT");
            Events.FireEvent(EventType.SyringeFromLuerlock, CallbackData.Object(attachedSyringe.transform.gameObject));
            //set syringe layer back to normal
            attachedSyringe.transform.gameObject.GetComponent<Rigidbody>().useGravity = true;
            attachedSyringe.transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
           
            attachedSyringe.transform.GetComponent<XRGrabInteractable>().interactionLayers = attachedObjecInteractionMask;

            //detach syringe from luerlock   
            XRInteractionManager manager = parent.interactionManager;
            manager.SelectCancel(attachSocket, attachedSyringe);
            attachedSyringe.transform.parent = null;
            attachedSyringe.transform.gameObject.GetComponent<CustomTakeMedicineButtonActionsXR>().enabled = true;

            //prevent socket from immediately re selecting the interactable
            objAttached = false;
            StartCoroutine(disableSocketFor(1.0f));
            return attachedSyringe;
        }
        return null;
    }
}
