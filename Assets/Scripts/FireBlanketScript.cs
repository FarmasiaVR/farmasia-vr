using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlanketScript : MonoBehaviour
{


    /// Checks if the other colliding object is FireGrid's colliderCube
    /// If the collision happens with FireGrid's colliderCube, calls the Extinguish script attached to that spesific gameObject
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "FireGrid")
        {
            collision.gameObject.GetComponentInParent<FireGrid>().ExtinguishWithBlanket();
        }
    }

}
