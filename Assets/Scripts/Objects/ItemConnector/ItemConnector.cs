using UnityEngine;

public abstract class ItemConnector {

    public Component Component;

    public ItemConnector(Component component) {
        this.Component = component;
    }

    public abstract void AttachItem();

    public abstract void ReleaseItem();
}