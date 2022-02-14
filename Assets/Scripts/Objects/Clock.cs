using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private GameObject hourPointer;
    private GameObject minutePointer;

    private void Start() {
        hourPointer = gameObject.transform.GetChild(1).gameObject;
        minutePointer = gameObject.transform.GetChild(2).gameObject;
    }

    private void FixedUpdate() {

        System.DateTime currentTime = System.DateTime.UtcNow;

        float minuteAngle = currentTime.Minute * 1 / 60f * 360f;
        float hourAngle = currentTime.Hour * 1 / 12f * 360f;
        hourPointer.transform.localEulerAngles = new Vector3(0f, 0f, hourAngle);
        minutePointer.transform.localEulerAngles = new Vector3(0f, 0f, minuteAngle);
    }
}
