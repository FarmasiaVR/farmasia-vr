
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


//this is crazy prototyping code from every software devs nightmares, do not try this at home
public class XRLuerlock : MonoBehaviour
{
    // Start is called before the first frame update
    public XRGrabInteractable interactableToTrack;


    public XRJoinSyringeToLuerlock syringe1Socket;
    public XRJoinSyringeToLuerlock syringe2Socket;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    XRJoinSyringeToLuerlock findClosestSocket(Vector3 pos)
    {
        Vector3 socket1Pos = syringe1Socket.transform.position;
        Vector3 socket2Pos = syringe2Socket.transform.position;
       

        float distanceToSocket1 = Vector3.Distance(socket1Pos, pos);
        float distanceToSocket2 = Vector3.Distance(socket2Pos, pos);
        Debug.Log("Finding closest socket");
        Debug.Log(distanceToSocket1 + "   " + syringe1Socket.gameObject.name);
        Debug.Log(distanceToSocket2 + "   " + syringe2Socket.gameObject.name);

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

        findClosestSocket(interactor.transform.position).detachSyringe();
    }

    public void sendMedicine(InputAction.CallbackContext context)
    {
        Debug.Log("sending medicine");
        TrackedDevice device = (TrackedDevice)context.control.device;
        Vector3 devicePos = device.devicePosition.ReadValue();
        Debug.Log("the device pos:");
        Debug.Log(devicePos);

        //dev testing...
        LiquidContainer target =  findFurthestSocket(devicePos).attachSocket.firstInteractableSelected.transform.GetComponentInChildren<LiquidContainer>();
        findClosestSocket(devicePos).attachSocket.firstInteractableSelected.transform.GetComponent<Syringe>().SendMedicineToLuerlockXR(target);
    }


    public void takeMedicine(InputAction.CallbackContext context)
    {
        Debug.Log("taking medicine");
        TrackedDevice device = (TrackedDevice) context.control.device;
        Vector3 devicePos = device.devicePosition.ReadValue();
        Debug.Log("the device pos:");
        Debug.Log(devicePos);

        //dev testing...
        LiquidContainer target = findFurthestSocket(devicePos).attachSocket.firstInteractableSelected.transform.GetComponentInChildren<LiquidContainer>();
        findClosestSocket(devicePos).attachSocket.firstInteractableSelected.transform.GetComponent<Syringe>().TakeMedicineFromLuerlockXR(target);

    }
}
