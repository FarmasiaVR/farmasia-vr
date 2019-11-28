using TMPro;
using UnityEngine;

public class SyringeDisplay : MonoBehaviour {

    #region Fields
    [SerializeField]
    GameObject textObject;
    GameObject followedObject;
    Transform textParentTransform;
    GameObject cam;
    Syringe syringe;
    LiquidContainer container;
    TextMeshPro textField;
    bool liquidPresent = false;
    #endregion

    #region Constants
    private const string VOLUME = "ml";
    #endregion

    public void setFollowedObject(GameObject follow) {
        followedObject = follow;
        syringe = followedObject.GetComponent<Syringe>();
        container = syringe.Container;
        liquidPresent = true;
    }

    void Start() {
        textParentTransform = textObject.transform.parent.transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        textField = textObject.GetComponent<TextMeshPro>();
    }

    void Update() {
        //transform.LookAt(cam.transform, Vector3.up);
        if (followedObject != null) {
            transform.position = followedObject.transform.position;
        } else {
            Logger.Warning("No object to follow in SyringeDisplay, Destroying!");
            Destroy(gameObject);
        }
        textParentTransform.LookAt(cam.transform, Vector3.up);
        if (liquidPresent) {
            textField.text = container.Amount + "/" + container.Capacity + VOLUME;
        }
    }
}
