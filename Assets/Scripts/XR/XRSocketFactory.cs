using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketFactory : MonoBehaviour
{
    /// <summary>
    /// This script is used to turn an XR socket into a factory, meaning that whenever an object is grabbed from a socket, another one spawns in its place.
    /// </summary>
    private XRSocketInteractor socket;
    public XRBaseInteractable socketInteractable;
    private XRBaseInteractable spawnedInst;

    private void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectExited.AddListener(SpawnInteractableToSocket);
        SpawnInteractableToSocket(new SelectExitEventArgs());
    }

    public void SpawnInteractableToSocket(SelectExitEventArgs eventArgs)
    {
        ///<summary>
        ///Spawns the XR interactable in the world and forces the socket to select it
        /// </summary>
        spawnedInst = Instantiate(socketInteractable);
        socket.interactionManager.SelectEnter(socket.GetComponent<IXRSelectInteractor>(), spawnedInst);
    }

    private void OnDestroy()
    {
        Destroy(spawnedInst);
    }


}
