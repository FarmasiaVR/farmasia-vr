using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using FarmasiaVR.Legacy;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;
using JetBrains.Annotations;

public class LiquidContainer : MonoBehaviour {

    [SerializeField]
    private LiquidObject liquid;

    public delegate void AmountChange();
    public AmountChange OnAmountChange { get; set; }

    [SerializeField]
    public int amount;

    public LiquidType LiquidType;
    public bool pcm;
    public MixingManager mixingManager;    
    public LiquidType contaminationLiquidType = LiquidType.None; //This is used in PCM for "Actually Working Pipette XR" to keep track of what it is contaminated with.
    public GeneralItem GeneralItem;

    private TriggerInteractableContainer itemContainer;

    public bool Impure;

    [Tooltip("Should the liquid container be allowed to transfer liquids to other containers")]
    public bool allowLiquidTransfer = true;

    [Tooltip("If set to true, then there can only be one type of liquid in the container at one time and mixing is impossible")]
    public bool allowMixingLiquids = true;

    [Tooltip("Called when the container TransferTo function is called. Passes the amount of liquid and the type of liquid in the container as the parameter.")]
    public UnityEvent<LiquidContainer> onLiquidTransfer;

    [Tooltip("Called when the container is filled. Passes the amount of liquid and the type of liquid in the container as the parameter.")]
    public UnityEvent<LiquidContainer> onLiquidAmountChanged;

    [Tooltip("Called when container transfers liquid to another container. Returns <target liquid container, source liquid container>")]
    public UnityEvent<LiquidContainer, LiquidContainer> onLiquidExchange;

    [Tooltip("Called when a filter half is dropped into the liquid container. Passes the filter half GeneralItem as a parameter")]
    public UnityEvent<GeneralItem> onFilterHalfEnter;

    [Tooltip("Called when liquid changes. Passes both source and target containers as parameters.")]
    public UnityEvent<LiquidType> onLiquidTypeChange;

    public UnityEvent<LiquidContainer> onMixingComplete;
    
    //This block is here for the PCM mixing functionality
    public bool mixingComplete = false;
    public int mixingValue = 0;
    private int pipettorMixingValue = 0;
    [SerializeField] private float movementSensitivity = 10f;
    private float lastYPosition;
    private float lastXPosition;
    private float lastZPosition;

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

        SetLiquidMaterial();
    }

    private void Update() {
        if (mixingComplete) return;
        if (GeneralItem){
            if (GeneralItem.ObjectType == ObjectType.Bottle && mixingManager != null) {
                float deltaY = transform.position.y - lastYPosition;
                float deltaX = transform.position.x - lastXPosition;
                float deltaZ = transform.position.z - lastZPosition;
                float totalMovement = Mathf.Sqrt(deltaY * deltaY + deltaX * deltaX + deltaZ * deltaZ);
                if (totalMovement > 0.001f) {
                    int changeAmount = Mathf.RoundToInt(totalMovement * movementSensitivity);
                    mixingValue+=Mathf.Abs(changeAmount)*500;
                }
                if (mixingValue >= 10000) {
                    onMixingComplete!.Invoke(this);
                    mixingValue = 0;
                }
                lastYPosition = transform.position.y;
                lastXPosition = transform.position.x;
                lastZPosition = transform.position.z;
            }
        }

    }

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

    public void SetLiquidMaterial() {
        if (pcm) onLiquidTypeChange?.Invoke(LiquidType);
        else liquid.SetMaterialFromType(LiquidType);
    }
    
    [SerializeField]
    private int capacity;
    public int Capacity {
        get { return capacity; }
        private set { capacity = Math.Max(value, 0); }
    }

    public int GetReceiveCapacity() {
        return Capacity - Amount;
    }

    public void TransferTo(LiquidContainer target, int amount) {
        if (!allowLiquidTransfer) return;
        //Debug.Log("Liguid container starts taking medicine");
        
        //This is to check whether a mix would happen with the transfer
        if (this.LiquidType != target.LiquidType && this.amount > 0 && target.amount > 0 && !target.allowMixingLiquids) 
        {
            FindObjectOfType<PopupManager>()?.NotifyPopup("Ole hyvä ja älä sekoita nesteitä");
            return;
        }

        if (target == null) {
            Logger.Error("Receiving LiquidContainer was null");
            return;
        }
        if (amount == 0) {
            return;
        }
        // Debug.Log("survived amount == 0 check");
        if (amount < 0) {
            target.TransferTo(this, -amount);
            return;
        }
         // Debug.Log("survived amount < 0 check");
        int receiveCapacity = target.GetReceiveCapacity();
        int canSend = Math.Min(Amount, amount);
        int toTransfer = Math.Min(canSend, receiveCapacity);

        if (toTransfer == 0) return;
         // Debug.Log("survived toTransfer == 0 check");
        
        if(this.LiquidType != target.LiquidType && this.amount > 0 && target.amount > 0 && pcm){
            if (mixingManager != null)
            {
                Debug.Log("mixingManager is assigned. Calling Dilution and Test methods.");
                // Call Dilution and Test methods
                // Denies transfering liquid if mixing the wrong recipe
                if(!mixingManager.Dilution(this, target, toTransfer)) toTransfer = 0;

            }
            else
            {
                Debug.LogError("mixingManager is null! Please assign it.");
            }            
            //toTransfer = 0;
            if (toTransfer == 0) return;
        }
        else{
            TransferLiquidType(target);
        }

        SetAmount(Amount - toTransfer);
        target.SetAmount(target.Amount + toTransfer);

        if (target.GeneralItem.ObjectType == ObjectType.Bottle && !target.mixingComplete)
        {
            target.pipettorMixingValue += 400; //PCM mixing functionality
            if (target.pipettorMixingValue >= 10000 && mixingManager != null && this.amount == 0)
            {
                onMixingComplete!.Invoke(target);
            }
        }      
        
        target.onLiquidTransfer.Invoke(target);
        onLiquidAmountChanged.Invoke(this);
        FireBottleFillingEvent(target);
    }

    private void TransferLiquidType(LiquidContainer target) {
       
        //This is used in PCM in phase after dilution
        target.contaminationLiquidType = LiquidType;
        // if (target.contaminationLiquidType == LiquidType.None){
        //     target.contaminationLiquidType = target.LiquidType;
        // }
        if (target.LiquidType == LiquidType || target.LiquidType == LiquidType.None) {
            target.LiquidType = LiquidType;
        } else { // Case: the target has held or holds different liquid
            
            //if container empty, the medicines didnt mix, but liquid is still impure...
            if (target.Amount == 0) {
                switchLiquidTypesAndMakeImpure(target);
            }
            //else, there was another type of liquid already in the container,
            //so give negative points and switch the liquid type to the just added medicine
            //TODO: should there be a mixedMedicine liquidType?
            else {
                switchLiquidTypesAndMakeImpure(target);
                Task.CreateGeneralMistake(Translator.Translate("XR MembraneFilteration 2.0", "MedicinesWereMixed"));
            }
        }
        target.SetLiquidMaterial();
    }

    void switchLiquidTypesAndMakeImpure(LiquidContainer target)
    {
        target.LiquidType = LiquidType;
        target.Impure = true;
    }

    private void FireBottleFillingEvent(LiquidContainer target) {
        // Debug.Log("FINALLY SENDING EVENT?");
        if (target.GeneralItem is Bottle || target.GeneralItem is FilterPart) {
            // Debug.Log("FINALLY SENDING EVENT!!");
            target.onLiquidAmountChanged.Invoke(target);
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
        if (GeneralItem is Bottle || GeneralItem.ObjectType == ObjectType.PumpFilterTank || GeneralItem is AgarPlateBottom) {
            pipette.State.On(InteractState.InBottle);
            pipette.hasBeenInBottle = true;

            if (!GeneralItem.IsClean) {
                pipette.Contamination = GeneralItem.ContaminateState.Contaminated;
            }
        }

        pipette.BottleContainer = this;
    }

    private void OnPipetteContainerEnter(PipetteContainer pipette) {
        if (GeneralItem is Bottle || GeneralItem is AgarPlateBottom) {
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
        // UnityEngine.Debug.Log("somebody called filter half enter!");
        if (GeneralItem is Bottle && genItem.ObjectType == ObjectType.FilterHalf) {
            //UnityEngine.Debug.Log("survived bottle check");
            genItem.State.On(InteractState.InBottle);

            if (!GeneralItem.IsClean) {
                //UnityEngine.Debug.Log("filter is contaminated!");
                genItem.Contamination = GeneralItem.ContaminateState.Contaminated;
            }
            //UnityEngine.Debug.Log("CALLED FILTER HALF ENTER!");
            Events.FireEvent(EventType.FilterHalfEnteredBottle, CallbackData.Object(GeneralItem));
            onFilterHalfEnter.Invoke(genItem);
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
        if (GeneralItem.ObjectType == ObjectType.Bottle || GeneralItem is AgarPlateBottom || GeneralItem.ObjectType == ObjectType.Medicine || GeneralItem.ObjectType == ObjectType.PumpFilterTank) {
            pipette.State.Off(InteractState.InBottle);
            pipette.BottleContainer = null;
        }
    }
    private void OnPipetteContainerExit(PipetteContainer pipetteContainer) {
        if (GeneralItem.ObjectType == ObjectType.Bottle || GeneralItem is AgarPlateBottom || GeneralItem.ObjectType == ObjectType.Medicine || GeneralItem.ObjectType == ObjectType.PumpFilterTank) {
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
