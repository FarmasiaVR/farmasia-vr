using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using FarmasiaVR.Legacy;

/// <summary>
/// Is <c>ReceiverItem</c> for PipetteContainer and transfers controller press events to it.
/// </summary>
public class BigPipetteXR : MonoBehaviour 
{
    public PlateCountMethodSceneManager sceneManager;
    public PipetteContainer pipetteContainerXR;

    [Tooltip("This is called when pipette capacity is exceeded")]
    public UnityEvent<string, int> onCapacityExceeded;

    public void TakeMedicine()
    {
        // Debug.Log("Big pipette starts taking medicine");
        if (pipetteContainerXR)
        {
            // Checks if the connected pipette is full
            if (pipetteContainerXR.GetPipetteCapacity() == 0) {
                PipetteCapacityExceeded();
            } else {
                pipetteContainerXR.TakeMedicine();
            }
        }
    }

    public void SendMedicine()
    {
        if (pipetteContainerXR) pipetteContainerXR.SendMedicine();
    }

    public void setConnectedItem(PipetteContainer NewConnectedItem)
    {
        pipetteContainerXR = NewConnectedItem;
    }

    public void PipetteCapacityExceeded()
    {   Debug.Log("Can't take more medicine");
        if (sceneManager != null)
        { 
            sceneManager.GeneralMistake("BreakingPipette", 10);
            pipetteContainerXR.ExceededCapacity();
        } else {
            pipetteContainerXR.ExceededCapacity();
            onCapacityExceeded?.Invoke("Overdrawn pipettor", 1);
            Task.CreateGeneralMistake(Translator.Translate("XR MembraneFilteration 2.0", "Overdrawn pipettor"), 1);
            
        }

    }
}
