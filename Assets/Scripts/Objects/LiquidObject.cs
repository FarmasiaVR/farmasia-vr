using System;
using System.Collections;
using UnityEngine;

public class LiquidObject : MonoBehaviour {

    #region fields
    [SerializeField]
    private MeshRenderer mesh;

    [SerializeField]
    private bool HasRealLiquidMaterial;

    [SerializeField]
    private float percentage;
    #endregion

    void Awake() {
        if (HasRealLiquidMaterial) {
            InitObject();
        } else {
            UpdateObject();
        }
    }

    // OnValidate sucks, don't use it
    //private void OnValidate() {
    //    UpdateObject();
    //}

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

    private void InitObject() {
        mesh.material.SetFloat("_Fill", percentage);
    }

    private void UpdateObject() {
        if (!HasRealLiquidMaterial) {
            // localScale scales around pivot (default is center of object)
            // Therefore, translation needed


            transform.localScale = new Vector3(1, percentage, 1);

            // Translate by scale delta amount
            float newY = percentage - 1;
            transform.localPosition = new Vector3(0, newY, 0);
        } else {
            StartCoroutine(LerpLiquid(percentage, 0.5f));
        }
    }

    private IEnumerator LerpLiquid(float targetAmount, float lerpTimeInSeconds) {
        if (lerpTimeInSeconds == 0) yield break;
        
        float t = 0;
        float startAmount = mesh.material.GetFloat("_Fill");
        float currentAmount;

        if (mesh != null && targetAmount != 0) {
            mesh.enabled = true;
        }

        while (t < lerpTimeInSeconds) {
            currentAmount = Mathf.Lerp(startAmount, targetAmount, t / lerpTimeInSeconds);
            t += Time.deltaTime;

            mesh.material.SetFloat("_Fill", currentAmount);

            yield return null;
        }

        if (mesh != null && targetAmount == 0) {
            mesh.enabled = false;
        }
    }

    public void SetMaterialFromType(LiquidType type) {
        switch (type) {
            case LiquidType.Peptonwater:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(0.3722854f, 0.8229616f, 0.8867924f, 1));
                    mesh.material.SetColor("_TopColor", new Color(0.2722854f, 0.8229616f, 0.8867924f, 1));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/PeptonWater");
                }
                break;
                
            case LiquidType.Tioglygolate:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(1f, 0.7570499f, 0.1603773f, 1));
                    mesh.material.SetColor("_TopColor", new Color(1f, 0.7570499f, 0.1603773f, 1));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/Tioglygolate");
                }
                break;
                
            case LiquidType.Soycaseine:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(0.9528301f, 0.5856799f, 0.215735f, 1));
                    mesh.material.SetColor("_TopColor", new Color(0.9528301f, 0.5856799f, 0.215735f, 1));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/Soycaseine"); ;
                }
                break;
        }
    }
}        
