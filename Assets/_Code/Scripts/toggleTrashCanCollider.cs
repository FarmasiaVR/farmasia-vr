using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleTrashCanCollider : MonoBehaviour
{
    public Collider colliderToToggle;
    public bool colliderEnabledAlways;
    // Start is called before the first frame update
    void Start()
    {
        colliderToToggle.enabled = colliderEnabledAlways;
        Events.SubscribeToEvent(enableTrashCollider, EventType.canCollectTrash);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void enableTrashCollider(CallbackData data)
    {
        colliderToToggle.enabled =true;
    }

  
}
