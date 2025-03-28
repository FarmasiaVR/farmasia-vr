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
        GeneralItem item = other.GetComponent<GeneralItem>();
        if (item == null) return;
        if (item.isTriggered == null) Logger.Print(item.name + "Dont have a trigger bool!");
	    item.isTriggered = true;

        if (item.name == "SabouraudPlateBottom")
        {
            rightItems++;
            Logger.Print($"Item {item.name} detected!");
            return;
        }

        if (item.name == "SabouraudPlateLid")
        {
            rightItems++;
            Logger.Print($"Item {item.name} detected!");
            return;
        }
        
        if (item.name == "SoyCaseinPlateLid")
        {
            rightItems++;
            Logger.Print($"Item {item.name} detected!");
            return;
        }

        if (item.name == "SoyCaseinPlateBottom")
        {
            rightItems++;
            Logger.Print($"Item {item.name} detected!");
            return;
        }  


    }
    private void OnTriggerExit(Collider other)
    {
        GeneralItem item = other.GetComponent<GeneralItem>();
        item.isTriggered = false;
        if (item.name == "SabouraudPlateBottom")
            {
            rightItems--;
            Logger.Print("Item left the trigger zone.");
            }

        if (item.name == "SabouraudPlateLid")
            {
            rightItems--;
            Logger.Print("Item left the trigger zone.");
            }
        
        if (item.name == "SoyCaseinPlateBottom")
            {
            rightItems--;
            Logger.Print("Item left the trigger zone.");
            }
        
        if (item.name == "SoyCaseinPlateLid")
            {
            rightItems--;
            Logger.Print("Item left the trigger zone.");
            } 
        
    }
}
