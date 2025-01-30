using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using FarmasiaVR.Legacy;
using UnityEngine.Localization.SmartFormat.PersistentVariables;


public class CabinetBasePCM : MonoBehaviour {

    public PlateCountMethodSceneManager sceneManager;
    private bool sterileDrapefolded;

    public Animator sterileDrape;

    private Dictionary<string, int> requiredItems = new Dictionary<string, int>{
        { "Bottle100ml XR", 2 },  // Requires 2 Bottle100ml XR

        };   
    
    private Dictionary<string, int> inPlaceItems = new Dictionary<string, int>{
        { "Bottle100ml XR", 0 }, 

        };   
    private void Start() {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered by: " + other.gameObject.name);
        GeneralItem item = other.GetComponent<GeneralItem>();
        
        if (item == null) {
            return;
        }

        if(!item.isClean){
            sceneManager.GeneralMistake("Dirty!",1);
            //Task.CreateGeneralMistake(Translator.Translate("XR MembraneFilteration 2.0", "FloorContaminedInCabinet"), 1);
            Debug.Log("Dirty: " + other.gameObject.name);
            return;
        }

        if (requiredItems.ContainsKey(other.gameObject.name)){
            inPlaceItems[other.gameObject.name]++;  // Increase count if the item exists            
            Debug.Log($"Added {other.gameObject.name}. New count: {inPlaceItems[other.gameObject.name]}");
        }
        else{
            sceneManager.GeneralMistake("Do you realy need it ?",1);
            Debug.Log($"{other.gameObject.name} is not in the required list.");
        }

        bool allReady = true;
        foreach (KeyValuePair<string, int> entry in requiredItems)
        {
            if(requiredItems[entry.Key] > inPlaceItems[entry.Key]) allReady = false;
            Debug.Log($"Item: {entry.Key}, Count: {entry.Value}");
        }

        if (allReady){
            Debug.Log($"Complete");
            sceneManager.CompleteTask("toolsToCabinet");
        }
        
        
        UnfoldSterileDrape();
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Triggered by: " + other.gameObject.name);
        GeneralItem item = other.GetComponent<GeneralItem>();
        
        if (item == null) {
            return;
        }

        if(!item.isClean){
            //Task.CreateGeneralMistake(Translator.Translate("XR MembraneFilteration 2.0", "FloorContaminedInCabinet"), 1);
            Debug.Log("Dirty: " + other.gameObject.name);
            return;
        }

        if (requiredItems.ContainsKey(other.gameObject.name)){
            inPlaceItems[other.gameObject.name]--;  // Increase count if the item exists            
            Debug.Log($"Removed {other.gameObject.name}. New count: {inPlaceItems[other.gameObject.name]}");
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
