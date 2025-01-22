using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTransparencyControl : MonoBehaviour
{
    public Material material;


    // Start is called before the first frame update
    void Start()
    {
        if(material == null)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            if(renderer != null)
            {
                material = renderer.material;
            }
        }

        if (material == null)
        {
            Debug.LogError("MaterialTransparencyControl did not find target Material from gameobject or it was not assigned from the editor!");
        }
    }


    public void setTransparency(float newAlpha)
    {
        Color color = material.color;
        color.a = newAlpha;
        material.color = color;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
