using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBurn : MonoBehaviour
{
    [Tooltip("The time it takes for the player to burn to death in seconds")]
    public float maxBurnHealth = 30;
    private float currentHealth;
    private HashSet<Collider> firesTouching = new HashSet<Collider>();

    [Tooltip("Passes a float with the current health percentage")]
    public UnityEvent <float> BurnUpdate;
    public UnityEvent Death;

    private void OnEnable()
    {
        currentHealth = maxBurnHealth;
    }

    private void Update()
    {
        if(firesTouching.Count >= 1)
        {
            BurnUpdate.Invoke(currentHealth/maxBurnHealth);
            currentHealth -= Time.deltaTime;
            Debug.Log("ow");
        }
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        Death.Invoke();
        Debug.Log("Sadge");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //the fire should have its own collision layer but this will do for now
        if (collision.collider.tag == "FireGrid")
            firesTouching.Add(collision.collider);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "FireGrid")
        {
            if (firesTouching.Contains(collision.collider))
                firesTouching.Remove(collision.collider);
        }
    }
}
