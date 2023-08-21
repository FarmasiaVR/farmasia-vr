using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class showLiguidContainerLiguidAmount : MonoBehaviour
{
    public LiquidContainer containerToShow;
    public TextMeshPro text;
    public bool displayText;

    //bool indicating if IEnumerator toggleDisplayFor has been invoked already
    bool toggledDisplay;

    // Start is called before the first frame update
    void Start()
    {
        toggledDisplay = false;
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

    public void displayFor(float seconds)
    {
        if(!toggledDisplay)
        {
            StartCoroutine(toggleDisplayFor(seconds));
        }
    }

    IEnumerator toggleDisplayFor(float seconds)
    {
        toggledDisplay = true;
        show();
        yield return new WaitForSeconds(seconds);
        hide();
        toggledDisplay = false;
    }
}
