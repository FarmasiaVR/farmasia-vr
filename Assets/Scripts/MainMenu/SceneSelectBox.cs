using UnityEngine;
using UnityEngine.Events;

public class SceneSelectBox : Interactable {
    [SerializeField]
    private bool isInstant;
    [SerializeField]
    GameObject liquid;
    ProgressBar bar;
    private bool activated;

    public delegate void OnAcceptCallback();

    [SerializeField]
    private OnAcceptCallback OnAccept;

    [SerializeField]
    private UnityEvent onActivate;

    protected override void Start() {
        base.Start();
        Type.Set(InteractableType.Interactable);
        bar = liquid.GetComponent<ProgressBar>();
    }

    private void Update() {
        if (bar.Done && !activated) {
            activated = true;
            onActivate?.Invoke();
        }
    }

    public override void Interact(Hand hand) {
        base.Interact(hand);
        if (isInstant) {
            onActivate?.Invoke();
        } else {
            bar.grabbing = true;
        }
    }

    public override void Uninteract(Hand hand) {
        base.Uninteract(hand);
        bar.grabbing = false;
    }
}
