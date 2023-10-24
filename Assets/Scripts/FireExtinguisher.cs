using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{

    private List<SimpleFire> inside = new List<SimpleFire>();
    private PlayerFireController playerFire;
    private bool canExtinguish;
    private bool extinguishing;
    // Start is called before the first frame update
    void Start() {
        canExtinguish = false;
        extinguishing = false;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Extinguish() {
        if (canExtinguish) {
            extinguishing = true;
            if (inside.Count != 0) {
                foreach (SimpleFire fire in inside) {
                    fire.Extinguish();
                }
            }

            if (playerFire != null)
                playerFire.Extinguish();
        }
    }

    public void StopExtinguish() {
        extinguishing = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerCollider") {
            playerFire = other.GetComponentInParent<PlayerFireController>();

            if (extinguishing) {
                playerFire.Extinguish();
            }
        } else if (other.tag == "FireGrid") {
            SimpleFire fire = other.GetComponentInParent<SimpleFire>();
            inside.Add(fire);

            if (extinguishing) {
                fire.Extinguish();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "PlayerCollider") {
            playerFire = null;
        } else if (other.tag == "FireGrid") {
            SimpleFire fire = other.GetComponentInParent<SimpleFire>();
            inside.Remove(fire);
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
