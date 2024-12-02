using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeBottom : MonoBehaviour {
    public LiquidObject liquidObject;
    private Vector3 on = new Vector3(5.75f, 5.75f, 1.45f);
    private Vector3 off = new Vector3(0, 0, 0);

    void Update()
    {
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
