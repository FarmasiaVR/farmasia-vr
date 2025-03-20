using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using FarmasiaVR;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public class CabinetBasePCM : MonoBehaviour {

    public PlateCountMethodSceneManager sceneManager;
    private bool questCompleted = false;

    public List<GameObject> requiredItems;  
    private List<bool> itemsFound;
    private bool allReady = false;
 
    private void Start() {

        itemsFound = new List<bool>();
        for (int i = 0; i < requiredItems.Count; i++)
        {
            itemsFound.Add(false);  // Initially, no items are found
        }

    }

    private void OnTriggerEnter(Collider other)
    {        
        GeneralItem item = other.GetComponent<GeneralItem>();

        if (item == null) {
            return;
        }
	sceneManager.objectsInLaminarCabinet.Add(other.gameObject);

        if (item.GetComponent<CoverOpeningHandler>() != null)
        {
            CoverOpeningHandler coverItem = item.GetComponent<CoverOpeningHandler>(); 
            coverItem?.completeAction();
        }


        if(!(item.Contamination == GeneralItem.ContaminateState.Clean)){
            sceneManager.GeneralMistake("DirtyItemInCabinet", 1);
            GUIConsole.Log("Dirty: " + other.gameObject.name);                       
            //Debug.Log("Dirty: " + other.gameObject.name);
            CleanItem(item);
        }



        if (requiredItems.Contains(other.gameObject)){            
            int index = requiredItems.IndexOf(other.gameObject);  // Get the index of the item in the list
            itemsFound[index] = true;  // Mark the item as found
            //Debug.Log($"{other.gameObject.name}, index {index} found in index.");
        }
        /* This can be used if we want to add penalty for bringing unnecessary items to the workstation
        else{
             sceneManager.GeneralMistake("Do you realy need it ?",1);
             Debug.Log($"{other.gameObject.name} is not in the required list.");
        }*/
        CheckCompletion();

        if (questCompleted) {
            return;
        }
    }
    private void CleanItem(GeneralItem item)
    {
        CleaningBottlePCM cleaningBottle = FindObjectOfType<CleaningBottlePCM>();
        if (cleaningBottle != null) {
            cleaningBottle.Clean();
            item.Contamination = GeneralItem.ContaminateState.Clean;
            Debug.Log($"{item.gameObject.name} has been cleaned.");

        } else {
            Debug.LogWarning("No Cleaning Bottle found in the scene!");
        }
    }

    private void CheckCompletion()
    {
        allReady = itemsFound.All(found => found);
        
        if (allReady) {
            Debug.Log("Complete");
            sceneManager.CompleteTask("toolsToCabinet");
            questCompleted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {   
        if (questCompleted) {
            return;
        }     
        GeneralItem item = other.GetComponent<GeneralItem>();
        
        if (item == null) {
            return;
        }

        if(!(item.Contamination == GeneralItem.ContaminateState.Clean)){            
            return;
        }

        if (requiredItems.Contains(other.gameObject)){            
            int index = requiredItems.IndexOf(other.gameObject);  // Get the index of the item in the list
            itemsFound[index] = false; 
        }

        sceneManager.objectsInLaminarCabinet.Add(other.gameObject);
    }
}
