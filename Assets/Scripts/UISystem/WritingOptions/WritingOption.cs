using UnityEngine;
using System.Collections;
using System;

public class WritingOption : DragAcceptable {

    [SerializeField]
    public string OptionText = "valinta";

    public WritingType WritingType;

    [SerializeField]
    private Material selectedMaterial;
    
    [SerializeField]
    private Material deselectedMaterial;

    [SerializeField]
    private GameObject boxObject;

    private bool selected;

    public Action<WritingOption> onSelect;
    public Action<WritingOption> onDeselect;

    public override void Interact(Hand hand) {
        base.Interact(hand);

        selected = !selected;
        if (selected)
            onSelect(this);
        else
            onDeselect(this);

        // UpdateMaterial();
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
