using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//general script for rotating a object for wanted amount in a wanted time. If the object has already rotated it rotates back to its original position.
//usefull for levers and handles and doors and such when moving by grabbing is not desired and an animation-like behaviour is wanted.

public class RotateObject : MonoBehaviour
{

    public Transform targetObject;


    public float xRotation = 0.0f;
    public float yRotation = 0.0f;
    public float zRotation = 0.0f;

    public float rotationDuration = 1.0f;

    private bool isRotating = false;
    private bool isOpen = false;
    private bool didAlready = false;

    public void ApplyRotation()
    {
        if (!isRotating)
        {
            StartCoroutine(Rotate());
        }
    }


    IEnumerator Rotate()
    {
        if (isOpen)
        {
            isRotating = true;

            Quaternion startRotation = targetObject.rotation;
            Quaternion endRotation = targetObject.rotation * Quaternion.Euler(-xRotation, -yRotation, -zRotation);

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
        }
        didAlready = false;
    }
}
