using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "LaminarInventory", menuName = "FarmasiaVR/LaminarInventory", order = 2)]
public class LaminarCabinetInventory : ScriptableObject
{
    [Tooltip("List of objects to be added to laminar cabinet. Please write the exact class name.")]
    public List<string> targetInventory;

    // Current items in cabinet
    private List<GeneralItem> currentInventory;
    // Stores information 
    private Dictionary<string, bool> correctItemsInInventory;

    // Resets the inventory and initializes the dictionary
    public void Enable()
    {
        currentInventory.Clear();

        if (correctItemsInInventory == null)
            correctItemsInInventory = new Dictionary<string, bool>();

        foreach (string targetItem in targetInventory)
        {
            correctItemsInInventory.Add(targetItem, false);
        }
    }

    private bool IsTarget(GeneralItem item)
    {
        return targetInventory.Contains(item.GetType().Name);
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
        correctItemsInInventory[item.GetType().Name] = value;
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
        else return false;
    }
}
