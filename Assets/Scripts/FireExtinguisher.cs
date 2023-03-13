using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{

    private List<FireGrid> inside = new List<FireGrid>();
    private bool canExtinguish;
    // Start is called before the first frame update
    void Start()
    {
        canExtinguish = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Extinguish()
    {
        if (canExtinguish)
        {
            if (inside.Count != 0)
            {
                foreach (FireGrid fire in inside)
                {
                    fire.Extinguish();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FireGrid")
        {
            inside.Add(other.GetComponentInParent<FireGrid>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "FireGrid")
        {
            inside.Remove(other.GetComponentInParent<FireGrid>());
        }
    }

    public void enableExtinguisher()
    {
        canExtinguish = true;
    }

    public void disableExtinguisher()
    {
        canExtinguish = false;
    }
}
