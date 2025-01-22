using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{

    private Vector3 savedPosition;
    private ConfigurableJoint joint;
    public GameObject connectedBodyObject;
    private Rigidbody connectedBody;

    public GameObject nozzleAttach;
    private Transform attach;

    private void Awake()
    {
        joint = GetComponent<ConfigurableJoint>();
        connectedBody = connectedBodyObject.GetComponent<Rigidbody>();
        attach = nozzleAttach.GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        savedPosition = attach.position;
        transform.position = savedPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Grab()
    {
        savedPosition = attach.position;
        joint.connectedBody = connectedBody;

    }

    public void Release()
    {
        transform.position = savedPosition;
        joint.connectedBody = null;
    }
}
