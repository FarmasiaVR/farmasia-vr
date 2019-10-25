using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR;

public class Hand : MonoBehaviour {

    #region fields
    public bool IsGrabbed { get => Connector.IsGrabbed; }
    public bool IsClean { get; set; }

    public SteamVR_Input_Sources HandType { get; private set; }

    private VRHandControls controls;

    public HandCollider coll { get; private set; }

    public Interactable Interactable { get; private set; }

    [SerializeField]
    private Hand other;
    public Hand Other { get => other; }

    public HandConnector Connector { get; private set; }

    public Transform Offset { get; private set; }

    public VRPadSwipe Swipe { get; private set; }
    #endregion

    private void Start() {
        HandType = GetComponent<VRHandControls>().handType;
        coll = transform.GetChild(0).GetComponent<HandCollider>();
        controls = GetComponent<VRHandControls>();
        Connector = new HandConnector(transform);

        Assert.IsNotNull(other, "Other hand was null");

        Offset = transform.Find("Offset");
    }

    private void Update() {
        UpdateControls();
    }

    private void UpdateControls() {

        // Grabbing
        if (VRInput.GetControlDown(HandType, Controls.Grab)) {
            InteractWithObject();
        }
        if (VRInput.GetControlUp(HandType, Controls.Grab)) {
            UninteractWithObject();
        }

        // Interacting
        if (VRInput.GetControlDown(HandType, Controls.GrabInteract)) {
            GrabInteract();
        }
        if (VRInput.GetControlUp(HandType, Controls.GrabInteract)) {
            GrabUninteract();
        }

        if (IsGrabbed && Interactable != null) {
            Interactable.UpdateInteract(this);
        }
    }


    #region Interaction
    public void InteractWithObject() {

        Events.FireEvent(EventType.InteractWithObject, CallbackData.Object(this));

        if (IsGrabbed) {
            Connector.ReleaseItem(0);
            return;
        }

        Interactable = coll.GetGrab();
        if (Interactable == null) {
            return;
        }

        if (Interactable.Type == InteractableType.Grabbable) {

            Offset.position = Interactable.transform.position;
            Offset.rotation = Interactable.transform.rotation;

            Connector.ConnectItem(Interactable, 0);

            //if (Interactable.State == InteractState.LuerlockAttatch) {

            //    var pair = Interactable.Interactors.LuerlockPair;

            //    pair.Value.Connector.ConnectItem(Interactable, pair.Key);
            //} else {
            //    Connector.ConnectItem(Interactable, 0);
            //}

        } else if (Interactable.Type == InteractableType.Interactable) {
            Interactable.Interact(this);
        }
    }

    public void UninteractWithObject() {

        Events.FireEvent(EventType.UninteractWithObject, CallbackData.Object(this));

        if (IsGrabbed) {
            if (VRControlSettings.HoldToGrab) {

                Connector.ReleaseItem(0);

                //if (Interactable.State == InteractState.LuerlockAttatch) {

                //    var pair = Interactable.Interactors.LuerlockPair;

                //    if (pair.Value == null) {
                //        throw new System.Exception("Interacting luerlock was null, key: " + pair.Key);
                //    }

                //    pair.Value.Connector.ReleaseItem(pair.Key);
                //} else {
                //    Connector.ReleaseItem(0);
                //}
            }
        } else if (Interactable != null) {
            Interactable.Uninteract(this);
            Interactable = null;
        }
    }

    public void GrabInteract() {

        Events.FireEvent(EventType.GrabInteractWithObject, CallbackData.Object(this));

        if (!IsGrabbed) {
            return;
        }

        //  Interactable = coll.GetGrab();

        if (Interactable == null) {
            Logger.Warning("Tryying to interact with null objet");
            UninteractWithObject();
        }

        if (Interactable.Type.AreOn(InteractableType.Grabbable, InteractableType.Interactable)) {
            Interactable.Interact(this);
        }
    }

    public void GrabUninteract() {

        Events.FireEvent(EventType.GrabUninteractWithObject, CallbackData.Object(this));

        if (!IsGrabbed) {
            return;
        }

        //Interactable = coll.GetGrab();

        if (Interactable == null) {
            Logger.Warning("Tryying to interact with null objet");
            UninteractWithObject();
        }

        if (Interactable.Type.AreOn(InteractableType.Grabbable, InteractableType.Interactable)) {
            Interactable.Uninteract(this);
        }
    }
    #endregion

    private void OnJointBreak(float breakForce) {
        Logger.Print("Joint force broken: " + breakForce);
        Connector.ReleaseItem(0);
    }

    public static Hand GrabbingHand(Rigidbody rb) {
        foreach (VRHandControls controls in VRInput.Hands) {
            if (rb == controls.Hand.Connector.GrabbedRigidbody) {
                return controls.Hand;
            }
        }

        return null;
    }
}
