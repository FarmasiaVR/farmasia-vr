using System;
using UnityEngine;

public class LaminarCabinetButton : MonoBehaviour {
    #region fields
    private bool isOn;
    #endregion

    private void Start() {
        isOn = false;
    }

    public void CheckButtonState() {
        isOn = !isOn;
        Events.FireEvent(EventType.CorrectItemsInLaminarCabinet, CallbackData.Boolean(isOn));
    }
}