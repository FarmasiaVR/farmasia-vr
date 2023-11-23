using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This class should be renamed if functionality other than fire is added to the "player body" 
public class PlayerInteractions : MonoBehaviour, ITogglableFire {
    [Tooltip("The time it takes for the player to burn to death in seconds")]
    public float maxBurnHealth = 30;
    public bool godMode = false;
    [SerializeField] private SimpleFire playerFire;
    [HideInInspector] public bool isBurning {get => playerFire.isBurning;}

    [Tooltip("Passes a float with the current health percentage")]
    public UnityEvent <float> BurnUpdate;
    public UnityEvent Death;
    public UnityEvent OnShower;

    private float currentHealth;
    private bool dead = false;

    // Extinguishing player fire now happens through the player hitbox just like igniting it
    public void Extinguish() => playerFire.Extinguish();
    public void Ignite() => playerFire.Ignite();

    public void Shower() {
        Extinguish();
        OnShower.Invoke();
    }

    private void OnEnable() {
        currentHealth = maxBurnHealth;
        dead = false;
    }

    private void Update() {
        if(isBurning) {
            BurnUpdate.Invoke(currentHealth/maxBurnHealth);
            currentHealth -= Time.deltaTime;
        }
        
        if (currentHealth <= 0 && !dead && !godMode) {
            Death.Invoke();
            dead = true;
        }
    }

    //TODO: Move ignition to fire sources. Makes more sense as extinguishing is already done by the thing extinguishing.
    private void OnTriggerEnter(Collider collision) {
        // Player fire should not have a cube hitbox
        if (collision.gameObject.tag == "FireGrid") {
            ITogglableFire fire = collision.GetComponentInParent(typeof(ITogglableFire)) as ITogglableFire;
            if (fire != null && fire.isBurning) {
                Ignite();
            } else if (fire == null) {
                Debug.Log("Stepped on top of tagged fire hitbox, that doesn't implement the fire interface");
                Ignite();
            }
        }
    }
}