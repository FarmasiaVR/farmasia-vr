using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveSpreadableFires : MonoBehaviour
{
    public UnityEvent AllFiresExtinguished;
    private int activeFires = 0;

    public void FireStarted()
    {
        activeFires += 1;
    }

    public void FireExtinguished()
    {
        activeFires -= 1;
        if (activeFires < 1)
            AllFiresExtinguished.Invoke();
    }


}
