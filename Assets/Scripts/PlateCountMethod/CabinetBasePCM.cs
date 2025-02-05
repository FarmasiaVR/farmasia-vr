using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using FarmasiaVR;
using UnityEngine.Localization.SmartFormat.PersistentVariables;


public class CabinetBasePCM : MonoBehaviour {

    public PlateCountMethodSceneManager sceneManager;
    private bool sterileDrapefolded;
    private bool questCompleted = false;

    public Animator sterileDrape;
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

        
        if(!(item.Contamination == GeneralItem.ContaminateState.Clean)){
            sceneManager.GeneralMistake("Dirty!",1);            
            Debug.Log("Dirty: " + other.gameObject.name);
            return;
        }

        if (questCompleted) {
            return;
        }

        Debug.Log("Collide: " + other.gameObject.name);

        if (requiredItems.Contains(other.gameObject)){            
            int index = requiredItems.IndexOf(other.gameObject);  // Get the index of the item in the list
            itemsFound[index] = true;  // Mark the item as found
            Debug.Log($"{other.gameObject.name}, index {index} found in index.");
            allReady = true;
        }
        /* This can be used if we want to add penalty for bringing unnecessary items to the workstation
        else{
             sceneManager.GeneralMistake("Do you realy need it ?",1);
             Debug.Log($"{other.gameObject.name} is not in the required list.");
        }*/

        foreach(bool i in itemsFound){
            if(!i) allReady = false;
        }

        if (allReady){
            Debug.Log($"Complete");
            sceneManager.CompleteTask("toolsToCabinet");
            questCompleted = true;
        }        
        
        UnfoldSterileDrape();
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
    }

    private void UnfoldSterileDrape() {
        if (sterileDrapefolded) {
            return;
        }
        sterileDrapefolded = true;
        sterileDrape.SetBool("ItemPlaced", true);
    }


}
