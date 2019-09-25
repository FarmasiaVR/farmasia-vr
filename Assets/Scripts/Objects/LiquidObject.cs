using System;
using UnityEngine;

public class LiquidObject : MonoBehaviour {

    #region fields
    [SerializeField]
    private MeshRenderer mesh;
    private MeshRenderer parent;

    private float percentage;

    #endregion

    private void OnValidate() {
        parent = GetComponentInParent<MeshRenderer>();
        UpdateObject();
    }

    public void SetFillPercentage(float percentage) {
        if (percentage < 0 || percentage > 1) {
            throw new ArgumentOutOfRangeException("percentage", percentage, "Percentage should be [0, 1]");
        }

        this.percentage = percentage;
        UpdateObject();
    }

    private void UpdateObject() {
        // localScale scales around pivot (default is center of object)
        // Therefore, translation needed
        transform.localScale = new Vector3(1, percentage, 1);

        // Translate by scale delta amount
        float newY = percentage - 1;
        transform.localPosition = new Vector3(0, newY, 0);

        if (mesh != null) {
            mesh.enabled = percentage > 0;
        }
    }
}        
