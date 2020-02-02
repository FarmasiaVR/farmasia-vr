using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAcceptable : Interactable {

    #region Fields
    private float defaultForce = 2500;
    private float mass;
    private float activateDistance = 0.1f;
    private float brakeAmount = 10;
    protected Vector3 startPos;
    private float destroyTime = 0.5f;
    private Transform pCamera;
    public int ActivateCount { get; private set; }
    public int ActivateCountLimit { get; set; } = -1;
    private bool disabled;
    public bool Disabled {
        get {
            return disabled | Hidden;
        }
        set {
            disabled = value;
        }
    }
    public bool Hidden { get; set; } = false;

    [SerializeField]
    private bool lookAtPlayer;
    public bool LookAtPlayer { get => lookAtPlayer; set => lookAtPlayer = value; }

    public bool ReleaseAfterActivate { get; set; }

    protected bool grabbed;
    private Transform hand;

    private float Distance {
        get {
            return Vector3.Distance(hand.position, startPos);
        }
    }

    public delegate void OnAcceptCallback();
    public OnAcceptCallback OnAccept { get; set; }
    #endregion

    protected override void Awake() {
        base.Awake();
    }
    protected override void Start() {
        base.Start();
        Initialize();
    }

    public void Initialize() {
        mass = Rigidbody.mass;
        Type.On(InteractableType.Interactable, InteractableType.Draggable);

        pCamera = Player.Camera.transform;

        startPos = transform.position;
    }

    public void Hide(bool hide) {
        Release();

        Hidden = hide;
        GetComponent<Collider>().isTrigger = hide;
        GetComponent<Renderer>().enabled = !hide;
        foreach (Transform child in transform) {
            var r = child.GetComponent<Renderer>();
            r.enabled = !hide;
        }
    }

    #region Updating
    protected virtual void Update() {

        if (Disabled) {
            return;
        }

        if (grabbed) {
            UpdateGrab();
            CheckActivate();
        } else {
            UpdatePosition();
            Brake();
        }

        if (lookAtPlayer) {
            transform.LookAt(pCamera);
        }
    }

    private void UpdateGrab() {
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.MovePosition(LerpedPosition());
    }
    private Vector3 LerpedPosition() {
        Vector3 direction = (hand.position - startPos).normalized;
        return startPos + direction * Distance * 0.5f;
    }

    private void UpdatePosition() {

        Vector3 direction = startPos - transform.position;
        if (direction.magnitude > 1) {
            direction = direction.normalized;
        }

        Rigidbody.AddForce(direction * defaultForce * mass * Time.deltaTime);
    }
    private void Brake() {
        Rigidbody.velocity = Rigidbody.velocity - Rigidbody.velocity * Time.deltaTime * brakeAmount;
    }
    private void CheckActivate() {
        if (Distance > activateDistance) {
            Activate();
        }
    }
    #endregion

    #region Activating
    protected virtual void Activate() {
        if (ActivateCount >= ActivateCountLimit && ActivateCountLimit >= 0) {
            return;
        }

        if (ReleaseAfterActivate) {
            Release();
        }
        OnAccept?.Invoke();
        ActivateCount++;
    }
    private void Release() {
        if (IsGrabbed) {
            Hand.GrabbingHand(this)?.Uninteract();
        }
    }

    public void SafeDestroy() {

        StartCoroutine(DestroyCoroutine());

        IEnumerator DestroyCoroutine() {

            float time = destroyTime;

            Vector3 originalScale = transform.localScale;

            while (time > 0) {
                time -= Time.deltaTime;

                float factor = time / destroyTime;

                transform.localScale = originalScale * factor;

                yield return null;
            }

            DestroyInteractable();
        }
    }
    #endregion

    public override void Interact(Hand hand) {
        base.Interact(hand);

        grabbed = true;
        this.hand = hand.transform;
    }
    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);

        grabbed = false;
    }
}
