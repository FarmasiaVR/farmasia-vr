using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachItem : GeneralItem
{
    public bool Attached = false;
    public AttachItem AttachedInteractable = null;

    public AttachItem GetParent() {
        if (!Attached) return this;
        return AttachedInteractable.GetParent();
    }
}
