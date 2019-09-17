using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineBottle : GeneralItem {


    [SerializeField]
    private int size = 100;
    public int Size {
        get => size;
        set {
            if (size >= 0)
                size = value;
        } 
    }

    [SerializeField]
    private string content = "medicine";
    public string Content { get => content; }

    [SerializeField]
    private int contentLeft = 100;
    public int ContentLeft {
        get => contentLeft;
        set {
            if (value <= size && value > 0)
                contentLeft = value;
        }
    }
    void Start() {
        base.Start();
        objectType = ObjectType.Bottle;
    }

}
