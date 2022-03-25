using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class FilteringButton : Interactable {

    private bool running = false;

    private UnityEvent onActivate;

    private Transform FilterTank;
    private LiquidContainer Container;

    protected override void Start() {
        base.Start();

        FindTank();
        Container = LiquidContainer.FindLiquidContainer(FilterTank);


        Type.Set(InteractableType.Interactable);
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);

        if(!running){
            running = true;
            Logger.Print("Pump ON");
            RemoveLiquid();

        } else {
            running = false;
            Logger.Print("Pump OFF");
        }
    }

    void FindTank() {
        // find pump
        Transform ButtonBase = this.transform.parent;
        Transform PumpBottom = ButtonBase.transform.parent;
        Transform Pump = PumpBottom.transform.parent;

        // find tanks liquid container
        FilterTank = Pump.transform.GetChild(3);
    }

    void RemoveLiquid() {
        
        transform.parent.GetComponentInChildren<AudioSource>().Play();

        int step = Container.Amount / 5;
        StartCoroutine(Wait(step));  
        
    }

    IEnumerator Wait(int step) {
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

        Events.FireEvent(EventType.FilterLiquid);
    }


}