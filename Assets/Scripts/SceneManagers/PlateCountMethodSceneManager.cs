using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlateCountMethodSceneManager : MonoBehaviour
{
    private TaskManager taskManager;

    public UnityEvent onMixingComplete;

    private const int dilutionTubesAmount = 4500;
    private const int controlTubeAmount = 1000;
    // Dict that stores information about dilution and control tubes
    private Dictionary<string, List<LiquidContainer>> testTubes = new Dictionary<string, List<LiquidContainer>>();
    private Dictionary<WritingType, LiquidContainer> dilutionTypesTubes = new Dictionary<WritingType, LiquidContainer>();
    private HashSet<WritingType> dilutionTypes = new HashSet<WritingType>
        {
            WritingType.OneToTen,
            WritingType.OneToHundred,
            WritingType.OneToThousand,
            WritingType.Control
        };

    public static Dictionary<Tuple<LiquidType, LiquidType>, LiquidType> recipes = new()
    {
        { Tuple.Create(LiquidType.SennaPowder,LiquidType.PhosphateBuffer), LiquidType.Senna1m },
        { Tuple.Create(LiquidType.Senna1m,LiquidType.PhosphateBuffer), LiquidType.Senna1m },
        { Tuple.Create(LiquidType.PhosphateBuffer,LiquidType.Senna1), LiquidType.Senna01m },
        { Tuple.Create(LiquidType.Senna01m,LiquidType.Senna1), LiquidType.Senna01m },                
        { Tuple.Create(LiquidType.PhosphateBuffer, LiquidType.Senna01), LiquidType.Senna001m },
        { Tuple.Create(LiquidType.Senna001m, LiquidType.Senna01), LiquidType.Senna001m },
        { Tuple.Create(LiquidType.PhosphateBuffer, LiquidType.Senna001), LiquidType.Senna0001m },
        { Tuple.Create(LiquidType.Senna0001m, LiquidType.Senna001), LiquidType.Senna0001m }        
    };


    public static Dictionary<LiquidType, Tuple<int, int>> minMaxMixingValue = new()
    {
        { LiquidType.SennaPowder, Tuple.Create(1000, 1500) },
        { LiquidType.Senna1m, Tuple.Create(500, 6000) },
        { LiquidType.Senna1, Tuple.Create(500, 6000) },
        { LiquidType.PhosphateBuffer, Tuple.Create(4500, 5000) },
        { LiquidType.Senna01m, Tuple.Create(500, 5000) },
        { LiquidType.Senna001m, Tuple.Create(4500, 5000) },
        { LiquidType.Senna0001m, Tuple.Create(4500, 5000) }
    };

    public static Dictionary<LiquidType, LiquidType> mixingTable = new()
    {
        { LiquidType.Senna1m, LiquidType.Senna1 },
        { LiquidType.Senna01m, LiquidType.Senna01 },
        { LiquidType.Senna001m, LiquidType.Senna001 },
        { LiquidType.Senna0001m, LiquidType.Senna0001 }
    };

    public static List<LiquidType> SennaTypes = new()
    {
        LiquidType.SennaPowder,
        LiquidType.Senna1m,
        LiquidType.Senna1,
        LiquidType.Senna01m,
        LiquidType.Senna01,
        LiquidType.Senna001m,
        LiquidType.Senna001,
        LiquidType.Senna0001m,
        LiquidType.Senna0001
    };

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
        // Prepare dictionaries
        testTubes.Add("dilution", new List<LiquidContainer>());
        testTubes.Add("control", new List<LiquidContainer>());

        foreach (WritingType type in dilutionTypes)
        {
            dilutionTypesTubes[type] = null;
        }
    }

    public void CompleteTask(string taskName)
    {
        // Debug.Log($"Trying to complete task"); // Please god no this spams so much
        taskManager.CompleteTask(taskName);
    }

    public void CleanHands()
    {
        CompleteTask("WashHands");
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
        CompleteTask(currentTask);
    }

    public void CheckTubesFill(LiquidContainer container)
    {
        if (taskManager.IsTaskCompleted("FillTubes")) { return; }

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

    public void SubmitWriting(GeneralItem foundItem, Dictionary<WritingType, string> selectedOptions)
    {
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
                dilutionTypesTubes[dilutionType.Value] = container;
                break;
            }
            default:
            {
                break;
            }
        }

        CheckWritingsIntegrity();
    }

    private void CheckWritingsIntegrity()
    {
        foreach (KeyValuePair<WritingType, LiquidContainer> entry in dilutionTypesTubes)
        {
            Debug.Log(entry.Key + ": " + entry.Value);
            if (entry.Value == null)
                return;
        }
        Debug.Log("Yay, you wrote on all tubes");
        CompleteTask("WriteOnTubes");
    }

    private bool IsControlTube(LiquidContainer container)
    {
        return testTubes["control"].Contains(container) || dilutionTypesTubes[WritingType.Control] == container;
    }

    public bool Dilution(LiquidContainer source, LiquidContainer target, int transferAmount){        
        Debug.Log("Trying to mix: " + source.LiquidType + " with " + target.LiquidType);

        if (IsControlTube(target))
        {
            GeneralMistake("Don't mix liquids in the control tube!", 1);
            return false;
        }

        var key = Tuple.Create(target.LiquidType, source.LiquidType);

        if (recipes.TryGetValue(key, out LiquidType newResult))
        {   
            if (minMaxMixingValue.TryGetValue(target.LiquidType, out var tupleValue)){
                var (min, max) = tupleValue;
                if (min <= target.Amount+transferAmount && target.Amount+transferAmount <= max){
                    if(target.LiquidType!= newResult) target.mixingValue = 0;
                    target.LiquidType = newResult;
                    target.SetLiquidMaterial();
                     // Apply the new result
                    
                    Debug.Log("New LiquidType: " + newResult);
                    return true;
                }
                else{
                    GeneralMistake("The amounts of the liquids are incorrect.",1);
                    return false;
                }
            }
            else{
                GeneralMistake("The amounts of the liquids are incorrect.",1);
                return false;
            }

        } else {          
            GeneralMistake($"Mixing failed: Are you shure these are the correct liquids?",1);
            return false;
        }
    }

    // Todo: since container.amount was meant to be private, maybe make a setter to change the amount indirectly
    public void MixingComplete(LiquidContainer container)
    {
        switch(container.LiquidType)
        {
            case LiquidType.Senna1m:
            {
                if (MixIfValid(container, 6000)){
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
}
