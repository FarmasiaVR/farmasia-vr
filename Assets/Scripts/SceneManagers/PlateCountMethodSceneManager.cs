using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using TMPro;
using System.Text;
using UnityEngine.Localization.Settings;

public class PlateCountMethodSceneManager : MonoBehaviour
{
    private TaskManager taskManager;
    public GameObject scoreboardPanel;

    public UnityEvent onMixingComplete;
    public UnityEvent<string> onSkipTask;
    public UnityEvent<string> notifyPlayer;
    public UnityEvent onLastTaskActive;
    public CameraFadeController fadeController;
    public Transform teleportDestination;// for teleporting to lab
    public GameObject player;
    
    private bool taskOrderViolated = false;

    private HashSet<int> usedPipetteHeads = new HashSet<int>();
    public HashSet<GameObject> objectsInLaminarCabinet = new HashSet<GameObject>(); // Items are added to this set in CabinetBasePCM.cs

    private const int dilutionTubesAmount = 4500;
    private const int controlTubeAmount = 1000;

    private int coloniesCounted = 0; // For completing colony count

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

    // Index consts for better readability in anything regarding dilutionDict
    private const int writeTube = 0;
    private const int fillTube = 1;
    private const int writeSoy = 2;
    private const int writeSab = 3;
    private const int spreadSoy = 4;
    private const int spreadSab = 5;

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
        UpdateCanvas();
    }

    public void CleanHands()
    {
        CompleteTask("WashHands");
    }

    public void ViolateTaskOrder()
    {
        GeneralMistake("OrderViolated", 5);
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

    // To use next three functions: 
    // 1. Add item with key to localization table 
    // 2. Call function using key and desired penalty 
    // ex. GeneralMistake("KeyForTesting", 2)
    public void GeneralMistake(string key, int penalty)
    {
        var localizedString = new LocalizedString("PlateCountMethod", key);
        LocalizedString.ChangeHandler localizedCallback = null;
        localizedCallback += (localizedText) => {
            taskManager.GenerateGeneralMistake(localizedText, penalty);
            localizedString.StringChanged -= localizedCallback;
        };
     

        localizedString.StringChanged += localizedCallback;
    }

    public void TaskMistake(string key, int penalty)
    {
        var localizedString = new LocalizedString("PlateCountMethod", key);
        LocalizedString.ChangeHandler localizedCallback = null;

        localizedCallback = (localizedText) => {
            taskManager.GenerateTaskMistake(localizedText, penalty);
            localizedString.StringChanged -= localizedCallback;
        };
        localizedString.StringChanged += localizedCallback;
    }

    public void NotifyPlayer(string key)
    {
        var localizedString = new LocalizedString("PlateCountMethod", key);
        LocalizedString.ChangeHandler localizedCallback = null;

        localizedCallback = (localizedText) => {
            notifyPlayer.Invoke(localizedText);
            localizedString.StringChanged -= localizedCallback;
        };
        localizedString.StringChanged += localizedCallback;
    }
    
    public void SkipCurrentTask()
    {
        string currentTask = taskManager.GetCurrentTask().key;
        onSkipTask.Invoke(currentTask);
        CompleteTask(currentTask);
    }

    // Checks if any index in dilutionDict already has this container
    private WritingType? FindSlotForContainer(LiquidContainer container, int index)
    {
        foreach (var entry in dilutionDict)
        {
            if (entry.Value[index] == container) {
                return entry.Key;
            }
        }
        return null;
    }

    private bool WritingTypeAlreadyInUse(WritingType writingType, int index)
    {
        return dilutionDict[writingType][index] != null;
    }

    public void PrepareForVentilationTask(){
        GameObject targetObject = GameObject.Find("TimerClock");
        
        if (targetObject != null)
        {            
            Logger.Print("Found object");
            Transform display = targetObject.transform.Find("Display");
            display.gameObject.SetActive(true);
        }
    }

    public void CompleteVentilationTask(){
        CompleteTask("VentilatingAgarPlates");
    }

    public void CompleteIncubationTask(bool boxesReady)
    {
        string task = taskManager.GetCurrentTask().key;
        if (task != "IncubatePlates") return;

        if (boxesReady)
        {
            CompleteTask("IncubatePlates");
            onLastTaskActive.Invoke();
            StartCoroutine(MoveToLab());
        }
        else
        {
            NotifyPlayer("IncubateBoxesNotReady");
        }
    }

    public void CompleteColonyCountTask()
    {
        CheckTaskOrderViolation("ColonyCount");
        coloniesCounted++;
        NotifyPlayer("AllColoniesFound");
        if (coloniesCounted == 2)
        {
            CompleteTask("ColonyCount");
            UpdateCanvas();
        }
    }

    private IEnumerator MoveToLab()
    {
        fadeController.BeginFadeOut();
        NotifyPlayer("AFewMomentsLater");
        yield return new WaitForSeconds(3f);
        player.transform.position = teleportDestination.position;
        fadeController.BeginFadeIn();
    }

    // This can be called by another object to mark a plate ready
    public void PlateReadyInSpreadTask(LiquidContainer container)
    {
        NotifyPlayer("SpreadDone");
        foreach (var entry in dilutionDict)
        {
            if (entry.Value[writeSoy] == container)
            {
                // Container found in soy caseins
                dilutionDict[entry.Key][spreadSoy] = container;
                break;
            }
            else if (entry.Value[writeSab] == container)
            {
                // Container found in sabourauds
                dilutionDict[entry.Key][spreadSab] = container;
                break;
            }
        }
        // Check if slots are filled after adding
        foreach (var entry in dilutionDict)
        {
            if (entry.Value[spreadSoy] == null || entry.Value[spreadSab] == null) return;
        }
        CompleteTask("SpreadDilution");
        PrepareForVentilationTask();
    }

    public void PourDilutionOnPlate(LiquidContainer container)
    {
        CheckTaskOrderViolation("SpreadDilution");

        LiquidType liquid = container.LiquidType;
        if (correctLiquids.ContainsKey(liquid))
        {
            WritingType desiredMarking = correctLiquids[liquid];
            if (dilutionDict[desiredMarking][writeSoy] == container
            || dilutionDict[desiredMarking][writeSab] == container)
            {
                // if this check passes, player put liquid in a correct plate
                Debug.Log(liquid + " put into " + desiredMarking + " successfully");
                return;
            }
        }

        GeneralMistake("WrongDilutionType", 5);
        // Allows to refill if liquid was incorrect
        container.SetAmount(0);
        container.LiquidType = LiquidType.None;
        container.contaminationLiquidType = LiquidType.None;
    }

    public void CheckTubesFill(LiquidContainer container)
    {
        if (taskManager.IsTaskCompleted("FillTubes")) { return; }

        //Debug.Log("Checking order violation for FillTubes");
        CheckTaskOrderViolation("FillTubes");

        switch(container.Amount)
        {
            case controlTubeAmount:
                if (dilutionDict[WritingType.Control][fillTube] is null)
                {
                    // Debug.Log("Container added to CONTROL");
                    dilutionDict[WritingType.Control][fillTube] = container;
                }
                break;
            case dilutionTubesAmount:
                WritingType? writingType = FindSlotForContainer(container, writeTube);
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
                dilutionDict[writingType.Value][fillTube] = container;
                // Debug.Log("Container added to " + writingType.Value);
                break;

            // If amount is changed, container needs to be removed from arrays
            default:
                if (containerBuffer.Contains(container))
                {
                    containerBuffer.Remove(container);
                    break;
                }
                DeleteObjectFromDictIfPresent(container, fillTube);
                break;
        }
        CheckIfTubesAreFilled();
    }

    private void CheckIfTubesAreFilled()
    {
        foreach (var entry in dilutionDict)
        {
            if (entry.Value[fillTube] == null) { return; }
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
        // Debug.Log("Dilution Type: " + dilutionType);

        string itemName = foundItem.GetType().Name;
        int index = 0;
        LiquidContainer container;

        switch(itemName)
        {
            case "Bottle":
            {
                container = foundItem.gameObject.GetComponentInChildren<LiquidContainer>();
                index = writeTube;
                break;
            }
            case "AgarPlateLid":
            {
                AgarPlateLid lid = foundItem.GetComponent<AgarPlateLid>();
                container = lid.PlateBottom.GetComponentInChildren<LiquidContainer>();
                if (lid.Variant == "Sabourad-dekstrosi") index = writeSab;
                else if (lid.Variant == "Soija-kaseiini") index = writeSoy;
                break;
            }
            default:
            {
                return;
            }
        }

        // If player deletes the line, also deletes object from the dictionary
        if (dilutionType == null) 
        {
            WritingType? oldDilution = FindSlotForContainer(container, index);
            if (oldDilution != null)
            {
                dilutionDict[oldDilution.Value][index] = null;
                Debug.Log("After erasing " + oldDilution.Value + ", deleted from index " + index);
            }
            return;
        }

        WritingType dilution = dilutionType.Value;

        // If another object already has this writing, notify the player and return, also be evil and delete written line
        if (WritingTypeAlreadyInUse(dilution, index))
        {
            if (dilutionDict[dilution][index] == container) return; // Player wrote the same dilution type on the same bottle

            NotifyPlayer("ExistingDilution");

            Writable writable = foundItem.GetComponent<Writable>();
            writable.removeLine(dilution);
            return;
        }

        DeleteObjectFromDictIfPresent(container, index);
        dilutionDict[dilution][index] = container;
        if (itemName == "Bottle" && containerBuffer.Contains(container))
        {
            containerBuffer.Remove(container);
            dilutionDict[dilution][fillTube] = container;
            CheckIfTubesAreFilled();
        }
        // Debug.Log("Added dilution: " + dilutionType.Value + " to: " + container);

        CheckWritingsIntegrity();
    }

    // If object is changed, deletes it from the old index. Used in writing and filling
    private void DeleteObjectFromDictIfPresent(LiquidContainer container, int index)
    {
        WritingType? writingType = FindSlotForContainer(container, index);
        if (writingType == null) { return; } // Object not found

        // Object is found
        dilutionDict[writingType.Value][index] = null;
        Debug.Log("Deleted container from " + writingType.Value + " at index " + index);
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
            for (int i = 0; i<spreadSoy; i++)
            {
                // Index 1 is reserved for phosphate buffer fill
                if (i == fillTube) continue;

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
        return dilutionDict[WritingType.Control][fillTube] == container || dilutionDict[WritingType.Control][writeTube] == container;
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
                    return;
                }    
                break;
            }
            case LiquidType.Senna01m or LiquidType.Senna001m:
            {
                if (MixIfValid(container, 5000))
                {
                    // Debug.Log("Mixing complete in " + container.LiquidType);
                    onMixingComplete.Invoke();
                    return;
                }
                break;
            }
            case LiquidType.Senna0001m:
            {
                if (MixIfValid(container, 5000))
                {
                    CompleteTask("PerformSerialDilution");
                    onMixingComplete.Invoke();
                    return;
                }
                break;
            }
        }

        if (taskManager.GetCurrentTask().key == "PerformSerialDilution" || taskManager.GetCurrentTask().key == "MixPhosphateToSenna")
        {
            NotifyPlayer("FailedMixing");
        }
    }

    private bool MixIfValid(LiquidContainer container, int amount)
    {
        bool valid = mixingTable.TryGetValue(container.LiquidType, out LiquidType newResult) && container.Amount == amount;
        if (valid)
        {
            container.mixingComplete = true;
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
                    GeneralMistake("ContaminatedPipette", 5);
                }
            }
        }
        else
        {
            // Check if just mixed senna, so that a mistake isn't given right after completing MixPhosphateToSenna
            bool finishedMixingSenna = pipette.Container.LiquidType == LiquidType.Senna1m && container.LiquidType == LiquidType.Senna1;
            if (usedPipetteHeads.Contains(pipetteID) && pipette.Container.LiquidType != container.LiquidType && !finishedMixingSenna)
            {
                GeneralMistake("ContaminatedPipette", 5);
            }
        }
    }
    public void UpdateCanvas()
    {
        // Find all TextMeshProUGUI elements under the scoreboard panel
        var texts = scoreboardPanel.GetComponentsInChildren<TextMeshProUGUI>(true);
        List<Mistake> generalMistakes = taskManager.taskListObject.GetGeneralMistakes();
        Dictionary<string, int> mistakeCounts = new Dictionary<string, int>();

        foreach (Mistake mistake in generalMistakes)
            {
                if (mistakeCounts.ContainsKey(mistake.mistakeText))
                    mistakeCounts[mistake.mistakeText]++;
                else
                    mistakeCounts[mistake.mistakeText] = 1;
            }


        foreach (var tmp in texts)
            {
            switch (tmp.name)
                {
                    case "ScoreText":
                        tmp.text = $"{taskManager.taskListObject.GetPoints()}/100 points ";
                        break;

                    // case "MainMessageText":
                    //     tmp.text = "congratulation!";
                    //     break;

                    // case "SubMessageText":
                    //     tmp.text = "You successfully completed the Plate Count Method simulation.";
                    //     break;

                    case "MistakesText":
                        StringBuilder sb = new StringBuilder();                        

                        foreach (var entry in mistakeCounts)
                        {
                            sb.AppendLine($"- {entry.Key}(x{entry.Value})");
                        }

                        
                        tmp.text = sb.ToString();
                        break;                    
                }
        }
    }
}
