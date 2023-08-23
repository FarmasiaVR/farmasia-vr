using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCounter : MonoBehaviour
{
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementFireCount()
    {
        counter++;
        Debug.Log("Fire added. Current count: " + counter);
    }

    public void DecrementFireCount()
    {
        counter--;
        if (counter < 0)
        {
            counter = 0;
            Debug.Log("Warning!! Fire cannot go under zero. Error/Bug in code");
        }
        Debug.Log("Fire reduced. Current count: " + counter);
    }

    public int GetFireCount() 
    {
            return counter; 
    }
}