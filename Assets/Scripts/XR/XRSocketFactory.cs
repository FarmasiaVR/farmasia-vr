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

    private List<XRBaseInteractable> instToDelete = new List<XRBaseInteractable>();

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

        //This is to make sure that the socket doesn't try to spawn new objects while it is exiting the game.
        if (gameObject.scene.isLoaded) return;

        spawnedInst = Instantiate(socketInteractable);
        spawnedInst.transform.position = transform.position;
        instToDelete.Add(spawnedInst);
        socket.interactionManager.SelectEnter(socket.GetComponent<IXRSelectInteractor>(), spawnedInst);
    }

    private void OnDestroy()
    {
        foreach (XRBaseInteractable inst in instToDelete.ToArray()) {
            if (inst != null) {
                Destroy(inst.gameObject);
            }
        }
    }


}
