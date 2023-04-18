using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
       List<IXRSelectInteractor> selectors =  interactableToTrack.interactorsSelecting;

        int selectorNum = 0;
        foreach(IXRSelectInteractor selector in selectors)
        {
            Debug.Log(selectorNum.ToString()+" " + selector.transform.position);
            selectorNum++;
        }


    }

    
    
    public void DetachSyringeFromLuerlock(ActivateEventArgs args)
    {
        IXRActivateInteractor interactor = args.interactorObject;

        Vector3 socket1Pos = syringe1Socket.transform.position;
        Vector3 socket2Pos = syringe2Socket.transform.position;
        Vector3 activatorHandPosition = interactor.transform.position;

        float distanceToSocket1 = Vector3.Distance(activatorHandPosition, socket1Pos);
        float distanceToSocket2 = Vector3.Distance(activatorHandPosition, socket2Pos);




        if(distanceToSocket1 < distanceToSocket2)
        {
            syringe1Socket.detachSyringe();
        }
        else
        {
            syringe2Socket.detachSyringe();
        }
       










    }
}
