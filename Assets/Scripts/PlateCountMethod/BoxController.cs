using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public int Temp;
    private List<AgarPlateBottom> sabouraudPlates = new List<AgarPlateBottom>();
    private List<AgarPlateBottom> soyCaseinPlates = new List<AgarPlateBottom>();
    private void OnTriggerEnter(Collider other)
    {        
        AgarPlateBottom plate = other.GetComponent<AgarPlateBottom>();
        if (plate == null) return;

        if (plate.name == "SabouraudPlateBottom" && Temp==25 )
        {
            if (!sabouraudPlates.Contains(plate)){
                sabouraudPlates.Add(plate);
            }

        }

        if (plate.name == "SoyCaseinPlateBottom" && Temp==37 )
        {
            if (!soyCaseinPlates.Contains(plate)) 
            {
                soyCaseinPlates.Add(plate);
            }

        }  


    }
    private void OnTriggerExit(Collider other)
    {
        AgarPlateBottom plate = other.GetComponent<AgarPlateBottom>();
        if (plate == null) return;

        if (plate.name == "SabouraudPlateBottom")
            {
            sabouraudPlates.Remove(plate);
            }
        
        if (plate.name == "SoyCaseinPlateBottom")
            {
            soyCaseinPlates.Remove(plate);
            }
                
    }    
    public bool AreAllPlatesReady()
    {
        foreach (var plate in sabouraudPlates)
        {
            if (plate.isOpen || plate.isVenting) return false;
        }
        foreach (var plate in soyCaseinPlates)
        {
            if (plate.isOpen || plate.isVenting) return false;
        }

        if (sabouraudPlates.Count + soyCaseinPlates.Count < 4) return false;
        return true;
    }
}
