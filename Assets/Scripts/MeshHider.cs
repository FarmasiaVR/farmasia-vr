using UnityEngine;

public class MeshHider : MonoBehaviour {

    [SerializeField]
    private bool onlyHide;

    [SerializeField]
    private Material testHandMaterial;

    private void Awake() {
#if !UNITY_NONVRCOMPUTER
        if (!onlyHide) {
            Destroy(GetComponent<Renderer>());
        } else {
            GetComponent<Renderer>().enabled = false;
        }

        Destroy(this);
#else
        if (testHandMaterial) {
            GetComponent<Renderer>().material = testHandMaterial;
        }
#endif
    }
}
