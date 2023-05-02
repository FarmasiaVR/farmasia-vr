using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using FarmasiaVR.Legacy;

public class LiquidContainer : MonoBehaviour {

    [SerializeField]
    private LiquidObject liquid;

    public delegate void AmountChange();
    public AmountChange OnAmountChange { get; set; }

    [SerializeField]
    private int amount;

    public LiquidType LiquidType;

    public GeneralItem GeneralItem;

    private TriggerInteractableContainer itemContainer;

    public bool Impure;

    public int Amount {
        get { return amount; }
    }
   
    public void SetAmountPercentage(float percentage) {
        int amount = (int)(percentage * Capacity);
        SetAmount(amount);
    }
    public void SetAmount(int value) {
        if (Capacity == 0) {
            amount = 0;
            liquid?.SetFillPercentage(0, this);
        } else {
            amount = Math.Max(Math.Min(value, Capacity), 0);
            // liquid is null when OnValidate is called twice before Awake
            // when playing in Editor Mode
            // See: https://forum.unity.com/threads/onvalidate-called-twice-when-pressing-play-in-the-editor.430250/
            float percentage = (float)amount / capacity;
            liquid?.SetFillPercentage(percentage, this);
        }
        OnAmountChange?.Invoke();
    }

    public void SetLiquidTypeNone() {
        LiquidType = LiquidType.None;
    }

    [SerializeField]
    private int capacity;
    public int Capacity {
        get { return capacity; }
        private set { capacity = Math.Max(value, 0); }
    }

    private void Awake() {
        Assert.IsNotNull(liquid);
        Capacity = capacity;
        SetAmount(amount);
    }


    private void Start() {
        itemContainer = gameObject.AddComponent<TriggerInteractableContainer>();
        itemContainer.OnEnter = OnTrueEnter;
        itemContainer.OnExit = OnTrueExit;

        StartCoroutine(SearchInteractable());

        IEnumerator SearchInteractable() {

            yield return null;

            GeneralItem = (GeneralItem)Interactable.GetInteractable(transform);
        }

        liquid.SetMaterialFromType(LiquidType);
    }

    public int GetReceiveCapacity() {
        return Capacity - Amount;
    }

    public void TransferTo(LiquidContainer target, int amount) {
         Debug.Log("Liguid container starts taking medicine");
        if (target == null) {
            Logger.Error("Receiving LiquidContainer was null");
            return;
        }
         Debug.Log("we survived target null check on bottle side");
        if (amount == 0) {
            return;
        }
        // Debug.Log("survived amount == 0 check");
        if (amount < 0) {
            target.TransferTo(this, -amount);
            return;
        }
         Debug.Log("survived amount < 0 check");
        int receiveCapacity = target.GetReceiveCapacity();
        int canSend = Math.Min(Amount, amount);
        int toTransfer = Math.Min(canSend, receiveCapacity);

        if (toTransfer == 0) return;
         Debug.Log("survived toTransfer == 0 check");
        TransferLiquidType(target);
      
        SetAmount(Amount - toTransfer);
        target.SetAmount(target.Amount + toTransfer);

        FireBottleFillingEvent(target);
    }

    private void TransferLiquidType(LiquidContainer target) {
        
        if (target.LiquidType == LiquidType || target.LiquidType == LiquidType.None) {
            target.LiquidType = LiquidType;
        } else { // Case: the target has held or holds different liquid
            
            if (target.Amount == 0) {
                
                target.LiquidType = LiquidType;
                target.Impure = true;
            } else {
                target.Impure = true;
            }
        }
        target.liquid.SetMaterialFromType(target.LiquidType);
    }

    private void FireBottleFillingEvent(LiquidContainer target) {
        //Debug.Log("FINALLY SENDING EVENT?");
        if (target.GeneralItem is Bottle || target.GeneralItem is FilterPart) {
            // Debug.Log("FINALLY SENDING EVENT!!");
            Events.FireEvent(EventType.TransferLiquidToBottle, CallbackData.Object(target));
        }
    }

    /// <summary>
    /// Tries to find a LiquidContainer component from the Transform component,
    /// or from its child object named "Liquid"
    /// </summary>
    /// <param name="t">The Transform of the GameObject to search</param>
    /// <returns>The LiquidContainer found or null</returns>
    public static LiquidContainer FindLiquidContainer(Transform t) {

        LiquidContainer c = t.GetComponent<LiquidContainer>();

        if (c != null) {
            return c;
        }

        return t.Find("Liquid")?.GetComponent<LiquidContainer>();
    }

    private void OnTrueEnter(Interactable enteringInteractable) {
        if (GeneralItem is null) return;
        Needle needle = enteringInteractable as Needle;
        Pipette pipette = enteringInteractable as Pipette;
        PipetteContainer pipetteContainer = enteringInteractable as PipetteContainer;
        SyringeNew syringeNew = enteringInteractable as SyringeNew;
        GeneralItem genItem = enteringInteractable as GeneralItem;

        if (needle!=null && needle.Connector.HasAttachedObject) {
            OnSyringeEnter(needle);
        }
        
        if (pipette!=null) {
            OnPipetteEnter(pipette);
        }

        if (pipetteContainer != null) {
            OnPipetteContainerEnter(pipetteContainer);
        }

        if (syringeNew != null) {
            OnSyringeNewEnter(syringeNew);
        }

        if (genItem != null) {
            //UnityEngine.Debug.Log("calling on filter half enter! from liguid container");
            OnFilterHalfEnter(genItem);
        }
    }

    private void OnSyringeEnter(Needle needle) {
        Syringe syringe = needle.Connector.AttachedInteractable as Syringe;
        if (syringe == null) {
            return;
        }

        if (GeneralItem.ObjectType == ObjectType.Bottle || GeneralItem.ObjectType == ObjectType.Medicine) {
            syringe.State.On(InteractState.InBottle);
            syringe.hasBeenInBottle = true;

            if (!GeneralItem.IsClean) {
                needle.Contamination = GeneralItem.ContaminateState.Contaminated;
            }

            if (G.Instance.Scene is MedicinePreparationScene) {
                if ((G.Instance.Scene as MedicinePreparationScene).NeedleUsed) {
                    Task.CreateGeneralMistake("L��kett� yritettiin ottaa uudestaan");
                }
            }
        }

        syringe.BottleContainer = this;
    }

    private void OnPipetteEnter(Pipette pipette) {
        if (GeneralItem is Bottle || GeneralItem.ObjectType == ObjectType.PumpFilterTank) {
            pipette.State.On(InteractState.InBottle);
            pipette.hasBeenInBottle = true;

            if (!GeneralItem.IsClean) {
                pipette.Contamination = GeneralItem.ContaminateState.Contaminated;
            }
        }

        pipette.BottleContainer = this;
    }

    private void OnPipetteContainerEnter(PipetteContainer pipette) {
        if (GeneralItem is Bottle) {
            pipette.State.On(InteractState.InBottle);
            pipette.hasBeenInBottle = true;

            if (!GeneralItem.IsClean) {
                pipette.Contamination = GeneralItem.ContaminateState.Contaminated;
            }
        }

        pipette.BottleContainer = this;
    }

    private void OnSyringeNewEnter(SyringeNew syringeNew) {
        if (GeneralItem is FilterPart) {
            syringeNew.State.On(InteractState.InBottle);
            syringeNew.hasBeenInBottle = true;

            if (!GeneralItem.IsClean) {
                syringeNew.Contamination = GeneralItem.ContaminateState.Contaminated;
            }
            syringeNew.BottleContainer = this;
        }
    }

    private void OnFilterHalfEnter(GeneralItem genItem) {
        //UnityEngine.Debug.Log("somebody called filter half enter!");
        if (GeneralItem is Bottle && genItem.ObjectType == ObjectType.FilterHalf) {
            //UnityEngine.Debug.Log("survived bottle check");
            genItem.State.On(InteractState.InBottle);

            if (!GeneralItem.IsClean) {
                //UnityEngine.Debug.Log("filter is contaminated!");
                genItem.Contamination = GeneralItem.ContaminateState.Contaminated;
            }
            //UnityEngine.Debug.Log("CALLED FILTER HALF ENTER!");
            Events.FireEvent(EventType.FilterHalfEnteredBottle, CallbackData.Object(GeneralItem));
        }
    }

    private void OnTrueExit(Interactable enteringInteractable) {
        Needle needle = enteringInteractable as Needle;
        if (needle != null) {
            OnNeedleExit(needle);
        }

        Pipette pipette = enteringInteractable as Pipette;
        if (pipette != null) {
            OnPipetteExit(pipette);
        }

        PipetteContainer pipetteContainer = enteringInteractable as PipetteContainer;
        if (pipetteContainer != null) {
            OnPipetteContainerExit(pipetteContainer);
        }

        SyringeNew syringeNew = enteringInteractable as SyringeNew;
        if (syringeNew != null) {
            OnSyringeNewExit(syringeNew);
        }
    }

    private void OnNeedleExit(Needle needle) {
        Syringe syringe = needle.Connector.AttachedInteractable as Syringe;
        if (syringe == null) {
            return;
        }

        if (GeneralItem.ObjectType == ObjectType.Bottle || GeneralItem.ObjectType == ObjectType.Medicine || GeneralItem.ObjectType == ObjectType.PumpFilterTank) {
            syringe.State.Off(InteractState.InBottle);
            syringe.BottleContainer = null;
        }
    }

    private void OnPipetteExit(Pipette pipette) {
        if (GeneralItem.ObjectType == ObjectType.Bottle || GeneralItem.ObjectType == ObjectType.Medicine || GeneralItem.ObjectType == ObjectType.PumpFilterTank) {
            pipette.State.Off(InteractState.InBottle);
            pipette.BottleContainer = null;
        }
    }
    private void OnPipetteContainerExit(PipetteContainer pipetteContainer) {
        if (GeneralItem.ObjectType == ObjectType.Bottle || GeneralItem.ObjectType == ObjectType.Medicine || GeneralItem.ObjectType == ObjectType.PumpFilterTank) {
            pipetteContainer.State.Off(InteractState.InBottle);
            pipetteContainer.BottleContainer = null;
        }
    }

    private void OnSyringeNewExit(SyringeNew syringe) {
        if (GeneralItem.ObjectType == ObjectType.Bottle || GeneralItem.ObjectType == ObjectType.Medicine || GeneralItem.ObjectType == ObjectType.PumpFilterTank) {
            syringe.State.Off(InteractState.InBottle);
            syringe.BottleContainer = null;
        }
    }
}
