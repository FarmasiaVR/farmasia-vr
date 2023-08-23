using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCollideChecker : MonoBehaviour
{

    public EyeShowerTutorialSceneManager manager;
    private bool inside;
    // Start is called before the first frame update
    void Start()
    {
        inside = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCollider")
        {
            inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerCollider")
        {
            inside = false;
        }
    }

    public void Activate()
    {
        if (inside)
        {
            manager.Aim();
        }
    }
}
