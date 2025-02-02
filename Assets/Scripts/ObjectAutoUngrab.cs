using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectAutoUngrab : MonoBehaviour {
    // This script ungrabs the object it is attached to, when the player hand is further than the parameter below  
    public float ungrabDistance = 0.3f;
    // The distance is calculated from the transform below if set, and otherwise the object's own transform will be used
    public Transform distanceCalcTransform;

    private XRGrabInteractable grabInteractable = null;
    private XRDirectInteractor selectingInteractor = null;
    private bool interactorWaitingForReset = false;

    private void OnEnable() {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null) {
            Debug.LogWarning($"ObjectAutoUngrab in {name} didn't find grab interactable. This script should only be used with gameobjects that have a grab interactable.");
        } else {
            grabInteractable.selectEntered.AddListener(OnSelectEntered);
            grabInteractable.selectExited.AddListener(OnSelectExited);
            // Use attach transform for distance calculations, if it has been manually set
            if (distanceCalcTransform == null)
                distanceCalcTransform = grabInteractable.attachTransform;
        }
    }

    private void OnDisable() {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args) {
        selectingInteractor = args.interactorObject as XRDirectInteractor;
    }

    private void OnSelectExited(SelectExitEventArgs args) {
        if (interactorWaitingForReset)
            selectingInteractor.allowSelect = true;
        selectingInteractor = null;
    }

    private void Update() {
        if (selectingInteractor != null) {
            float distance = Vector3.Distance(distanceCalcTransform.position, selectingInteractor.transform.position);
            if (distance > ungrabDistance) {
                Debug.Log($"Ungrabbed {name}, because selecting interactor went too far from it");
                selectingInteractor.allowSelect = false;
                interactorWaitingForReset = true;
            }
        } 
    }
}
