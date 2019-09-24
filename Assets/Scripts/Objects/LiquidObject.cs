using System;
using UnityEngine;

public class LiquidObject : MonoBehaviour {

    #region fields
    private MeshRenderer mesh;

    private float prevLength;
    private float length;

    [SerializeField]
    private float maxLength = 0.2f;
    #endregion

    private void Start() {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    public void SetFillPercentage(float percentage) {
        if (percentage < 0 || percentage > 1) {
            throw new ArgumentOutOfRangeException("percentage", percentage, "Percentage should be [0, 1]");
        }

        prevLength = length;
        length = percentage * maxLength;

        ScaleAroundBottom();
        mesh.enabled = length > 0;
    }

    private void ScaleAroundBottom() {
        // localScale scales around pivot (default is center of object)
        // Therefore, translation needed
        Vector3 s = transform.localScale;
        transform.localScale = new Vector3(s.x, length, s.z);

        // Translate by scale delta amount
        Vector3 p = transform.localPosition;
        float newPos = p.y + (length - prevLength);
        transform.localPosition = new Vector3(p.x, newPos, p.z);
    }
}        
