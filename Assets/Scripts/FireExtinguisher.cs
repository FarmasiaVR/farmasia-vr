using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{

    private List<FireGrid> inside = new List<FireGrid>();
    private bool canExtinguish;
    private bool extinguishing;
    // Start is called before the first frame update
    void Start()
    {
        canExtinguish = false;
        extinguishing = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Extinguish()
    {
        if (canExtinguish)
        {
            extinguishing = true;
            if (inside.Count != 0)
            {
                foreach (FireGrid fire in inside)
                {
                    fire.Extinguish();
                    
                }
            }
        }
    }

    public void StopExtinguish()
    {
        extinguishing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FireGrid")
        {
            FireGrid fire = other.GetComponentInParent<FireGrid>();
            inside.Add(fire);

            if (extinguishing)
            {
                fire.Extinguish();
            }
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
