using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateCountMethodSceneManager : MonoBehaviour
{
    private TaskManager taskManager;
    private const int sennaTubeAmount = 5000;
    private const int dilutionTubesAmount = 4500;
    private const int controlTubeAmount = 1000;
    // Dict that stores information about dilution and control tubes
    private Dictionary<string, List<LiquidContainer>> testTubes = new Dictionary<string, List<LiquidContainer>>();

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
        { LiquidType.SennaPowder, Tuple.Create(1000, 1000) },
        { LiquidType.Senna1m, Tuple.Create(500, 6000) },
        { LiquidType.Senna1, Tuple.Create(500, 6000) },
        { LiquidType.PhosphateBuffer, Tuple.Create(4500, 5000) },
        { LiquidType.Senna01m, Tuple.Create(500, 5000) },
        { LiquidType.Senna001m, Tuple.Create(4500, 5000) },
        { LiquidType.Senna0001m, Tuple.Create(4500, 5000) }
    };

    public static Dictionary<LiquidType, LiquidType> mixingTable = new()
    {
        { LiquidType.Senna01m, LiquidType.Senna01 },
        { LiquidType.Senna001m, LiquidType.Senna001 },
        { LiquidType.Senna0001m, LiquidType.Senna0001 },
     
    };


    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
        // Prepare dictionary
        testTubes.Add("dilution", new List<LiquidContainer>());
        testTubes.Add("control", new List<LiquidContainer>());
    }

    public void CompleteTask(string taskName)
    {
        Debug.Log($"Trying to complete task");
        taskManager.CompleteTask(taskName);
    }

    public void CleanHands()
    {
        CompleteTask("WashHands");
    }

    public void CheckTubesFill(LiquidContainer container)
    {
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

        if (TubesAreFilled())
        {
            CompleteTask("FillTubes");
            Debug.Log("All the tubes are filled");
        }
    }

    public void CheckIfSennaInControlBottle(LiquidContainer target, LiquidContainer source)
    {
        if (testTubes["control"].Contains(target) && source.LiquidType == LiquidType.SennaPowder && target.LiquidType==LiquidType.PhosphateBuffer)
        {
            Debug.Log($"target: {target.LiquidType}, source:{source.LiquidType}");
            GeneralMistake("DON'T PUT SENNA IN THE CONTROL TUBE", 1);
        }
    }

    public bool Dilution(LiquidContainer container1, LiquidContainer container2){        
        Debug.Log("Trying to mix: " + container1.LiquidType + " with " + container2.LiquidType);

        var key = Tuple.Create(container2.LiquidType,container1.LiquidType);

        if (recipes.TryGetValue(key, out LiquidType newResult))
        {   
            
            if (minMaxMixingValue.TryGetValue(container2.LiquidType, out var tupleValue)){
                var (min, max) = tupleValue;
                if (min <= container2.amount && container2.amount <= max){
                    if(container2.LiquidType!=newResult) container2.mixingValue = 0;
                    container2.LiquidType = newResult; // Apply the new result
                    
                    Debug.Log("New LiquidType: " + newResult);
                    return true;
                }
                else{
                    Debug.Log("Mixing failed: No matching recipe found.");
                    return false;
                }
            }
            else{
                Debug.Log("Mixing failed: Ammounts faile.");
                return false;
            }

        }
        else
        {          
            Debug.Log("Mixing failed: No matching recipe found.");
            return false;
        }
    }

    public void MixingComplete(LiquidContainer container){
        
        if (mixingTable.TryGetValue(container.LiquidType, out LiquidType newResult) && container.amount == 5000) container.LiquidType = newResult;
    }
    
    public void GeneralMistake(string message, int penalty)
    {
        taskManager.GenerateGeneralMistake(message, penalty);
    }

    private bool TubesAreFilled()
    {
        bool filled = testTubes["dilution"].Count == 3 && testTubes["control"].Count == 1;
        return filled;
    }
}
