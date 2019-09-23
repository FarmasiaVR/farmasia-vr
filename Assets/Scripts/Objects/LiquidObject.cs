using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidObject : MonoBehaviour {

    #region fields
    //[SerializeField]
    //private LiquidContainer lc;
    private MeshRenderer renderer;

    [SerializeField]
    private float size = 100;

    [SerializeField]
    private float amount = 100;

    [SerializeField]
    private float radius = 0.4f;
    private float length;
    private float minLength = 0;

    [SerializeField]
    private float maxLength = 0.2f;

    #endregion

    private void Start() {
        renderer = gameObject.GetComponent<MeshRenderer>();
        transform.localScale = new Vector3(radius, 0, radius);
        // Get size from liquid class
        // Get max length and radius from somewhere
    }

    private void Update() {
        UpdateAmount();

        float fillRate = amount / size;
        float newLength = maxLength * fillRate;

        Vector3 s = transform.localScale;
        transform.localScale = new Vector3(s.x, length,s.z);

        Vector3 p = transform.localPosition;
        float newPos = p.y + (newLength - length);
        transform.localPosition = new Vector3(p.x, newPos, p.z);

        if (newLength == 0) {
            renderer.enabled = false;
        } else {
            if (!renderer.enabled) renderer.enabled = true;
        }

        length = newLength;
    }

    private void UpdateAmount() {
        // Get amount from liquid class, where ever it might be
        //amount = lc.getAmount();
    }
}
