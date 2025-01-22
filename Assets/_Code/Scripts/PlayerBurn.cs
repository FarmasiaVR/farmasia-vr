using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBurn : MonoBehaviour
{
    [Tooltip("The time it takes for the player to burn to death in seconds")]
    public float maxBurnHealth = 30;
    public bool godMode = false;
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
        if(firesTouching.Count >= 1) {
            Debug.Log(firesTouching.Count);
            BurnUpdate.Invoke(currentHealth/maxBurnHealth);
            currentHealth -= Time.deltaTime;
        }
        if (currentHealth <= 0 && !godMode)
            Die();
    }

    public void Die()
    {
        Death.Invoke();
        dead = true;
    }

    private void RemoveFire(Collider collision) {
        if (firesTouching.Contains(collision.gameObject))
            firesTouching.Remove(collision.gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        //the fire should have its own collision layer but this will do for now
        FireGrid fire = collision.gameObject.GetComponentInParent<FireGrid>(); //TODO: After new fire supports extinguishing, get the parent in a different way and remove null check below
        
        if (collision.gameObject.tag == "FireGrid") {
            firesTouching.Add(collision.gameObject);
            if (fire != null) { //Skip new fire, because it doesn't support extinguishing
                fire.onExtinguish.AddListener(delegate{RemoveFire(collision);});
                fire.onIgnite.AddListener(delegate{firesTouching.Add(collision.gameObject);});
            }
        }
    }
    private void OnTriggerExit(Collider collision) {
        FireGrid fire = collision.gameObject.GetComponentInParent<FireGrid>(); //TODO: After new fire supports extinguishing, get the parent in a different way and remove null check below

        if (collision.gameObject.tag == "FireGrid") {
            RemoveFire(collision);
            if (fire != null) { //Skip new fire, because it doesn't support extinguishing
                fire.onExtinguish.RemoveListener(delegate{RemoveFire(collision);});
                fire.onIgnite.RemoveListener(delegate{firesTouching.Add(collision.gameObject);});
            }
        }
    }
}
