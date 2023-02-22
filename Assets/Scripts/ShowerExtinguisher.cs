using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerExtinguisher : MonoBehaviour
{

    private bool containsFire = false;
    private FireGrid fireGrid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FireGrid")
        {
            fireGrid = other.GetComponentInParent<FireGrid>();
            containsFire = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "FireGrid")
        {
            fireGrid = null;
            containsFire = false;
        }
    }

    public void Extinguish()
    {
        if (containsFire)
        {
            fireGrid.Extinguish();
        }
    }

}
