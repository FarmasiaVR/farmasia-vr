using UnityEngine;

public class SyringeCapBag : GeneralItem {

    protected override void Start() {
        base.Start();
        ObjectType = ObjectType.SyringeCapBag;
        Type.On(InteractableType.Interactable);
    }

    public void DisableSyringeCapBag() {
        gameObject.GetComponent<SyringeCapBag>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void EnableSyringeCapBag() {
        gameObject.GetComponent<SyringeCapBag>().enabled = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
