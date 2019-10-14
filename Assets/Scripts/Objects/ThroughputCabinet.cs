using System.Collections.Generic;
using UnityEngine;

public class ThroughputCabinet : MonoBehaviour {

    #region Fields
    private List<Transform> objects;
    #endregion

    public void TransferObjectsTo(ThroughputCabinet other) {
        if (other == null) {
            return;
        }

        other.Clear();
        Transform dstCabinet = other.transform;
        Transform srcCabinet = transform;
        foreach (Transform obj in objects) {
            Vector3 pos = dstCabinet.position + (obj.position - srcCabinet.position);
            float rotDelta = Quaternion.Angle(srcCabinet.rotation, dstCabinet.rotation);

            GameObject instance = Instantiate(obj.gameObject, pos, obj.rotation);
            instance.transform.RotateAround(dstCabinet.position, dstCabinet.up, rotDelta);
        }
    }

    public void Clear() {
        foreach (Transform t in objects) {
            Destroy(t.gameObject);
        }
        objects.Clear();
    }

    private void Start() {
        objects = new List<Transform>();
    }

    private void OnTriggerEnter(Collider other) {
        SaveObject(other.transform);
    }

    private void OnTriggerExit(Collider other) {
        ForgetObject(other.transform);
    }

    private void SaveObject(Transform obj) {
        Logger.Print("Saving obj");
        if (obj.GetComponent<GeneralItem>() != null && !objects.Contains(obj)) {
            objects.Add(obj);
        }
    }

    private void ForgetObject(Transform obj) {
        objects.Remove(obj);
    }
}
