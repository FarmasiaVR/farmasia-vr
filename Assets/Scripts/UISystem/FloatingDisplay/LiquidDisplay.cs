using TMPro;
using UnityEngine;
using System.Text;

public class LiquidDisplay : Display {

    LiquidContainer container;
    TextMeshPro textField;
    bool liquidPresent = false;

    private StringBuilder stringBuilder;

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
        stringBuilder.EnsureCapacity(16);
        stringBuilder.Append("12.123/12.123" + VOLUME);
        textField = base.textObject.GetComponent<TextMeshPro>();
    }

    new void Update() {
        base.Update();
        if (liquidPresent) {
            UpdateText();
        }
    }

    private void UpdateText() {
        stringBuilder.Clear();

        double contAmount = 1.0 * container.Amount / 1000;
        stringBuilder.Append(contAmount.ToString("F3"));
        stringBuilder.Append("/");

        double contCapacity = 1.0 * container.Capacity / 1000;
        stringBuilder.Append(contCapacity.ToString("F3"));
        stringBuilder.Append(VOLUME);

        textField.SetText(stringBuilder.ToString());
    }
}
