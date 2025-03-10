using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveItemsToTablePCM : MonoBehaviour
{
    public TaskManager taskManager;

    [System.Serializable]
    public struct ItemPair
    {
        public GameObject item;        // The item to move
        public Transform targetLocation; // Target position for the item
    }
    public ItemPair[] itemsToMove; // Array of items and their target locations
    public GameObject DisableAfterPress; // To disable the button after it is pressed

    private void Start()
    {
        // Get the XR Simple Interactable component and register the event
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnButtonPressed);
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        foreach (var itemPair in itemsToMove)
        {
            // Move the actual object
            if (itemPair.item != null && itemPair.targetLocation != null)
            {
                itemPair.item.transform.position = itemPair.targetLocation.position;
                itemPair.item.transform.rotation = itemPair.targetLocation.rotation;
            }
        }
        DisableButton();
    }

    void Update()
    {
        if (taskManager.IsTaskCompleted("toolsToCabinet"))
        {
            DisableButton();
        }
    }

    public void DisableButton()
    {
        if (DisableAfterPress != null)
        {
            DisableAfterPress.SetActive(false);
        }
    }
}
