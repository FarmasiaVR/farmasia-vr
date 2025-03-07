using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveItemsToTablePCM : MonoBehaviour
{
    public GameObject[] closetItems;
    public GameObject closetParent;// Parent holding items in the closet
    public GameObject tableParent;// Parent holding items on the table

    private void Start()
    {
        // Get the XR Simple Interactable component and register the event
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnButtonPressed);
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (closetParent != null) closetParent.SetActive(false);// Disable closet items
        // Disable all closet items manually
        foreach (GameObject item in closetItems)
        {
            if (item != null) item.SetActive(false);
        }
        if (tableParent != null) tableParent.SetActive(true);// Enable table items
    }
}
