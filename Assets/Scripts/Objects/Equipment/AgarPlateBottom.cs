using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class AgarPlateBottom : GeneralItem {
    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.AgarPlateBottom;
        Type.On(InteractableType.Attachable, InteractableType.Interactable, InteractableType.SmallObject);
    }
}
