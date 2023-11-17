using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBunsenController : MonoBehaviour
{
    [SerializeField] private Animator myBunsen = null;

    [SerializeField] private bool openTrigger = false;

    public UnityEvent onFirstEnter = new UnityEvent();

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "PlayerCollider")
        {
            if(openTrigger)
            {
                //myBunsen.Play("TripBunsen", 0, 0.0f);
                gameObject.SetActive(false);
                onFirstEnter.Invoke();
            }
        }
    }

}
