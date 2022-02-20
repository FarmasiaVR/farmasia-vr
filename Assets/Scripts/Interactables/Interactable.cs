using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class Interactable : MonoBehaviour {

    #region fields
    public static string iTag = "Interactable";

    public EnumBitField<InteractableType> Type { get; protected set; } = new EnumBitField<InteractableType>();

    public EnumBitField<InteractState> State { get; set; } = new EnumBitField<InteractState>();

    public RigidbodyContainer RigidbodyContainer { get; private set; }
    public Rigidbody Rigidbody {
        get {
            if (RigidbodyContainer.Enabled) {
                return RigidbodyContainer.Rigidbody;
            } else {
                return null;
            }
        }
    }

    public bool IsInteracting;

    // CAN'T BE A PROPERTY
    public Interactors Interactors;

    [SerializeField]
    private bool disableHighlighting;

    private ObjectHighlight highlight;

    public bool Destroyed { get; private set; }
    #endregion

    protected virtual void Awake() {
        RigidbodyContainer = new RigidbodyContainer(this);
    }

    protected virtual void Start() {
        if (gameObject.GetComponent<ObjectHighlight>() == null) {
            gameObject.AddComponent<ObjectHighlight>().DisableHighlighting(disableHighlighting);

        }

        gameObject.tag = iTag;
    }


    public virtual void Interact(Hand hand) { }
    public virtual void OnGrab(Hand hand) { }
    public virtual void OnGrabStart(Hand hand) {
        IsInteracting = true;
    }
    public virtual void Uninteract(Hand hand) { }
    public virtual void OnGrabEnd(Hand hand) {
        IsInteracting = false;
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

    public void DestroyInteractable() {
        if (Destroyed) {
            return;
        }

        Destroyed = true;

        if (this.enabled && gameObject.activeInHierarchy) {
            StartCoroutine(DestroySequence());
        } else {
            PrepareItemForRemoval();
            Destroy(gameObject);
        }

        IEnumerator DestroySequence() {
            
            // Could cause problems, need to verify that Interactors are nullified when releasing from hand, bottle or luerlock

            PrepareItemForRemoval();

            transform.position = new Vector3(10000, 10000, 10000);
            yield return null;
            yield return null;

            //Logger.Print("Destroy interactable " + this.name);
            Destroy(gameObject);
        }
    }
    private void PrepareItemForRemoval() {
        if (IsGrabbed) {
            Interactors.Hand.Uninteract();
        }

        if (Interactors.LuerlockPair.Value != null) {
            Interactors.LuerlockPair.Value.GetConnector(Interactors.LuerlockPair.Key).Connection.Remove();
        }
        //if (this as GeneralItem is var item && item != null) {
        //    if (item.ObjectType == ObjectType.Luerlock && item as LuerlockAdapter is var l) {
        //        Logger.Print("Removing luerlock items: " + l.AttachedInteractables.Count);
        //        l.LeftConnector.AttachedInteractable?.DestroyInteractable();
        //        l.RightConnector.AttachedInteractable?.DestroyInteractable();
        //    } else if (item.ObjectType == ObjectType.Needle && item as Needle is var n ) {
        //        n.Connector.AttachedInteractable?.DestroyInteractable();
        //    }
        //}
    }

    protected virtual void OnDestroy() {
        if (!Destroyed && gameObject.activeInHierarchy) {
            Logger.Error(string.Format("Active Interactables must be destroyed using Interactable.DestroyInteractable method. Destroyed interactable: {0}", this.name));
        }
    }

    public bool IsAttached {
        get {
            return State == InteractState.LuerlockAttached || State == InteractState.NeedleAttached || State == InteractState.LidAttached || State == InteractState.PumpFilterAttached;
        }
    }

    public bool IsGrabbed {
        get {
            return State == InteractState.Grabbed;
        }
    }

    public bool IsOnFloor {
        get {
            return State == InteractState.OnFloor;
        }
    }

    public ObjectHighlight Highlight {
        get {
            if (highlight == null) {
                highlight = GetComponent<ObjectHighlight>();
            }
            return highlight;
        }
    }
}