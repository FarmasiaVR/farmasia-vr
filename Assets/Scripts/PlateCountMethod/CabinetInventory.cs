using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetInventory : MonoBehaviour
{

    [Tooltip("List of objects to be added to laminar cabinet.")]
    public List<GameObject> targetInventory;

    // Current items in cabinet
    private List<GeneralItem> currentInventory;
    // Stores information 
    private Dictionary<GeneralItem, bool> correctItemsInInventory;

    void Start()
    {
        currentInventory = new List<GeneralItem>();

        if (correctItemsInInventory == null)
            correctItemsInInventory = new Dictionary<GeneralItem, bool>();

        foreach (GameObject targetItem in targetInventory)
        {
            correctItemsInInventory.Add(ConvertToGeneralItem(targetItem), false);
        }
    }

    private bool IsTarget(GeneralItem item)
    {
        return correctItemsInInventory.ContainsKey(item);
    }

    // Checks if the cabinet contains all needed items
    private bool AllItemsArePlaced()
    {
        bool allTrue = true;
        foreach (bool value in correctItemsInInventory.Values)
        {
            if (!value)
            {
                allTrue = false;
                break;
            }
        }
        return allTrue;
    }

    private void MarkItem(GeneralItem item, bool value)
    {
        correctItemsInInventory[item] = value;
    }

    // Adds items to cabinet. Returns true if all necessary items are placed, false otherwise
    public bool AddItem(GeneralItem item)
    {
        currentInventory.Add(item);
        Debug.Log(item + " added to laminar cabinet");

        if (IsTarget(item))
        {
            Debug.Log(item + " was in target list.");
            MarkItem(item, true);
            bool complete = AllItemsArePlaced();
            Debug.Log("All items are placed = " + complete);
            return complete;
        }
        return false;
    }

    // Removes items from cabinet when they leave collider
    public void RemoveItem(GeneralItem item)
    {
        currentInventory.Remove(item);
        if (IsTarget(item))
        {
            MarkItem(item, false);
        }
        
    }

    // Checks if cabinet contains an item
    public bool isInCabinet(GeneralItem item)
    {
        if (currentInventory.Contains(item))
        {
            return true;
        }
        return false;
    }

    private GeneralItem ConvertToGeneralItem(GameObject obj)
    {
        GeneralItem item = obj.GetComponent<GeneralItem>();
        return item;
    }
}
