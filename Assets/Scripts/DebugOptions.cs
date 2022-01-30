using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOptions : MonoBehaviour
{
    public bool DebugInput = false;

    private void Update() {
        VRInput.DebugInput = DebugInput;
    }
}
