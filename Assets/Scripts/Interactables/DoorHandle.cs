using UnityEngine;
public class DoorHandle : Interactable {

    #region Fields
    private Hand hand;
    private OpenableDoor door;
    #endregion

    #region Animation Fields

    public enum HandleState { Up, Down, Opening, Closing };
    private HandleState state;
    private float startAngle;
    private float currentAngle = 0;
    private readonly float maxAngle = 45f;
    private float handleSpeed = 200f;
    private Transform handle;
    #endregion

    protected override void Start_Impl() {
        door = transform.parent.GetComponent<OpenableDoor>();
        Type.Set(InteractableType.Interactable);
        handle = transform.GetChild(1).transform;
        if (handle != null) {
            startAngle = handle.eulerAngles.z;
        }
    }

    private void Update() {
        if (State == InteractState.Grabbed) {
            door.SetByHandPosition(hand);
        }
        UpdateHandle();

    }

    private void UpdateHandle() {
        UpdateHandleAnimation();
        CheckHandleState();
        if (handle != null) {
            handle.eulerAngles = new Vector3(handle.eulerAngles.x, handle.eulerAngles.y, startAngle - currentAngle);
        }
    }

    private void CheckHandleState() {
        if (hand == null) {
            if (currentAngle <= 0) {
                state = HandleState.Up;
                currentAngle = 0;
            } else {
                state = HandleState.Closing;
            }
        } else {
            if (currentAngle >= maxAngle) {
                state = HandleState.Down;
                currentAngle = maxAngle;
            } else {
                state = HandleState.Opening;
            }
        }
    }
    private void UpdateHandleAnimation() {
        switch (state) {
            case HandleState.Closing:
                currentAngle -= handleSpeed * Time.deltaTime;
                break;
            case HandleState.Opening:
                currentAngle += handleSpeed * Time.deltaTime;
                break;
            default:
                break;
        }
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Door interact");

        door.SetAngleOffset(hand.ColliderPosition);

        this.hand = hand;
        State.On(InteractState.Grabbed);
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);

        this.hand = null;
        State.Off(InteractState.Grabbed);
        door.ReleaseDoor();
    }
}
