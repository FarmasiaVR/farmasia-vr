using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlanketPackageScript : MonoBehaviour
{
    public GameObject FireBlanket;
    public GameObject Cover;
 
    //"Spawns" a blanket from the cover by disabling cover and setting blanket active
    public void SpawnBlanket()
    {
        Cover.SetActive(false);
        FireBlanket.SetActive(true);
    }
}
