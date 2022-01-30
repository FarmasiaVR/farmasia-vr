using UnityEngine;

public class MeshHider : MonoBehaviour {

    [SerializeField]
    private bool disable;

    [SerializeField]
    private bool onlyHide;

    [SerializeField]
    private bool invertFunctionality;

    [SerializeField]
    private Material testHandMaterial;

    private void Awake() {
        if (disable) {
            return;
        }
#if UNITY_NONVRCOMPUTER
        if (testHandMaterial != null) {
            Renderer r = GetComponent<Renderer>();
            if (r != null) {
               r.material = testHandMaterial;
            }
        }
#else
        if (invertFunctionality) {
            GetComponent<Renderer>().enabled = true;
        } else if (onlyHide) {
            GetComponent<Renderer>().enabled = false;
        } else {
            Destroy(GetComponent<Renderer>());
            Destroy(GetComponent<MeshFilter>());
        }

        Destroy(this);
#endif
    }
}
