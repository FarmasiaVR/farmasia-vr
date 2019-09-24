using UnityEngine;

public class TestContaminable : Interactable {

    #region fields
    private GeneralItem states;
    #endregion

    protected override void Start() {
        base.Start();
        SetFlags(true, InteractableType.Grabbable, InteractableType.Interactable);

        states = GetComponent<GeneralItem>();
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.PrintVariables("Clean", states.GetFlag(ItemState.Status.Clean));
    }

    private void OnCollisionEnter(Collision coll) {
        GameObject collisionObject = coll.gameObject;
        if (collisionObject.tag == "Floor") {
            states.SetFlags(false, ItemState.Status.Clean);
        }
        Logger.Print("Contaminable hit something");
    }
}
