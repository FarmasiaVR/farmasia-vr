using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TipOverObject : MonoBehaviour
{
    public UnityEvent ObjectTippedOver;
    public UnityEvent ObjectTippingStarted;
    public Transform targetObject;
    private Rigidbody targetRigidbody; // Reference to the Rigidbody

    public float xRotation = 0.0f;
    public float yRotation = 0.0f;
    public float zRotation = 0.0f;

    public float rotationDuration = 1.0f;

    private bool isRotating = false;
    private bool isOpen = false;
    private bool didAlready = false;

    void Start()
    {
        // Initialize the Rigidbody reference
        targetRigidbody = targetObject.GetComponent<Rigidbody>();
        // Optionally, disable gravity at start if needed
        if (targetRigidbody != null)
        {
            targetRigidbody.useGravity = false;
        }
    }

    public void ApplyRotation()
    {
        if (!isRotating)
        {
            StartCoroutine(Rotate());
            ObjectTippingStarted.Invoke();
        }
    }


    IEnumerator Rotate()
    {
        if (!isOpen && !didAlready)
        {
            isRotating = true;

            Quaternion startRotation = targetObject.rotation;
            Quaternion endRotation = targetObject.rotation * Quaternion.Euler(xRotation, yRotation, zRotation);

            float elapsedTime = 0.0f;
            while (elapsedTime < rotationDuration)
            {
                float t = elapsedTime / rotationDuration;
                targetObject.rotation = Quaternion.Slerp(startRotation, endRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            targetObject.rotation = endRotation;
            isOpen = true;
            isRotating = false;
            ObjectTippedOver.Invoke();

            if (targetRigidbody != null)
            {
                targetRigidbody.useGravity = true;
                targetRigidbody.isKinematic = false;
            }
        }
        

        didAlready = false;
    }
}
