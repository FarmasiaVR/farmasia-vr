using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followObjectInFlatSurface : MonoBehaviour
{

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       gameObject.transform.position = new Vector3(target.transform.position.x, gameObject.transform.position.y, target.transform.position.z);
    
    }
}
