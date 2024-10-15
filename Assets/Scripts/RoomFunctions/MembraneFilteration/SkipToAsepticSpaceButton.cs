using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipToAsepticSpaceButton : MonoBehaviour
{

    public GameObject helperObject; // Reference to the GameObject with the ManualTester'sLilHelper component

    public void ActivateManualTesterHelper()
    {
        if (helperObject != null)
        {
            helperObject.SetActive(true);
            UISystem.Instance.CreatePopup(Translator.Translate("XR MembraneFilteration 2.0", "SkipToAsepticSpaceButton"), MsgType.Notify);
            G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);
        }
        else
        {
            Debug.LogError("Helper object reference is missing.");
        }
    }
}
