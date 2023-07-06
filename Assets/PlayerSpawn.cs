using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{

    [SerializeField]
    GameObject XRPlayer;

    [SerializeField]
    Transform spawnPosition;
    // Start is called before the first frame update
    void Awake()
    {
        XRPlayer.transform.position = spawnPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
