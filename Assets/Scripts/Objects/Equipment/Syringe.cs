using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : GeneralItem {

    [SerializeField]
    private ObjectType objectType = ObjectType.Syringe;
    public ObjectType ObjectType { get => objectType; }

    [SerializeField]
    private int size = 50;
    public int Size {
        get => size;
        set {
            if (size >= 0)
                size = value;
        } 
    }

    [SerializeField]
    private string content = "";
    public string Content {
        get => content;
        set { content = value; }
    }

    [SerializeField]
    private int contentLeft = 0;
    public int ContentLeft {
        get => contentLeft;
        set {
            if (value <= size && value > 0)
                contentLeft = value;
        }
    }
    void Start() {
        base.Start();
    }
}
