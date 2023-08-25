using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCounter : MonoBehaviour
{
    // Adjusts how ofter in seconds the update is run
    [SerializeField]
    private float seconds;

    private float nextLogTime = 0.0f;
    public int counter = 0;

    void Start()
    {

    }

    void Update()
    {
        if (Time.time > nextLogTime)
        {
            nextLogTime += seconds;
            Debug.Log("Current fire count: " + counter);
        }
    }

    public void IncrementFireCount()
    {
        counter++;
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