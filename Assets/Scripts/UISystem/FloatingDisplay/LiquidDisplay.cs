using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;

public class LiquidDisplay : Display {

    LiquidContainer container;
    TextMeshPro textField;
    bool liquidPresent = false;

    private StringBuilder stringBuilder = new StringBuilder();

    private const string VOLUME = "ml";

    private Color originalColor;

    public float resetDelay = 1.5f;
    public float shakeIntensity = 0.005f;

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
            if (container == null) Logger.Warning(pipette);
            liquidPresent = true;
            return;
        }

        PipetteContainer pipetteContainer = follow.GetComponent<PipetteContainer>();
        if (pipetteContainer != null) {
            container = pipetteContainer.Container;
            liquidPresent = true;
            return;
        }

        SyringeNew syringeNew = follow.GetComponent<SyringeNew>();
        if (syringeNew != null) {
            container = syringeNew.Container;
            liquidPresent = true;
            return;
        }

        Logger.Warning(gameObject + " did not have a container");
    }

    new void Start() {
        base.Start();
        stringBuilder.EnsureCapacity(16);
        textField = base.textObject.GetComponent<TextMeshPro>();
        originalColor = textField.color;
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

    // Change the color of the text and shake if pipette container is full (Used as a warning to not break automatic pipette/pipettor)
    public void ExceededCapacity() {
        // Debug.Log("\nEXCEEDED CAPACITY DISPLAY\n");
        textField.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        StartCoroutine(ResetColorAfterDelay());
        StartCoroutine(TextJitter());
    }

    private IEnumerator ResetColorAfterDelay() { 
        yield return new WaitForSeconds(resetDelay);
        textField.color = originalColor;
    }

    private IEnumerator TextJitter() {
        Vector3 originalPosition = textField.transform.localPosition;
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            // Apply a small offset to the text position
            textField.transform.localPosition = originalPosition + (Vector3)(Random.insideUnitCircle * shakeIntensity);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset position
        textField.transform.localPosition = originalPosition;
    }
}
