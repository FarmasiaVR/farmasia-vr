using TMPro;
using UnityEngine;

public class LiquidDisplay : Display {

    LiquidContainer container;
    TextMeshPro textField;
    bool liquidPresent = false;
    
    private const string VOLUME = "ml";

    public override void SetFollowedObject(GameObject follow) {
        base.SetFollowedObject(follow);

        // Workaround, check Syringe and Pipette. Later, they hopefully both belong to a base class

        Syringe syringe = follow.GetComponent<Syringe>();
        if (syringe != null) {
            container = syringe.Container;
            liquidPresent = true;
            return;
        }

        Pipette pipette = follow.GetComponent<Pipette>();
        if (pipette != null) {
            container = pipette.Container;
            liquidPresent = true;
            return;
        }

    }

    new void Start() {
        base.Start();
        textField = base.textObject.GetComponent<TextMeshPro>();
    }

    new void Update() {
        base.Update();
        if (liquidPresent) {
            double contAmount = (double)container.Amount / 1000;

            textField.text = contAmount.ToString("F3") + "/" + ((double)container.Capacity / 1000) + VOLUME;
        }
    }
}
