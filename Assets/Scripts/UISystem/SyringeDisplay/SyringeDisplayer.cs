using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyringeDisplayer : MonoBehaviour {
    #region Fields
    [SerializeField]
    GameObject handLeft, handRight;
    [SerializeField]
    private GameObject liquidDisplay;
    private Hand left, right;
    private GameObject displayLeft, displayRight;
    private bool leftChecked, rightChecked = false;
    #endregion

    private void Start() {
        left = handLeft.GetComponent<Hand>();
        right = handRight.GetComponent<Hand>();
    }

    private void Update() {
        CheckHandGrab(ref left, ref leftChecked, ref displayLeft);
        CheckHandGrab(ref right, ref rightChecked, ref displayRight);
    }

    private void CheckHandGrab(ref Hand hand, ref bool check, ref GameObject display) {
        if (hand.IsGrabbed) {
            if (!check) {
                GameObject obj = hand.Connector.Connection.gameObject;
                obj = Interactable.GetInteractableObject(obj.transform);
                GeneralItem item = obj.GetComponent<GeneralItem>();
                if (item.ObjectType == ObjectType.Syringe) {
                    InstantiateSyringeDisplay(ref display, obj);
                }
                check = true;
            }
        } else {
            check = false;
            if (display != null) {
                Destroy(display);
            }
        }
    }

    private void InstantiateSyringeDisplay(ref GameObject reference, GameObject followedObject) {
        reference = Instantiate(liquidDisplay);
        SyringeDisplay display = reference.GetComponent<SyringeDisplay>();
        display.setFollowedObject(followedObject);
    }

}
