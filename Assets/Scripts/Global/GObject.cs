using UnityEngine;

public class GObject : MonoBehaviour {

    private void Update() {
        G.Instance.Update(Time.deltaTime);
    }
}