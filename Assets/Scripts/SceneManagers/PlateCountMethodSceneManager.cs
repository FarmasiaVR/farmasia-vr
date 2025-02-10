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
    private Dictionary<string, LiquidContainer> dilutionTypes = new Dictionary<string, LiquidContainer>();

    private void Awake()
    {
        taskManager = GetComponent<TaskManager>();
        // Prepare dictionaries
        testTubes.Add("dilution", new List<LiquidContainer>());
        testTubes.Add("control", new List<LiquidContainer>());

        dilutionTypes.Add("one-tenth", null);
        dilutionTypes.Add("one-hundredth", null);
        dilutionTypes.Add("one-thousandth", null);
        dilutionTypes.Add("control", null);
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
