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
                mesh.material.SetColor("_SideColor", new Color(0.3722854f, 0.8229616f, 0.8867924f, 1));
                mesh.material.SetColor("_TopColor", new Color(0.2722854f, 0.8229616f, 0.8867924f, 1));

                //mesh.material = Resources.Load<Material>("Liquids/PeptonWater");
                break;
                
            case LiquidType.Tioglygolate:
                mesh.material.SetColor("_SideColor", new Color(1f, 0.7570499f, 0.1603773f, 1));
                mesh.material.SetColor("_TopColor", new Color(1f, 0.7570499f, 0.1603773f, 1));

                //mesh.material = Resources.Load<Material>("Liquids/Tioglygolate");
                break;
                
            case LiquidType.Soycaseine:
                mesh.material.SetColor("_SideColor", new Color(0.9528301f, 0.5856799f, 0.215735f, 1));
                mesh.material.SetColor("_TopColor", new Color(0.9528301f, 0.5856799f, 0.215735f, 1));

                //mesh.material = Resources.Load<Material>("Liquids/Soycaseine"); ;
                break;
        }
    }
}        
