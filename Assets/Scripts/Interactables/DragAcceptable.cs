using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DragAcceptable : Interactable {
    #region Fields
    private float defaultForce = 2500;
    private float mass;
    private float activateDistance = 0.25f;
    private float brakeAmount = 10;
    protected Vector3 startPos;
    private float destroyTime = 0.5f;
    private Transform pCamera;
    public bool Activated { get; protected set; } = false;
    public bool Disabled { get; set; } = false;

    [SerializeField]
    private bool lookAtPlayer;

    protected bool grabbed;
    private Transform hand;

    private float Distance {
        get {
            return Vector3.Distance(hand.position, startPos);
        }
    }
    #endregion

    protected override void Awake() {
        base.Awake();

        Type.On(InteractableType.Interactable);
    }
    protected override void Start() {
        base.Awake();
        Initialize();
    }

    public void Initialize() {
        mass = Rigidbody.mass;
        Type.On(InteractableType.Interactable, InteractableType.Draggable);

        pCamera = Player.Camera.transform;

        startPos = transform.position;
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
    protected abstract void Activate();

    protected void SafeDestroy() {

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

        Logger.Print("Drag acceptable interact");

        grabbed = true;
        this.hand = hand.transform;
    }
    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);

        Logger.Print("Drag acceptable uninteract");

        grabbed = false;
    }
}
