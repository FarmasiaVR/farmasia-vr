using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBurn : MonoBehaviour
{
    [Tooltip("The time it takes for the player to burn to death in seconds")]
    public float maxBurnHealth = 15;
    private float currentHealth;
    private HashSet<GameObject> firesTouching = new HashSet<GameObject>();
    private bool dead = false;

    [Tooltip("Passes a float with the current health percentage")]
    public UnityEvent <float> BurnUpdate;
    public UnityEvent Death;

    private void OnEnable()
    {
        currentHealth = maxBurnHealth;
        dead = false;
    }

    private void Update()
    {
        if (dead)
            return;
        if(firesTouching.Count >= 1)
        {
            BurnUpdate.Invoke(currentHealth/maxBurnHealth);
            currentHealth -= Time.deltaTime;
        }
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        Death.Invoke();
        dead = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        //the fire should have its own collision layer but this will do for now
        if (other.gameObject.tag == "FireGrid")
            firesTouching.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "FireGrid")
        {
            if (firesTouching.Contains(other.gameObject))
                firesTouching.Remove(other.gameObject);
        }
    }
}
