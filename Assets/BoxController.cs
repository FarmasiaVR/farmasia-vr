using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public int Temp;
    public int rightItems;
    public int wrongItems;
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
        AgarPlateBottom plate = other.GetComponent<AgarPlateBottom>();
        if (plate == null) return;
        if (plate.isTriggered == null) Logger.Print(plate.name + "Dont have a trigger bool!");
	    plate.isTriggered = true;

        if (plate.name == "SabouraudPlateBottom" && Temp==37 && !plate.isVenting && !plate.isOpen)
        {
            rightItems++;
            Logger.Print($"Item {plate.name} detected!");
            return;
        }

        // if (plate.name == "SabouraudPlateLid")
        // {
        //     rightItems++;
        //     Logger.Print($"Item {plate.name} detected!");
        //     return;
        // }
        
        // if (plate.name == "SoyCaseinPlateLid")
        // {
        //     rightItems++;
        //     Logger.Print($"Item {plate.name} detected!");
        //     return;
        // }

        if (plate.name == "SoyCaseinPlateBottom" && Temp==25 && !plate.isVenting && !plate.isOpen)
        {
            rightItems++;
            Logger.Print($"Item {plate.name} detected!");
            return;
        }  


    }
    private void OnTriggerExit(Collider other)
    {
        AgarPlateBottom plate = other.GetComponent<AgarPlateBottom>();
        if (plate == null) return;
        if (plate.isTriggered == null) Logger.Print(plate.name + "Dont have a trigger bool!");
        plate.isTriggered = false;
        if (plate.name == "SabouraudPlateBottom")
            {
            rightItems--;
            Logger.Print("Item left the trigger zone.");
            }

        // if (item.name == "SabouraudPlateLid")
        //     {
        //     rightItems--;
        //     Logger.Print("Item left the trigger zone.");
        //     }
        
        if (plate.name == "SoyCaseinPlateBottom")
            {
            rightItems--;
            Logger.Print("Item left the trigger zone.");
            }
        
        // if (item.name == "SoyCaseinPlateLid")
        //     {
        //     rightItems--;
        //     Logger.Print("Item left the trigger zone.");
        //     } 
        
    }
}
