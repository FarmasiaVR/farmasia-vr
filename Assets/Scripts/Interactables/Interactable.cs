using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour {

    #region fields
    private static string iTag = "Interactable";

    public EnumBitField<InteractableType> Type { get; protected set; } = new EnumBitField<InteractableType>();

    public EnumBitField<InteractState> State { get; private set; } = new EnumBitField<InteractState>();

    public RigidbodyContainer RB { get; private set; }
    public Rigidbody Rigidbody {
        get {
            if (RB.Enabled) {
                return RB.Rigidbody;
            } else {
                Logger.Warning("Accessing rigidbody while disabled");
                return null;
            }
        }
    }

    // CAN'T BE A PROPERTY
    public Interactors Interactors;
    #endregion

    private void Awake() {
        RB = new RigidbodyContainer(this);
        Awake_Interactable();
    }

    private void Start() {
        gameObject.AddComponent<ObjectHighlight>();
        gameObject.AddComponent<ItemPlacement>();
        gameObject.tag = iTag;
        Start_Interactable();
    }

    protected virtual void Awake_Interactable() {}
    protected virtual void Start_Interactable() {}
    

    public virtual void Interact(Hand hand) {}
    public virtual void Interacting(Hand hand) {}
    public virtual void Uninteract(Hand hand) {}
    public virtual void UpdateInteract(Hand hand) {}

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

    public void DestroyInteractable() {

        if (Interactors.Hand != null) {
            Interactors.Hand.Uninteract();
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