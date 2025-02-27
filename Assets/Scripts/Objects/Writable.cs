using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class Writable : WritingTarget {

    // Displays the text, must have a TextMeshPro component
    [SerializeField]
    private GameObject textObject;

    [SerializeField]
    public int MaxLines = 4;
    
    [SerializeField]
    private TextMeshPro textField;

    [SerializeField]
    public bool isAgar = false;

    public string Text {
        get { return textField.GetParsedText(); }
    }

    public Dictionary<WritingType, string> WrittenLines = new Dictionary<WritingType, string>();
    public List<WritingType> writingsInOrder = new List<WritingType>();


    public void AddWrittenLines(Dictionary<WritingType, string> options) {
        
        foreach(var option in options)
        {
            WrittenLines.Add(option.Key, option.Value);
        }

        UpdateWrittenLines();
    }

    public void addLine(WritingType type, string text)
    {
        if (!WrittenLines.ContainsKey(type) && writingsInOrder.Count <= MaxLines)
        {
            WrittenLines.Add(type, text);
            writingsInOrder.Add(type);
            UpdateWrittenLines();
        }
    }

    public void removeLine(WritingType type)
    {
        if (WrittenLines.ContainsKey(type))
        {
            WrittenLines.Remove(type);
            writingsInOrder.Remove(type);
            UpdateWrittenLines();
        }
    }

    public void removeLatest()
    {
        if(writingsInOrder.Count>0)
        {
            WritingType writingToRemove = writingsInOrder[writingsInOrder.Count - 1];
            removeLine(writingToRemove);
        }
    }

    public void UpdateWrittenLines()
    {
        string resultText = "";
        int n = 0;
        foreach (string line in WrittenLines.Values)
        {
            resultText += line + '\n';
            n++;
            if (n == 2 && isAgar)
            {
                resultText += '\n';
            }
        }
        textField.SetText(resultText);
    }

    void Start() {
        if (textField == null) {
            Logger.Warning("Writable '" + gameObject.ToString() + "' does not have a valid textObject attached");
        }
    }

    public override Writable GetWritable() {
        return this;
    }

    public List<WritingType> GetWritingsInOrder()
    {
        return writingsInOrder;
    }
}
