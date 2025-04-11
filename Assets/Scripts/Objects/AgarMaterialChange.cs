using System;
using Unity;
using UnityEngine;


class AgarMaterialChange : MonoBehaviour {
    private GameObject agar;
    private GameObject parent;
    public Material material;

    void Start() {
        agar = gameObject;
        Debug.Log(agar);
        parent = agar.transform.parent.gameObject;
        Debug.Log(parent);
        material = agar.GetComponent<MeshRenderer>().material;
        Debug.Log(material);
    }

    public void LiquidAdded() {
        switch(parent.GetComponent<AgarPlateBottom>().ObjectType) {
            case ObjectType.SabouradDextrosiPlate: {
                material.SetColor("_Color", new Color(0.8490566f, 0.6081737f, 0.01762191f, 0.6588235f));
                material.SetColor("_BaseColor", new Color(0.8490566f, 0.6081737f, 0.01762191f, 0.6588235f));
                break;
            }
            case ObjectType.SoycaseinePlate: {

                
                material.SetColor("_Color", new Color(0.9849057f, 0.8376793f, 0.4144036f, 1f));
                material.SetColor("_BaseColor", new Color(0.9849057f, 0.8376793f, 0.4144036f, 1f));
                break;
            }
        }

        material.SetFloat("_Smoothness", .95f);
        Debug.Log("Set Wet");
    }

    public void LiquidSpread() {
        material.SetFloat("_Smoothness", .5f);
        Debug.Log("Set Spread");
    }
}
