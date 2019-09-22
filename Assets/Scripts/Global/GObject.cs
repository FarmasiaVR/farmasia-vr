using UnityEngine;

public class GObject : MonoBehaviour {

    private void Update() {
        G.Instance.ProgressManager.Update(Time.deltaTime);
    }
}