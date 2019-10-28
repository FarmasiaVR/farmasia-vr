using UnityEngine;

public class MeshHider : MonoBehaviour {

    [SerializeField]
    private bool onlyHide;

    [SerializeField]
    private Material testHandMaterial;

    private void Awake() {
#if UNITY_NONVRCOMPUTER
        if (testHandMaterial != null) {
            GetComponent<Renderer>().material = testHandMaterial;
        }
#else
        if (onlyHide) {
            GetComponent<Renderer>().enabled = false;
        } else {
            Destroy(GetComponent<Renderer>());
        }

        Destroy(this);
#endif
    }
}
