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
                    Debug.Log("Container added to DILTUINO");
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
