using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Collections.Generic;

public class FilteringButton : Interactable
{

    private bool running = false;

    [SerializeField]
    private GameObject pump;
    private LiquidContainer Container;

    protected override void Start()
    {
        base.Start();

        Type.Set(InteractableType.Interactable);

        Events.SubscribeToEvent(OnPumpFilterAttach, EventType.AttachFilter);
        Events.SubscribeToEvent(OnFilterAssemble, EventType.FilterAssembled);
    }

    private void OnPumpFilterAttach(CallbackData data)
    {
        Container = transform.parent.parent.GetComponentInChildren<LiquidContainer>();
    }

    private void OnFilterAssemble(CallbackData data)
    {
        // Check if this is filter connected to base or tank connected to filter
        /*var dataObjectList = data.DataObject as List<GeneralItem>;
        if (dataObjectList == null || dataObjectList.Count != 2) return;

        var (bottom, top) = (dataObjectList[0], dataObjectList[1]);
        Logger.Print(data.DataObject);
   
        if (bottom.ObjectType != ObjectType.PumpFilterBase && bottom.ObjectType != ObjectType.PumpFilterFilter) return;
        // Now we can reuse the attach callback
        OnPumpFilterAttach(CallbackData.NoData());*/
    }

    public override void Interact(Hand hand)
    {
        if (G.Instance.Progress.CurrentPackage.doneTypes.Contains(TaskType.WetFilter))
        {
            base.Interact(hand);

            if (!running)
            {
                running = true;
                Logger.Print("Pump ON");
                RunPump();
            }
        }
    }

    void FindTank()
    {
        // find pump
        Transform ButtonBase = this.transform.parent;
        Transform PumpBottom = ButtonBase.transform.parent;
        Transform Pump = PumpBottom.transform.parent;

        // find tanks liquid container
        //FilterTank = Pump.transform.GetChild(3);
    }

    public void RunPump()
    {

        transform.parent.GetComponentInChildren<AudioSource>().Play();
        StartCoroutine(RemoveLiquid());

    }

    IEnumerator RemoveLiquid()
    {
        int step = Container.Amount / 5;
        Container.SetAmount(Container.Amount - step);

        yield return new WaitForSeconds(0.25f);
        Container.SetAmount(Container.Amount - step);

        yield return new WaitForSeconds(0.25f);
        Container.SetAmount(Container.Amount - step);

        yield return new WaitForSeconds(0.25f);
        Container.SetAmount(Container.Amount - step);

        yield return new WaitForSeconds(0.25f);
        Container.SetAmount(Container.Amount - step);

        running = false;
        Logger.Print("Pump OFF");

        Container.SetAmount(0);
        Container.SetLiquidTypeNone();
        Events.FireEvent(EventType.FilterEmptied);
    }


}