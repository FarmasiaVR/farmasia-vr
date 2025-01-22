using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerProximityTrigger: MonoBehaviour
{
    public UnityEvent onFirstEnter = new UnityEvent();

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "PlayerCollider")
        {
            gameObject.SetActive(false);
            onFirstEnter.Invoke();
        }
    }

}
