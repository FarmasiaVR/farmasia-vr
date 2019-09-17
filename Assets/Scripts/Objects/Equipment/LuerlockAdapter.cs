using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuerlockAdapter : GeneralItem {

    [SerializeField]
    private GameObject attachedObject1;
    public GameObject AttachedObject1 {
        get => attachedObject1;
        set {
            attachedObject1 = value;
        }
    }

    [SerializeField]
    private GameObject attachedObject2;
    public GameObject AttachedObject2 {
        get => attachedObject2;
        set {
            attachedObject2 = value;
        }
    }

    void Start() {
        base.Start();
        objectType = ObjectType.Luerlock;
    }

}
