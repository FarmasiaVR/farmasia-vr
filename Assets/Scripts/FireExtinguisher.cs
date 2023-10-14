using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{

    private List<PlayerFireController> inside = new List<PlayerFireController>();
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
                foreach (PlayerFireController fire in inside) {
                    fire.Extinguish();
                }
            }
        }
    }

    public void StopExtinguish()
    {
        extinguishing = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "PlayerCollider") {
            PlayerFireController playerBody = other.GetComponentInParent<PlayerFireController>();
            inside.Add(playerBody);

            if (extinguishing) {
                playerBody.Extinguish();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "PlayerCollider") {
            // This would be very slow with a lot of fires in the list
            inside.Remove(other.GetComponentInParent<PlayerFireController>());
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
