using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour {

    #region fields
    private static string iTag = "Interactable";

    public EnumBitField<InteractableType> Type { get; protected set; } = new EnumBitField<InteractableType>();

    public EnumBitField<InteractState> State { get; private set; } = new EnumBitField<InteractState>();

    private Rigidbody rb;

    // CAN'T BE A PROPERTY
    public Interactors Interactors;
    #endregion

    protected virtual void Start() {
        gameObject.AddComponent<ObjectHighlight>();
        gameObject.AddComponent<ItemPlacement>();

        gameObject.tag = iTag;
    }

    public virtual void Interact(Hand hand) {
    }
    public virtual void Interacting(Hand hand) {
    }
    public virtual void Uninteract(Hand hand) {
    }

    public virtual void UpdateInteract(Hand hand) {
    }

    public static Interactable GetInteractable(Transform t) {
        return GetInteractableObject(t)?.GetComponent<Interactable>();
    }
    public static GameObject GetInteractableObject(Transform t) {
        while (t != null) {
            if (t.tag == iTag) {
                return t.gameObject;
            }

            t = t.parent;
        }
        return null;
    }

    public Rigidbody Rigidbody {
        get {

            if (rb == null) {
                rb = GetComponent<Rigidbody>();
            }

            return rb;
        }
    }

    public void DestroyInteractable() {

        if (Interactors.Hand != null) {
            Interactors.Hand.ReleaseObject();
        }
        // Could cause problems, need to verify that Interactors are nullified when releasing from hand, bottle or luerlock
        if (Interactors.LuerlockPair.Value != null) {
            Interactors.LuerlockPair.Value.GetConnector(Interactors.LuerlockPair.Key).ReleaseItem();
        }
        
        IEnumerator DestroySequence() {
            transform.position = new Vector3(10000, 10000, 10000);
            yield return null;
            yield return null;
            Destroy(gameObject);
        }

        StartCoroutine(DestroySequence());
    }

    public static implicit operator Interactable(GameObject g) {

        if (g == null) {
            throw new System.Exception("Gameobject was null");
        }

        Interactable i = g.GetComponent<Interactable>();

        if (i == null) {
            throw new System.Exception("Interactable not found, use method Interactable.GetInteractable() instead");
        }

        return i;
    }
}