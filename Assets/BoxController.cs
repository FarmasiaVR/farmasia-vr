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

        if (item == null) {
            return;
        }
	    
        if (item.name == "Sabouraudplatebottom")
        {
            Logger.Print($"Item {item.name} detected!");
            return;
        }
        
        if (item.CompareTag("SoyCaseinPlate"))
        {
            Logger.Print($"Item {item.name} detected!");
            return;
        }
        
        wrongItems++;

        Logger.Print("item : " + item);

    }
}
