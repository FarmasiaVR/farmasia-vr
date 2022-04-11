using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class FilteringButton : Interactable {

    private bool running = false;

    [SerializeField]
    private GameObject pump;
    private LiquidContainer Container;

    private FilterPart basePart;

    protected override void Start() {
        base.Start();

        Type.Set(InteractableType.Interactable);

        Events.SubscribeToEvent(OnPumpFilterAttach, EventType.AttachFilter);
        Events.SubscribeToEvent(OnFilterAssemble, EventType.FilterAssembled);
    }

    private void OnPumpFilterAttach(CallbackData data) {
        // check if base got attached to this pump
        if (pump.GetComponent<Pump>().ConnectedItem?.ObjectType != ObjectType.PumpFilterBase) return;
        basePart = pump.GetComponent<Pump>().ConnectedItem as FilterPart;
        // check if the base has a filter and the filter has a tank
        if ((basePart?.ConnectedItem as FilterPart)?.ConnectedItem?.ObjectType != ObjectType.PumpFilterTank) return;
        var tank = (basePart.ConnectedItem as FilterPart).ConnectedItem as FilterPart;
        // Now we can get the liquidContainer :D
        Container = tank.GetComponentInChildren<LiquidContainer>();
    }

    private void OnFilterAssemble(CallbackData data) {
        // Check if this is filter connected to base or tank connected to filter
        var (bottom, top) = data.DataObject as Tuple<FilterPart, FilterPart>;
        Logger.Print(data.DataObject);
   
        if (bottom.ObjectType != ObjectType.PumpFilterBase && bottom.ObjectType != ObjectType.PumpFilterFilter) return;
        // Now we can reuse the attach callback
        OnPumpFilterAttach(CallbackData.NoData());
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);

        if(!running){
            running = true;
            Logger.Print("Pump ON");
            RunPump();

        }
    }

    void FindTank() {
        // find pump
        Transform ButtonBase = this.transform.parent;
        Transform PumpBottom = ButtonBase.transform.parent;
        Transform Pump = PumpBottom.transform.parent;

        // find tanks liquid container
        //FilterTank = Pump.transform.GetChild(3);
    }

    void RunPump() {
        
        transform.parent.GetComponentInChildren<AudioSource>().Play();

        
        StartCoroutine(RemoveLiquid());  
        
    }

    IEnumerator RemoveLiquid() {
        int step = Container.Amount / 5;
        Container.SetAmount(Container.Amount-step); 
        
        yield return new WaitForSeconds(0.25f);
        Container.SetAmount(Container.Amount-step); 

        yield return new WaitForSeconds(0.25f);
        Container.SetAmount(Container.Amount-step); 

        yield return new WaitForSeconds(0.25f);
        Container.SetAmount(Container.Amount-step); 

        yield return new WaitForSeconds(0.25f);
        Container.SetAmount(Container.Amount-step); 

        running = false;
        Logger.Print("Pump OFF");

        Container.SetAmount(0);
        Container.SetLiquidTypeNone();
        Events.FireEvent(EventType.FilterEmptied);
    }


}