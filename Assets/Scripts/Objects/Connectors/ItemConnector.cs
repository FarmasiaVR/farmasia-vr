using UnityEngine;

public abstract class ItemConnector {

    public Transform Object { get; private set; }

    public abstract ItemConnection Connection { get; set; }

    public ItemConnector(Transform obj) {
        this.Object = obj;
    }

    public abstract void ConnectItem(Interactable interactable);

    public abstract void OnReleaseItem();
}