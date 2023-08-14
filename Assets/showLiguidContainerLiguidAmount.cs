using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class showLiguidContainerLiguidAmount : MonoBehaviour
{
    public LiquidContainer containerToShow;
    public TextMeshPro text;
    public bool displayText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void FixedUpdate()
    {
        if (displayText)
        {
            text.text = containerToShow.Amount.ToString() + " / " + containerToShow.Capacity.ToString();
        }
        else
        {
            text.text = "";
        }
    }

    public void show()
    {
        displayText = true;
    }

    public void hide()
    {
        displayText = false;
    }
}
