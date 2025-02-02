using UnityEngine;

/// <summary>
/// Is <c>ReceiverItem</c> for PipetteContainer and transfers controller press events to it.
/// </summary>
public class BigPipetteXR : MonoBehaviour 
{
    public PipetteContainer pipetteContainerXR;
   


    public void TakeMedicine()
    {
        // Debug.Log("Big pipette starts taking medicine");
        if (pipetteContainerXR)
        {
            pipetteContainerXR.TakeMedicine();
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

}
