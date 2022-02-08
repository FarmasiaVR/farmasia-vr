using UnityEngine;
using System.Collections;
using System;

public class WritingOption : DragAcceptable {

    [SerializeField]
    private string optionName = "valinta";

    [SerializeField]
    private Material selectedMaterial;
    [SerializeField]
    private Material deselectedMaterial;

    [SerializeField]
    private GameObject boxObject;

    private bool selected;

    public Action<string> onSelect;
    public Action<string> onDeselect;

    public override void Interact(Hand hand) {
        base.Interact(hand);

        selected = !selected;
        if (selected)
            onSelect(optionName);
        else
            onDeselect(optionName);

        UpdateMaterial();
    }

    public void Reset() {
        selected = false;
    }

    private void UpdateMaterial() {
        if (selected) {
            boxObject.GetComponent<MeshRenderer>().material = selectedMaterial;
        } else {
            boxObject.GetComponent<MeshRenderer>().material = deselectedMaterial;
        }
    }
}
