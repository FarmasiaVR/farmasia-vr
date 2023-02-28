using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnFireManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fireToControl;

    void Start()
    {
        fireToControl.GetComponent<FireGrid>().Extinguish();
        fireToControl.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision) {
        Debug.Log("trigger enter");
        //layer 15 = "Player"
        if (collision.gameObject.tag == "FireGrid") {
            
            Debug.Log("STANDING ON FIRE");
            if (collision.gameObject.layer != 15 && collision.gameObject.GetComponentInParent<FireGrid>().IsIgnited()) {
                Debug.Log("Player is on fire after if-condition");
                fireToControl.SetActive(true);
                fireToControl.GetComponent<FireGrid>().Ignite();
            }
        }
    }
}
