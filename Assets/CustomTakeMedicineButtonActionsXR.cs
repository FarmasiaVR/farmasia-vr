using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CustomTakeMedicineButtonActionsXR : MonoBehaviour
{
    public InputActionReference takeMedicineActionReference;
    public InputActionReference transferMedicineActionReference;
    public UnityEvent<InputAction.CallbackContext> onTakeMedicineFunctionsToCall;
    public UnityEvent<InputAction.CallbackContext> onSendMedicineFunctionsToCall;
    bool isSelected = false;

    void Start()
    {
        if (takeMedicineActionReference)
        {
            takeMedicineActionReference.action.started += takeMedicineBigBottle;
        }

        if (transferMedicineActionReference)
        {
            transferMedicineActionReference.action.started += transferMedicineBigBottle;

        }
    }

    private void OnDestroy()
    {
        if (takeMedicineActionReference)
        {
            takeMedicineActionReference.action.started -= takeMedicineBigBottle;
        }

        if (transferMedicineActionReference)
        {
            transferMedicineActionReference.action.started -= transferMedicineBigBottle;

        }
    }

    public void takeMedicineBigBottle(InputAction.CallbackContext context)
    {
        if (isSelected)
        {
            onTakeMedicineFunctionsToCall.Invoke(context);
        }
    }

    public void transferMedicineBigBottle(InputAction.CallbackContext context)
    {
        if (isSelected)
        {
            onSendMedicineFunctionsToCall.Invoke(context);
        }
    }

    public void toggleSelectState()
    {
        isSelected = !isSelected;
    }
}
