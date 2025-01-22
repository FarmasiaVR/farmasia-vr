using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class XRSocketFactory : MonoBehaviour
{
    /// <summary>
    /// This script is used to turn an XR socket into a factory, meaning that whenever an object is grabbed from a socket, another one spawns in its place.
    /// </summary>
    private XRSocketInteractor socket;
    public XRBaseInteractable socketInteractable;
    private XRBaseInteractable spawnedInst;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectExited.AddListener(SpawnInteractableToSocket);
        socket.showInteractableHoverMeshes = false;
        SceneManager.sceneLoaded += SpawnInteractableOnSceneLoad;
    }



    public void SpawnInteractableToSocket(SelectExitEventArgs eventArgs)
    {
        ///<summary>
        ///Spawns the XR interactable in the world and forces the socket to select it
        /// </summary>

        //This is to make sure that the socket doesn't try to spawn new objects while it is exiting the game.
        if (!gameObject) return;

        if (!gameObject.scene.isLoaded) return;

        spawnedInst = Instantiate(socketInteractable);
        spawnedInst.transform.position = transform.position;
        socket.interactionManager.SelectEnter(socket.GetComponent<IXRSelectInteractor>(), spawnedInst);
    }

    private void SpawnInteractableOnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        ///<summary>
        ///Spawns an interactable into the socket when the scene is loaded.
        ///Running the SpawnInteractableToSocket in Start doesn't spawn the interactable into the socket.
        /// </summary>
        SpawnInteractableToSocket(new SelectExitEventArgs());
    }

    private void OnDestroy()
    {
        socket.selectExited.RemoveAllListeners();
        SceneManager.sceneLoaded -= SpawnInteractableOnSceneLoad;
    }


}
