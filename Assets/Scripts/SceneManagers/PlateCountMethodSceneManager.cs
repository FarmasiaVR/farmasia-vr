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
    // Dict that stores information about dilution and control tubes
    private Dictionary<string, List<LiquidContainer>> testTubes = new Dictionary<string, List<LiquidContainer>>();
    private Dictionary<WritingType, LiquidContainer[]> dilutionDict;
    private HashSet<WritingType> dilutionTypes = new HashSet<WritingType>
        {
            WritingType.OneToTen,
            WritingType.OneToHundred,
            WritingType.OneToThousand,
            WritingType.Control
        };

    public static Dictionary<LiquidType, LiquidType> mixingTable = new()
    {
        { LiquidType.Senna1m, LiquidType.Senna1 },
        { LiquidType.Senna01m, LiquidType.Senna01 },
        { LiquidType.Senna001m, LiquidType.Senna001 },
        { LiquidType.Senna0001m, LiquidType.Senna0001 }
    };

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();

        // Prepare dictionaries
        dilutionDict = new Dictionary<WritingType, LiquidContainer[]>();
        testTubes.Add("dilution", new List<LiquidContainer>());
        testTubes.Add("control", new List<LiquidContainer>());

        foreach (WritingType type in dilutionTypes)
        {
            dilutionDict[type] = new LiquidContainer[4];
        }
        Debug.Log(dilutionDict);
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
        var localizedString = new LocalizedString("PlateCountMethod", "OrderViolated");
        localizedString.StringChanged += (localizedText) => {
            GeneralMistake(localizedText, 1);
        };
        taskOrderViolated = true;
    }

    public void CheckTaskOrderViolation(string taskName)
    {
        string currentTask = taskManager.GetCurrentTask().name;
        if (currentTask != taskName && !taskOrderViolated) ViolateTaskOrder();
    }

    public void GeneralMistake(string message, int penalty)
    {
        taskManager.GenerateGeneralMistake(message, penalty);
    }

    public void TaskMistake(string message, int penalty)
    {
        taskManager.GenerateTaskMistake(message, penalty);
    }

    public void SkipCurrentTask()
    {
        string currentTask = taskManager.GetCurrentTask().name;

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

    public void CheckTubesFill(LiquidContainer container)
    {
        if (taskManager.IsTaskCompleted("FillTubes")) { return; }

        //Debug.Log("Checking order violation for FillTubes");
        CheckTaskOrderViolation("FillTubes");

        switch(container.Amount)
        {
            case controlTubeAmount:
                if (!testTubes["control"].Any())
                {
                    Debug.Log("Container added to CONTROL");
                    testTubes["control"].Add(container);
                }
                break;
            case dilutionTubesAmount:
                if (testTubes["dilution"].Count < 3)
                {
                    testTubes["dilution"].Add(container);
                    Debug.Log("Container added to DILTUION");
                    break;
                }
                break;
            // If amount is changed, container needs to be removed from lists
            default:
                if (testTubes["control"].Contains(container))
                {
                    testTubes["control"].Remove(container);
                    Debug.Log("Container removed from CONTROL");
                    break;
                }
                else if (testTubes["dilution"].Contains(container))
                {
                    testTubes["dilution"].Remove(container);
                    Debug.Log("Container removed from DILUTION");
                    break;
                }
                break;
        }

        // Check if tubes are filled
        if (testTubes["dilution"].Count == 3 && testTubes["control"].Count == 1)
        {
            CompleteTask("FillTubes");
            Debug.Log("All the tubes are filled");
        }
    }

    // Checks if the dilution type was written onto the object and updates the according dictionaries
    public void SubmitWriting(GeneralItem foundItem, Dictionary<WritingType, string> selectedOptions)
    {
        if (taskManager.IsTaskCompleted("WriteOnTubes")) { return; }

        CheckTaskOrderViolation("WriteOnTubes");
        //Debug.Log("Checking order violation for WriteOnTubes");

        WritingType? dilutionType = selectedOptions.Keys.FirstOrDefault(key => dilutionTypes.Contains(key));
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
        return testTubes["control"].Contains(container) || dilutionDict[WritingType.Control][0] == container;
    }

    public void MixingComplete(LiquidContainer container)
    {
        if (taskManager.IsTaskCompleted("MixPhosphateToSenna"))
        {
            //Debug.Log("Checking order violation for PerformSerialDilution");
            CheckTaskOrderViolation("PerformSerialDilution");
        }

        switch(container.LiquidType)
        {
            case LiquidType.Senna1m:
            {
                //Debug.Log("Checking order violation for MixPhosphateToSenna");
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
                    GeneralMistake("Used a contaminated pipette", 1);
                }
            }
        }
        else
        {
            // Check if just mixed senna, so that a mistake isn't given right after completing MixPhosphateToSenna
            bool finishedMixingSenna = pipette.Container.LiquidType == LiquidType.Senna1m && container.LiquidType == LiquidType.Senna1;
            if (usedPipetteHeads.Contains(pipetteID) && pipette.Container.LiquidType != container.LiquidType && !finishedMixingSenna)
            {
                TaskMistake("Used a contaminated pipette", 1);
            }
        }
    }
}
