using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnFireManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fireToControl;

    [SerializeField]
    bool startOnFire;

    void Start()
    {
        if (!startOnFire)
        {
            fireToControl.GetComponent<FireGrid>().Extinguish();
            fireToControl.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("trigger enter");
        //layer 15 = "Player"
        if (collision.gameObject.tag == "FireGrid")
        {
            fireToControl.SetActive(true);
            fireToControl.GetComponent<FireGrid>().Ignite();
        }
    }
}
