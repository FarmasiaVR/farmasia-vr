using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlanketScript : MonoBehaviour
{
    private FireGrid fireGrid;

    // Fetches the FireGrid script that's attached to the FireGridObject
    private void Start()
    {
        fireGrid = FindObjectOfType<FireGrid>();
    }

    // Calls FireGrid's Extinguish script if its the other colliding object
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "FireGrid")
        {
            Debug.Log("pitais");
            fireGrid.Extinguish();
        }
    }

}
