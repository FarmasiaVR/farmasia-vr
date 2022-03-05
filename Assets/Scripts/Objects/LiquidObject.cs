using System;
using UnityEngine;

public class LiquidObject : MonoBehaviour {

    #region fields
    [SerializeField]
    private MeshRenderer mesh;

    private float percentage;
    #endregion

    private void OnValidate() {
        UpdateObject();
    }

    public void SetFillPercentage(float percentage) {

        if (float.IsNaN(percentage)) {
            throw new Exception("Value was NaN");
        }

        if (percentage < 0 || percentage > 1) {
            throw new ArgumentOutOfRangeException("percentage", percentage.ToString(), "Percentage should be [0, 1]");
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

    public void SetMaterialFromType(LiquidType type) {
        switch (type) {
            case LiquidType.Peptonwater:
                mesh.material = Resources.Load<Material>("Liquids/PeptonWater");
                break;
                
            case LiquidType.Tioglygolate:
                mesh.material = Resources.Load<Material>("Liquids/Tioglygolate");
                break;
                
            case LiquidType.Soycaseine:
                mesh.material = Resources.Load<Material>("Liquids/Soycaseine"); ;
                break;
        }
    }
}        
