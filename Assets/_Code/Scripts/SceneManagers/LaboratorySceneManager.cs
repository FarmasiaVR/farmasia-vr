using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LaboratorySceneManager : MonoBehaviour {
    public enum LaboratorySide {Left, Right}; // Which side of the lab the item is in (from the main entrance) 
    private static Dictionary<LaboratorySide, bool> itemsFound = new Dictionary<LaboratorySide, bool>{
        {LaboratorySide.Left, false}, {LaboratorySide.Right, false}
    };
    private Dictionary<LaboratorySide, bool> blanketsFound = new Dictionary<LaboratorySide, bool>(itemsFound);
    private Dictionary<LaboratorySide, bool> eyeShowersFound = new Dictionary<LaboratorySide, bool>(itemsFound);
    private int emergencyShowersFound = 0, extinguishersFound = 0, exitsFound = 0;

    private TaskManager taskManager;
    private TaskboardManager taskboardManager;

    private void Awake() {
        taskManager = GetComponent<TaskManager>();
        taskboardManager = GetComponent<TaskboardManager>();
    }

    public void CompleteTask(string taskName) {
        taskManager.CompleteTask(taskName);
        taskboardManager.MarkTaskAsCompleted(taskName);
    }

    // Sets found items and returns whether an item has been found on both sides of the lab or not
    private bool ItemFoundOnBothSides(Dictionary<LaboratorySide, bool> itemsFound, string labSide) {
        LaboratorySide result;
        if (Enum.TryParse<LaboratorySide>(labSide, out result)) {
            itemsFound[result] = true;
        } else {
            throw new Exception("Wrong laboratory side. Please check the LaboratorySide enum in LaboratorySceneManager script.");
        }
        return blanketsFound[LaboratorySide.Left] && blanketsFound[LaboratorySide.Right];
    }

    public void FindBlanket(string labSide) {
        if (ItemFoundOnBothSides(blanketsFound, labSide))
            CompleteTask("FireBlanket");
    }

    public void FindEyeShower(string labSide) {
        if (ItemFoundOnBothSides(eyeShowersFound, labSide))
            CompleteTask("EyeShower");
    }

    private bool NItemsFound(ref int foundCount, int n) {
        if (foundCount < n) {
            foundCount += 1;
        }
        return foundCount >= n;
    }

    public void FindEmergencyShower() {
        if (NItemsFound(ref emergencyShowersFound, 2))
            CompleteTask("EmergencyShower");
    }

    public void FindExtinguisher() {
        if (NItemsFound(ref extinguishersFound, 2)) 
            CompleteTask("Extinguisher");
    }

    public void FindEmergencyExit() {
        if (NItemsFound(ref exitsFound, 2)) 
            CompleteTask("EmergencyExit");
    }
}