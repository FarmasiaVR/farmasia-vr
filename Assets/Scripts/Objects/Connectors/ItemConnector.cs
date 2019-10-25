using UnityEngine;

public abstract class ItemConnector {

    public Transform Object { get; private set; }

    public ItemConnector(Transform obj) {
        this.Object = obj;
    }

    public abstract void ConnectItem(Interactable interactable, int options);

    public abstract void ReleaseItem(int options);
}