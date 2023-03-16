using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class showLiguidContainerLiguidAmount : MonoBehaviour
{
    public LiquidContainer containerToShow;
    public TextMeshPro text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void FixedUpdate()
    {
        text.text = containerToShow.Amount.ToString() + " / " + containerToShow.Capacity.ToString();    
    }
}
