using UnityEngine;

public class MeshHider : MonoBehaviour {

    [SerializeField]
    private bool onlyHide;

    private void Awake() {

        if (!onlyHide) {
            Destroy(GetComponent<Renderer>());
        } else {
            GetComponent<Renderer>().enabled = false;
        }

        Destroy(this);
    }
}
