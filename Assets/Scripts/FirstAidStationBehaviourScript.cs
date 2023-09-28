using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FirstAidStationBehaviourScript : MonoBehaviour
{
    public List<GameObject> targetObjects = new List<GameObject>();
    public Transform targetObject;

    public float rotationAmount = 135.0f;
    public float rotationDuration = 2.0f;

    private bool isRotating = false;
    private bool isOpen = false;
    private bool didAlready = false;

    private void Start()
    {
        foreach (GameObject targetObject in targetObjects)
        {
            if (targetObject != null)
            {
                Rigidbody rb = targetObject.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.detectCollisions = false;
                }
                else
                {
                    Debug.LogError("No Rigidbody found on the target GameObject: " + targetObject.name);
                }
            }
            else
            {
                Debug.LogError("A target GameObject in the list is null.");
            }
        }
    }

    public void OpenPlasticLid()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateObject());
        }
    }

    IEnumerator RotateObject()
    {
        if (isOpen)
        {
            isRotating = true;

            Quaternion startRotation = targetObject.rotation;
            Quaternion endRotation = targetObject.rotation * Quaternion.Euler(0, -rotationAmount, 0);

            float elapsedTime = 0.0f;
            while (elapsedTime < rotationDuration)
            {
                float t = elapsedTime / rotationDuration;
                targetObject.rotation = Quaternion.Slerp(startRotation, endRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            targetObject.rotation = endRotation;
            isOpen = false;
            isRotating = false;
            didAlready = true;
        }
        if (!isOpen && !didAlready) {
            isRotating = true;

            Quaternion startRotation = targetObject.rotation;
            Quaternion endRotation = targetObject.rotation * Quaternion.Euler(0, rotationAmount, 0);

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
        }
        didAlready = false;
    }

    public void TurnOnInsideColliderAndGravity()
    {
        foreach (GameObject targetObject in targetObjects)
        {
            if (targetObject != null)
            {
                Rigidbody rb = targetObject.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.useGravity = true;
                    rb.detectCollisions = true;
                }
            }
        }
    }
}