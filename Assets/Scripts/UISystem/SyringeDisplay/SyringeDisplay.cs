using TMPro;
using UnityEngine;

public class SyringeDisplay : MonoBehaviour {

    #region Fields
    [SerializeField]
    GameObject textObject;
    GameObject followedObject;
    Transform textParentTransform;
    GameObject cam;
    LiquidContainer container;
    TextMeshPro textField;
    bool liquidPresent = false;
    #endregion

    #region Constants
    private const string VOLUME = "ml";
    #endregion

    public void SetFollowedObject(GameObject follow) {
        followedObject = follow;

        Syringe syringe = followedObject.GetComponent<Syringe>();
        if (syringe != null) {
            container = syringe.Container;
            liquidPresent = true;
            return;
        }

        Pipette pipette = follow.GetComponent<Pipette>();
        if (pipette != null) {
            container = pipette.Container;
            liquidPresent = true;
            return;
        }
    }

    void Start() {
        textParentTransform = textObject.transform.parent.transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        textField = textObject.GetComponent<TextMeshPro>();
    }

    void Update() {
        if (followedObject != null) {
            transform.position = followedObject.transform.position;
        } else {
            Destroy(gameObject);
        }
        textParentTransform.LookAt(cam.transform, Vector3.up);
        if (liquidPresent) {
            double contAmount = (double)container.Amount / 1000;

            textField.text = contAmount.ToString("F3") + "/" + ((double)container.Capacity / 1000) + VOLUME;
        }
    }
}
