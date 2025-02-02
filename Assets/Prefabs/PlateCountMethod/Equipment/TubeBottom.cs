using Newtonsoft.Json.Converters;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class TubeBottom : MonoBehaviour { // jank code to make filling the test tube appear natural. Scaling the test tube might require changing the size of "on"!
    public LiquidObject liquidObject;
    private Vector3 on = new Vector3(5.75f, 5.75f, 1.45f);
    private Vector3 off = new Vector3(0, 0, 0);
    public GameObject otherObject;
    private Material otherMaterial;

    public async void CopyMaterial() // performant way to handle this! no calling update every frame!!!
    {

        await Task.Delay(10); // wait just a bit to make sure we get the updated texture
        if (otherObject != null) { 
            otherMaterial = otherObject.GetComponent<MeshRenderer>().material;
            GetComponent<MeshRenderer>().material = otherMaterial;
        }
        if (liquidObject.percentage > 0)
        {
            transform.localScale = on;
        }
        else
        {
            transform.localScale = off;
        }
    }
}
