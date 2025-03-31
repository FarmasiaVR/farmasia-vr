using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SkipCurrentTaskPCM : MonoBehaviour
{
    public GameObject skips; // Assigned in the inspector
    public GameObject laminarCabinetGO; // Assigned in the inspector
    private CabinetBasePCM laminarCabinet;
    public PlateCountMethodSceneManager sceneManager; // Assigned in the inspector
    private int smallObjectLayer;
    private int tubeLayer;

    private void Awake()
    {
        if (laminarCabinetGO != null)
        {
            laminarCabinet = laminarCabinetGO.GetComponent<CabinetBasePCM>();
        }
        else
        {
            Debug.LogError("laminarCabinetGO is not assigned in the Inspector!");
        }
        smallObjectLayer = LayerMask.NameToLayer("SmallObject");
        tubeLayer = LayerMask.NameToLayer("TestTube");
    }

    public void SkipCurrentTask(string currentTask)
    {
        switch (currentTask)
        {
            case "toolsToCabinet":
                Transform toolsToCabinetGO = skips.transform.Find("ToolsToCabinet");
                toolsToCabinetGO.gameObject.SetActive(true);
                break;
            case "WriteOnTubes":
                SkipWriteOnTubes();
                break;
            case "FillTubes":
                ChangeLiquidInDilutionTubes(currentTask);
                break;
            case "MixPhosphateToSenna":
                foreach (GameObject obj in laminarCabinet.objectsInCabinet)
                {
                    if (obj.layer == tubeLayer && obj.name.Contains("SennaTube"))
                    {
                        LiquidContainer liquid = obj.transform.GetComponentInChildren<LiquidContainer>();
                        SetLiquid(liquid, 6000, LiquidType.Senna1);
                        break;
                    }
                }
                break;
            case "PerformSerialDilution":
                ChangeLiquidInDilutionTubes(currentTask);
                break;
            case "SpreadDilution":
                sceneManager.PrepareForVentilationTask();
                break;
        }
    }

    private void ChangeLiquidInDilutionTubes(string task)
    {
        int dilutionTubes = 0;
        int controlTubes = 0;

        foreach (GameObject obj in laminarCabinet.objectsInCabinet)
        {
            if (obj.layer != tubeLayer || !obj.name.Contains("TestTubePCM")) continue;

            LiquidContainer liquid = obj.transform.GetComponentInChildren<LiquidContainer>();
            TextMeshPro tmpText = obj.GetComponentInChildren<TextMeshPro>();

            switch(task)
            {
                case "FillTubes":
                    if (!tmpText.text.Contains("Control") && dilutionTubes < 3)
                    {
                        SetLiquid(liquid, 4500, LiquidType.PhosphateBuffer);
                        dilutionTubes++;
                    }
                    else if (controlTubes < 1)
                    {
                        SetLiquid(liquid, 1000, LiquidType.PhosphateBuffer);
                        controlTubes++;
                    }
                    break;
                case "PerformSerialDilution":
                    if (tmpText.text == "1:10" && liquid.LiquidType == LiquidType.PhosphateBuffer)
                    {
                        SetLiquid(liquid, 5000, LiquidType.Senna01);
                    }
                    else if (tmpText.text == "1:100" && liquid.LiquidType == LiquidType.PhosphateBuffer)
                    {
                        SetLiquid(liquid, 5000, LiquidType.Senna001);
                    }
                    else if (tmpText.text == "1:1000" && liquid.LiquidType == LiquidType.PhosphateBuffer)
                    {
                        SetLiquid(liquid, 5000, LiquidType.Senna0001);
                    }
                    break;
            }
        }
    }

    private void SetLiquid(LiquidContainer liquid, int amount, LiquidType type)
    {
        liquid.SetAmount(amount);
        liquid.LiquidType = type;
        liquid.SetLiquidMaterial();
    }

    private void SkipWriteOnTubes()
    {
        string[] dilutionWritings = { "1:10", "1:100", "1:1000", "Control" };
        WritingType[] writingTypes = {
                    WritingType.OneToTen,
                    WritingType.OneToHundred,
                    WritingType.OneToThousand,
                    WritingType.Control
                };
        string date = "23.03.2025";
        string player = "Pelaaja";
        int sabouraudIdx = 0;
        int soyCaseineIdx = 0;
        int testTubeIdx = 0;

        foreach (GameObject obj in laminarCabinet.objectsInCabinet)
        {
            TextMeshPro tmp = obj.GetComponentInChildren<TextMeshPro>();
            if (tmp == null)
            {
                continue;
            }

            Dictionary<WritingType, string> writtenLines = new Dictionary<WritingType, string>();
            string tmpDilutionText = null;
            if (obj.layer == smallObjectLayer && obj.name.Contains("agarplatelid"))
            {
                AgarPlateLid lid = obj.GetComponent<AgarPlateLid>();
                if (lid.Variant == "Sabourad-dekstrosi")
                {
                    tmpDilutionText = $"{dilutionWritings[sabouraudIdx]}";
                    writtenLines[writingTypes[sabouraudIdx]] = tmpDilutionText;
                    sabouraudIdx++;
                }
                else if (lid.Variant == "Soija-kaseiini")
                {
                    tmpDilutionText = $"{dilutionWritings[soyCaseineIdx]}";
                    writtenLines[writingTypes[soyCaseineIdx]] = tmpDilutionText;
                    soyCaseineIdx++;
                }

                tmp.text = $"{tmpDilutionText}\n" +
                    $"{date}\n\n" +
                    $"{player}";
                writtenLines[WritingType.Date] = date;
                writtenLines[WritingType.Name] = player;
            }
            else if (obj.layer == tubeLayer && obj.name.Contains("TestTubePCM"))
            {
                tmp.text = $"{dilutionWritings[testTubeIdx]}";
                writtenLines[writingTypes[testTubeIdx]] = tmpDilutionText;
                testTubeIdx++;
            }
            sceneManager.SubmitWriting(obj.GetComponent<GeneralItem>(), writtenLines);
        }
    }
}