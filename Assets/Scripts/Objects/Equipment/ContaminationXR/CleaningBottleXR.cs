using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningBottleXR : MonoBehaviour
{
    public ParticleSystem particle;
    private List<ContaminableItem> contaminableItems = new List<ContaminableItem>();

    public void Clean()
    {
        particle.Play();

        foreach (ContaminableItem item in contaminableItems)
        {
            item.SetClean();
        }
    }

    // These functions are called through the collider event redirector that is attached to "CleaningCollider" object.
    public void ItemEnteredTrigger(Collider item)
    {
        if (item.gameObject.GetComponent<ContaminableItem>())
        {
            contaminableItems.Add(item.gameObject.GetComponent<ContaminableItem>());
        }
    }

    public void ItemExitedTrigger(Collider item)
    {
        if (contaminableItems.Contains(item.gameObject.GetComponent<ContaminableItem>())) 
        {
            contaminableItems.Remove(item.gameObject.GetComponent<ContaminableItem>());
        }
    }

}
