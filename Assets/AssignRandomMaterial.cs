using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.XR.CoreUtils;

public class AssignRandomMaterial : MonoBehaviour
{
    [SerializeField]
    private List<Material> materials;
    private MeshRenderer meshRenderer;
    private static System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material newMat = randomMaterial();
        // Debug.Log("Adding " + newMat);
        meshRenderer.AddMaterial(newMat);
    }

    private Material randomMaterial()
    {
        return materials[random.Next(materials.Count)];
    }
}
