using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralItem : MonoBehaviour {

    private int flags;

    void Start() {
        EnableFlags(ItemState.Status.Clean);
    }

    public void EnableFlags(params ItemState.Status[] status) {
        ItemState.EnableFlags(ref flags, status);
    }

    public void DisableFlags(params ItemState.Status[] status) {
        ItemState.DisableFlags(ref flags, status);
    }

    public bool GetFlag(ItemState.Status status) {
        return ItemState.GetFlag(flags, status);
    }
}
