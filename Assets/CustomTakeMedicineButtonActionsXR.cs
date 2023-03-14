using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomTakeMedicineButtonActionsXR : MonoBehaviour
{

    public BigPipette BigPipetteToControl;
    public InputActionReference takeMedicineActionReference;
    public InputActionReference transferMedicineActionReference;
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
        BigPipetteToControl.TakeMedicine();
    }

    public void transferMedicineBigBottle(InputAction.CallbackContext context)
    {
        BigPipetteToControl.SendMedicine();
    }

    public void toggleSelectState()
    {
        isSelected = !isSelected;
    }
}
