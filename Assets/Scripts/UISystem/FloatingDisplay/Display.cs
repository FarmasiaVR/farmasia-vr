using UnityEngine;
using TMPro;

public class Display : MonoBehaviour {

    [SerializeField]
    protected GameObject textObject;
    private GameObject followedObject;
    private Transform textParentTransform;
    private GameObject cam;

    public virtual void SetFollowedObject(GameObject follow) {
        followedObject = follow;
    }

    protected void Start() {
        textParentTransform = textObject.transform.parent.transform; // the fuck..?
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    protected void Update() {
        if (followedObject != null) {
            transform.position = followedObject.transform.position;
        }
        textParentTransform.LookAt(cam.transform, Vector3.up);
    }
}
