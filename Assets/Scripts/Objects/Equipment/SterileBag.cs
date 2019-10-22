using System.Collections.Generic;
using UnityEngine;

public class SterileBag : MonoBehaviour {

    #region Fields
    public List<GameObject> objectsInBag;
    public bool IsClosed { get; private set; }
    public bool IsSterile { get; private set; }
    #endregion
    
    // Start is called before the first frame update
    void Start() {
        objectsInBag = new List<GameObject>();
        IsClosed = false;
        IsSterile = true;
    }

    private void OnTriggerEnter(Collider other) {
        GeneralItem item = GeneralItem.Find(other.transform);
        if (item == null || IsClosed) {
            return;
        }
        
        if (!objectsInBag.Contains(other.gameObject)) {
            objectsInBag.Add(other.gameObject);
            if (!item.IsClean) {
                IsSterile = false;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        GameObject foundObject = other.gameObject;
        GeneralItem item = GeneralItem.Find(other.transform);
        if (!IsClosed && item != null) {
            objectsInBag.Remove(foundObject);
        }
    }
}