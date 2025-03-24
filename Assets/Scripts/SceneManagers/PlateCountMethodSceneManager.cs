using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class PlateCountMethodSceneManager : MonoBehaviour
{
    private TaskManager taskManager;

    public GameObject skips; // Assigned in the inspector

    public UnityEvent onMixingComplete;

    private bool taskOrderViolated = false;

    private HashSet<int> usedPipetteHeads = new HashSet<int>();

    private const int dilutionTubesAmount = 4500;
    private const int controlTubeAmount = 1000;

    // Dict that stores information about dilution, tubes and plates
    private Dictionary<WritingType, LiquidContainer[]> dilutionDict;
    private HashSet<WritingType> dilutionTypes = new HashSet<WritingType>
        {
            WritingType.OneToTen,
            WritingType.OneToHundred,
            WritingType.OneToThousand,
            WritingType.Control
        };

    // If player starts filling tubes before writing on them, they are added to this list instead of dilutionDict
    private List<LiquidContainer> containerBuffer = new List<LiquidContainer>();

    public static Dictionary<LiquidType, LiquidType> mixingTable = new()
    {
        { LiquidType.Senna1m, LiquidType.Senna1 },
        { LiquidType.Senna01m, LiquidType.Senna01 },
        { LiquidType.Senna001m, LiquidType.Senna001 },
        { LiquidType.Senna0001m, LiquidType.Senna0001 }
    };

    private Dictionary<LiquidType, WritingType> correctLiquids = new()
    {
        { LiquidType.Senna01, WritingType.OneToTen},
        { LiquidType.Senna001, WritingType.OneToHundred },
        { LiquidType.Senna0001, WritingType.OneToThousand },
        { LiquidType.PhosphateBuffer, WritingType.Control }
    };

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();

        // Prepare dictionary
        dilutionDict = new Dictionary<WritingType, LiquidContainer[]>();

        foreach (WritingType type in dilutionTypes)
        {
            dilutionDict[type] = new LiquidContainer[6];
        }
    }

    public void CompleteTask(string taskName)
    {
        taskManager.CompleteTask(taskName);
        taskOrderViolated = false;
    }

    public void CleanHands()
    {
        CompleteTask("WashHands");
    }

    public void ViolateTaskOrder()
    {
        GeneralMistake("OrderViolated", 1);
        taskOrderViolated = true;
    }

    public void CheckTaskOrderViolation(string taskName)
    {
        string currentTask = taskManager.GetCurrentTask().key;
        if (currentTask != taskName && !taskOrderViolated)
        {
            Debug.Log(taskName + " != " + currentTask);
            ViolateTaskOrder();
        }
    }

    public void GeneralMistake(string key, int penalty)
    {
        var localizedString = new LocalizedString("PlateCountMethod", key);
        localizedString.StringChanged += (localizedText) => {
            taskManager.GenerateGeneralMistake(localizedText, penalty);
        };
        
    }

    public void TaskMistake(string key, int penalty)
    {
        var localizedString = new LocalizedString("PlateCountMethod", key);
        localizedString.StringChanged += (localizedText) => {
            taskManager.GenerateTaskMistake(localizedText, penalty);
        };
    }

    public void SkipCurrentTask()
    {
        string currentTask = taskManager.GetCurrentTask().key;

        switch (currentTask)
        {
            case "toolsToCabinet":
                Transform toolsToCabinetGO = skips.transform.Find("ToolsToCabinet");
                toolsToCabinetGO.gameObject.SetActive(true);
                break;
            case "FillTubes":
                Transform emptyTubesStand = skips.transform.Find("ToolsToCabinet/BigTestTubeStandPCM (1)");
                emptyTubesStand.gameObject.SetActive(false);

                GameObject[] allObjects = FindObjectsOfType<GameObject>();
                int tubeLayer = LayerMask.NameToLayer("TestTube");
                foreach (GameObject obj in allObjects)
                {
                    if (obj.layer == tubeLayer)
                    {
                        Destroy(obj);
                    }
                }

                Transform fullTubes = skips.transform.Find("FillTubes");
                fullTubes.gameObject.SetActive(true);
                break;
        }
        CompleteTask(currentTask);
    }

    // If container is not found, it means the player messed writing and they are added to naughty list >:)
    private WritingType? FindSlotForContainer(LiquidContainer container)
    {
        foreach (var entry in dilutionDict)
        {
            if (entry.Value[0] == container) {
                return entry.Key;
            }
        }
        return null;
    }

    // This can be called by another object to mark a plate ready
    public void PlateReadyInSpreadTask(LiquidContainer container)
    {
        foreach (var entry in dilutionDict)
        {
            if (entry.Value[2] == container)
            {
                // Container found in soy caseins
                dilutionDict[entry.Key][4] = container;
                break;
            }
            else if (entry.Value[3] == container)
            {
                // Container found in sabourauds
                dilutionDict[entry.Key][5] = container;
                break;
            }
        }
        // Check if slots are filled after adding
        foreach (var entry in dilutionDict)
        {
            if (entry.Value[4] == null || entry.Value[5] == null) return;
        }
        CompleteTask("SpreadDilution");
    }

    public void PourDilutionOnPlate(LiquidContainer container)
    {
        CheckTaskOrderViolation("SpreadDilution");

        LiquidType liquid = container.LiquidType;
        WritingType desiredMarking = correctLiquids[liquid];

        if (dilutionDict[desiredMarking][2] == container
        || dilutionDict[desiredMarking][3] == container)
        {
            // if this check passes, player put liquid in a correct plate
            Debug.Log(liquid + " put into " + desiredMarking + " successfully");
        }
        else
        {
            TaskMistake("WrongDilutionType", 1);
            // Allows to refill if liquid was incorrect
            container.SetAmount(0);
            container.LiquidType = LiquidType.None;
            container.contaminationLiquidType = LiquidType.None;
        }
    }

    public void CheckTubesFill(LiquidContainer container)
    {
        if (taskManager.IsTaskCompleted("FillTubes")) { return; }

        //Debug.Log("Checking order violation for FillTubes");
        CheckTaskOrderViolation("FillTubes");

        switch(container.Amount)
        {
            case controlTubeAmount:
                if (dilutionDict[WritingType.Control][1] is null)
                {
                    Debug.Log("Container added to CONTROL");
                    dilutionDict[WritingType.Control][1] = container;
                }
                break;
            case dilutionTubesAmount:
                WritingType? writingType = FindSlotForContainer(container);
                if (writingType is null)
                {
                    // Will not complain if write on tubes has been skipped manually
                    if (!taskManager.IsTaskCompleted("WriteOnTubes"))
                    {
                        GeneralMistake("WriteBeforeFill", 1);
                    }
                    
                    containerBuffer.Add(container);
                    break;
                }
                dilutionDict[writingType.Value][1] = container;
                Debug.Log("Container added to " + writingType.Value);
                break;

            // If amount is changed, container needs to be removed from arrays
            default:
                if (containerBuffer.Contains(container))
                {
                    containerBuffer.Remove(container);
                    break;
                }
                foreach (var entry in dilutionDict)
                {
                    if (entry.Value[1] == container)
                    {
                        entry.Value[1] = null;
                        Debug.Log("Container removed from " + entry.Key);
                        break;
                    }
                }
                break;
        }
        CheckIfTubesAreFilled();
    }

    private void CheckIfTubesAreFilled()
    {
        foreach (var entry in dilutionDict)
        {
            if (entry.Value[1] == null) { return; }
        }
        CompleteTask("FillTubes");
        Debug.Log("All the tubes are filled");
    }

    private WritingType? DilutionTypeFromWriting(Dictionary<WritingType, string> selectedOptions)
    {
        foreach (var entry in selectedOptions)
        {
            if (dilutionTypes.Contains(entry.Key)) return entry.Key;
        }
        return null;
    }

    // Checks if the dilution type was written onto the object and updates the dictionary
    public void SubmitWriting(GeneralItem foundItem, Dictionary<WritingType, string> selectedOptions)
    {
        if (taskManager.IsTaskCompleted("WriteOnTubes")) { return; }

        CheckTaskOrderViolation("WriteOnTubes");

        WritingType? dilutionType = DilutionTypeFromWriting(selectedOptions);
        Debug.Log("Dilution Type: " + dilutionType);

        if (dilutionType == null) return;

        Debug.Log(foundItem.GetType().Name);
        switch(foundItem.GetType().Name)
        {
            case "Bottle":
            {
                LiquidContainer container = foundItem.gameObject.GetComponentInChildren<LiquidContainer>();
                Debug.Log("Writing to a tube: " + dilutionType.Value + " value: " + container);

                dilutionDict[dilutionType.Value][0] = container;
                if (containerBuffer.Contains(container))
                {
                    containerBuffer.Remove(container);
                    dilutionDict[dilutionType.Value][1] = container;
                    CheckIfTubesAreFilled();
                }
                break;
            }
            case "AgarPlateLid":
            {
                AgarPlateLid lid = foundItem.GetComponent<AgarPlateLid>();
                LiquidContainer container = lid.PlateBottom.GetComponentInChildren<LiquidContainer>();
                Debug.Log("Writing to a plate: " + dilutionType.Value + " value: " + container);

                if (lid.Variant == "Sabourad-dekstrosi")
                {
                    dilutionDict[dilutionType.Value][3] = container;
                }
                else if (lid.Variant == "Soija-kaseiini")
                {
                    dilutionDict[dilutionType.Value][2] = container;
                }
                break;
            }
            default:
            {
                return;
            }
        }

        CheckWritingsIntegrity();
    }

    private void CheckWritingsIntegrity()
    {
        if (IsAnySlotEmpty(dilutionDict))
        {
            return;
        }
        Debug.Log("Yay, you wrote on all tubes");
        CompleteTask("WriteOnTubes");
    }

    private bool IsAnySlotEmpty(Dictionary<WritingType, LiquidContainer[]> dict)
    {
        foreach (var entry in dict)
        {
            for (int i = 0; i<4; i++)
            {
                // Index 1 is reserved for phosphate buffer fill
                if (i == 1) continue;

                if (entry.Value[i] == null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsControlTube(LiquidContainer container)
    {
        return dilutionDict[WritingType.Control][1] == container || dilutionDict[WritingType.Control][0] == container;
    }

    public void MixingComplete(LiquidContainer container)
    {
        // Stop doing it if serial dilution is completed
        if (taskManager.IsTaskCompleted("PerformSerialDilution")) return;

        if (taskManager.IsTaskCompleted("MixPhosphateToSenna"))
        {
            CheckTaskOrderViolation("PerformSerialDilution");
        }

        switch(container.LiquidType)
        {
            case LiquidType.Senna1m:
            {
                CheckTaskOrderViolation("MixPhosphateToSenna");
                if (MixIfValid(container, 6000))
                {
                    CompleteTask("MixPhosphateToSenna");
                }    
                return;
            }
            case LiquidType.Senna01m:
            {
                if (MixIfValid(container, 5000))
                {
                    Debug.Log("Mixing complete in " + container.LiquidType);
                    onMixingComplete.Invoke();
                }
                break;
            }
            case LiquidType.Senna001m:
            {
                if (MixIfValid(container, 5000))
                {
                    Debug.Log("Mixing complete in " + container.LiquidType);
                    onMixingComplete.Invoke();
                }
                break;
            }
            case LiquidType.Senna0001m:
            {
                if (MixIfValid(container, 5000))
                {
                    CompleteTask("PerformSerialDilution");
                    onMixingComplete.Invoke();
                }
                break;
            }
        }
    }

    private bool MixIfValid(LiquidContainer container, int amount)
    {
        bool valid = mixingTable.TryGetValue(container.LiquidType, out LiquidType newResult) && container.Amount == amount;
        if (valid)
        {
            container.LiquidType = newResult;
            container.SetLiquidMaterial();
        }
        return valid;
    }

    // Invoked when pipettor pipette head enters a liquid container
    public void PipetteUsed(PipetteContainer pipette, LiquidContainer container)
    {
        string task = taskManager.GetCurrentTask().key;
        int pipetteID = pipette.transform.GetInstanceID();

        // Debug.Log("Pipette used in task " + task);
        if (!taskManager.IsTaskCompleted("MixPhosphateToSenna"))
        {
            // Debug.Log("Pipette ID: " + pipette);
            usedPipetteHeads.Add(pipetteID);

            // Check if a contaminated pipette is used in the wrong container (e.g. trying to take phosphate with a pipette contaminated with senna)
            if (pipette.Container.contaminationLiquidType != LiquidType.None)
            {
                if (pipette.Container.contaminationLiquidType != container.LiquidType && pipette.Container.contaminationLiquidType != LiquidType.PhosphateBuffer)
                {
                    GeneralMistake("ContaminatedPipette", 1);
                }
            }
        }
        else
        {
            // Check if just mixed senna, so that a mistake isn't given right after completing MixPhosphateToSenna
            bool finishedMixingSenna = pipette.Container.LiquidType == LiquidType.Senna1m && container.LiquidType == LiquidType.Senna1;
            if (usedPipetteHeads.Contains(pipetteID) && pipette.Container.LiquidType != container.LiquidType && !finishedMixingSenna)
            {
                TaskMistake("ContaminatedPipette", 1);
            }
        }
    }
}
