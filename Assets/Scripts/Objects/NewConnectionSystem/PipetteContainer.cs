using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class PipetteContainer : AttachmentItem
{
    public bool ignoreOldInteractStateCheck;

    public LiquidContainer Container;

    // How much liquid is moved per click
    public int LiquidTransferStep = 10000;

    public float defaultPosition, maxPosition;

    public Transform handle;

    // The LiquidContainer this Pipette is interacting with
    public LiquidContainer BottleContainer { get; set; }

    public bool hasBeenInBottle;

    [SerializeField]
    private ItemDisplay display;
    public ItemDisplay Display { get { return display; } set { display = value; } }

    [Tooltip("This is called when pipette capacity is exceeded")]
    public UnityEvent onCapacityExceeded;

    [Tooltip("This is called when liquid is transferred")]
    public UnityEvent<PipetteContainer, LiquidContainer> onTransferLiquid;

    protected override void Start() {
        base.Start();

        Type.On(InteractableType.Interactable);
    }

    public void TakeMedicine() {
        // Debug.Log("pipette container starts taking medicine");
        if (ignoreOldInteractStateCheck == false)
        {
            if (State == InteractState.InBottle)
            {
                TransferToBottle(false);
                return;
            }
            else
            {
                // Debug.Log("PipetteContainer not in bottle");
            }
        }
        TransferToBottle(false);
    }

    public void SendMedicine() {
        if (State == InteractState.InBottle)
        {
            TransferToBottle(true);
        }
    }

    private void Eject() {
        Container.SetAmount(0);
    }

    private void TransferToBottle(bool into) {
        // Debug.Log("pipette container still starts taking medicine");
        if (BottleContainer == null) return;
        // Debug.Log("we survived null bottle check and will now transfor to bottle container");
        
        if (!into && Vector3.Distance(BottleContainer.transform.position, transform.position) > 0.3f) return;
        // Debug.Log("Trying to transfer" + LiquidTransferStep);
        Container.TransferTo(BottleContainer, into ? LiquidTransferStep : -LiquidTransferStep);

        // Send info of liquid transfer to scene manager through an event
        onTransferLiquid?.Invoke(this, BottleContainer);
    }

    public int GetPipetteCapacity() {
        return Container.GetReceiveCapacity();
    }

    public void ExceededCapacity() { 
        onCapacityExceeded?.Invoke();
    }
}
