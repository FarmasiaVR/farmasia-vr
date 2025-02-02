using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRGlassesObject : MonoBehaviour
{
    private bool isClean;

    public XRHandWashingLiquid sink;
    public GameObject tapCollider;

    private Glasses legacyObject;

    private void OnTriggerEnter(Collider other)
    {
        if (!isClean)
        {
            if (other.gameObject != tapCollider.gameObject || !sink.IsRunning()) return;
            isClean = true;
            UpdateLegacyObject();
            Events.FireEvent(EventType.CleaningGlasses, CallbackData.Object(legacyObject));
        }
        if (other.CompareTag("PlayerCollider"))
        {
            Destroy(gameObject);
        }
    }

    private void UpdateLegacyObject()
    {
        legacyObject= new Glasses();
        legacyObject.tapCollider = tapCollider;
    }
}
