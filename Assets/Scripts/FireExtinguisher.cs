using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{

    private List<ITogglableFire> inside = new List<ITogglableFire>();
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
                foreach (ITogglableFire fire in inside) {
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
        if (other.tag == "PlayerCollider" || other.tag == "FireGrid") {
            ITogglableFire fire = other.GetComponentInParent(typeof(ITogglableFire)) as ITogglableFire;
            inside.Add(fire);

            if (extinguishing) {
                fire.Extinguish();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "PlayerCollider" || other.tag == "FireGrid") {
            ITogglableFire fire = other.GetComponentInParent(typeof(ITogglableFire)) as ITogglableFire;
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
