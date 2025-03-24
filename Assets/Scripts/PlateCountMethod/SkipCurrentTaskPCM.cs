using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SkipCurrentTaskPCM : MonoBehavior
{
    public GameObject skips; // Assigned in the inspector
    private int smallObjectLayer = LayerMask.NameToLayer("SmallObject");
    private int tubeLayer = LayerMask.NameToLayer("TestTube");

    public void SkipCurrentTask(string currentTask)
    {
        switch (currentTask)
        {
            case "toolsToCabinet":
                Transform toolsToCabinetGO = skips.transform.Find("ToolsToCabinet");
                toolsToCabinetGO.gameObject.SetActive(true);
                break;
            case "WriteOnTubes":
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

                foreach (GameObject obj in objectsInLaminarCabinet)
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
                    SubmitWriting(obj.GetComponent<GeneralItem>(), writtenLines);
                }
                break;
            case "FillTubes":
                int dilutionTubes = 0;
                int controlTubes = 0;
                foreach (GameObject obj in objectsInLaminarCabinet)
                {
                    if (obj.layer == tubeLayer && obj.name.Contains("TestTubePCM"))
                    {
                        LiquidContainer liquid = obj.transform.GetComponentInChildren<LiquidContainer>();
                        TextMeshPro tmpText = obj.GetComponentInChildren<TextMeshPro>();
                        if (!tmpText.text.Contains("Control") && dilutionTubes < 3)
                        {
                            liquid.SetAmount(4500);
                            liquid.LiquidType = LiquidType.PhosphateBuffer;
                            dilutionTubes++;
                        }
                        else if (controlTubes < 1)
                        {
                            liquid.SetAmount(1000);
                            liquid.LiquidType = LiquidType.PhosphateBuffer;
                            controlTubes++;
                        }
                    }
                }
                break;
            case "MixPhosphateToSenna":
                foreach (GameObject obj in objectsInLaminarCabinet)
                {
                    if (obj.layer == tubeLayer && obj.name.Contains("SennaTube"))
                    {
                        LiquidContainer liquid = obj.transform.GetComponentInChildren<LiquidContainer>();
                        liquid.SetAmount(6000);
                        liquid.LiquidType = LiquidType.Senna1;
                    }
                }
                break;
            case "PerformSerialDilution":
                foreach (GameObject obj in objectsInLaminarCabinet)
                {
                    if (obj.layer == tubeLayer && obj.name.Contains("TestTubePCM"))
                    {
                        LiquidContainer liquid = obj.transform.GetComponentInChildren<LiquidContainer>();
                        TextMeshPro tmpText = obj.GetComponentInChildren<TextMeshPro>();
                        if (tmpText.text.Contains("1:10") && liquid.LiquidType == LiquidType.PhosphateBuffer)
                        {
                            liquid.SetAmount(5000);
                            liquid.LiquidType = LiquidType.Senna01;
                        }
                        else if (tmpText.text.Contains("1:100") && liquid.LiquidType == LiquidType.PhosphateBuffer)
                        {
                            liquid.SetAmount(5000);
                            liquid.LiquidType = LiquidType.Senna001;
                        }
                        else if (tmpText.text.Contains("1:1000") && liquid.LiquidType == LiquidType.PhosphateBuffer)
                        {
                            liquid.SetAmount(5000);
                            liquid.LiquidType = LiquidType.Senna0001;
                        }
                    }
                }
                break;
        }
    }
}