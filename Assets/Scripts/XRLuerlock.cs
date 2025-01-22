using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;


//this is crazy prototyping code from every software devs nightmares, do not try this at home
public class XRLuerlock : MonoBehaviour
{
    // Start is called before the first frame update
    public XRGrabInteractable interactableToTrack;


    public XRJoinSyringeToLuerlock syringe1Socket;
    public XRJoinSyringeToLuerlock syringe2Socket;
    public float detachDistance;
    enum transferMode
    {
        take,
        send
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        luerlockSyringeDetachUpdate();
    }

    //Checks all interactors who are currently selecting(grabbing) the luerlock
    //and detaches a syringe if selector is detachDistance away from a socket
    void luerlockSyringeDetachUpdate()
    {
        List<IXRSelectInteractor> interactors = interactableToTrack.interactorsSelecting;
        foreach (var interactor in interactors)
        {
            float distanceToClosestSocket = getDistanceToClosestSocket(interactor.transform.position);
            if (distanceToClosestSocket > detachDistance)
            {
                IXRSelectInteractable detachedSyringe = detachSyringeFromClosestSocket(interactor.transform.position);
                interactableToTrack.interactionManager.SelectEnter(interactor, detachedSyringe);
            }
        }
    }

    float getDistanceToClosestSocket(Vector3 pos)
    {
        Vector3 socket1Pos = syringe1Socket.transform.position;
        Vector3 socket2Pos = syringe2Socket.transform.position;


        float distanceToSocket1 = Vector3.Distance(socket1Pos, pos);
        float distanceToSocket2 = Vector3.Distance(socket2Pos, pos);

        if (distanceToSocket1 < distanceToSocket2)
        {
            return distanceToSocket1;
        }
        else
        {
            return distanceToSocket2;
        }
    }

    XRJoinSyringeToLuerlock findClosestSocket(Vector3 pos)
    {
        Vector3 socket1Pos = syringe1Socket.transform.position;
        Vector3 socket2Pos = syringe2Socket.transform.position;
       

        float distanceToSocket1 = Vector3.Distance(socket1Pos, pos);
        float distanceToSocket2 = Vector3.Distance(socket2Pos, pos);

        if (distanceToSocket1 < distanceToSocket2)
        {
            return syringe1Socket;
        }
        else
        {
            return syringe2Socket;
        }
    }

    XRJoinSyringeToLuerlock findFurthestSocket(Vector3 pos)
    {
        Vector3 socket1Pos = syringe1Socket.transform.position;
        Vector3 socket2Pos = syringe2Socket.transform.position;


        float distanceToSocket1 = Vector3.Distance(socket1Pos, pos);
        float distanceToSocket2 = Vector3.Distance(socket2Pos, pos);
        Debug.Log("Finding furthest socket");
        Debug.Log(distanceToSocket1 + "   " + syringe1Socket.gameObject.name);
        Debug.Log(distanceToSocket2 + "   " + syringe2Socket.gameObject.name);


        if (distanceToSocket1 > distanceToSocket2)
        {
            return syringe1Socket;
        }
        else
        {
            return syringe2Socket;
        }
    }



    public void DetachSyringeFromLuerlock(ActivateEventArgs args)
    {
        IXRActivateInteractor interactor = args.interactorObject;

        detachSyringeFromClosestSocket(interactor.transform.position);
    }

    IXRSelectInteractable detachSyringeFromClosestSocket(Vector3 interactorPosition)
    {
        XRJoinSyringeToLuerlock closestSocket = findClosestSocket(interactorPosition);
        XRJoinSyringeToLuerlock furtestSocket = findFurthestSocket(interactorPosition);

        //if the other socket has no object attached, disable it to prevent it from stealing the detached syringe again.
        if (!furtestSocket.objAttached)
        {
            StartCoroutine(furtestSocket.disableSocketFor(1.0f));
        }

        IXRSelectInteractable detachedSyringe = closestSocket.detachSyringe();
        return detachedSyringe;
    }

    public void sendMedicine(InputAction.CallbackContext context)
    {
        Debug.Log("sending medicine");
        transferMedicine(context, transferMode.send);
    }


    public void takeMedicine(InputAction.CallbackContext context)
    {
        Debug.Log("taking medicine");
        transferMedicine(context, transferMode.take);
    }
   
    void transferMedicine(InputAction.CallbackContext context, transferMode mode)
    {
        TrackedDevice device = (TrackedDevice)context.control.device;
        Vector3 devicePos = device.devicePosition.ReadValue();
        Debug.Log("the device pos:");
        Debug.Log(devicePos);

        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").gameObject.transform.position;
        Vector3 deviceWorldPos = playerPos + devicePos;

        LiquidContainer target = findFurthestSocket(deviceWorldPos).attachSocket.firstInteractableSelected.transform.GetComponentInChildren<LiquidContainer>();
        Syringe syringe = findClosestSocket(deviceWorldPos).attachSocket.firstInteractableSelected.transform.GetComponent<Syringe>();
        
        if (mode == transferMode.take)
        {
            syringe.TakeMedicineFromLuerlockXR(target);
        }
        else if (mode == transferMode.send) {
            syringe.SendMedicineToLuerlockXR(target);
        }
    }
}
