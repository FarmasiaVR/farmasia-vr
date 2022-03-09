using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentItem : GeneralItem
{
    public bool Attached = false;
    public AttachmentItem AttachedInteractable = null;

    public AttachmentItem GetParent() {
        if (!Attached) return this;
        return AttachedInteractable.GetParent();
    }
}
