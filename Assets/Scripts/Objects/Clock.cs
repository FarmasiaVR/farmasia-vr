using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public GameObject GObject;
    private GameObject hourPointer;
    private GameObject minutePointer;
    private int[] sceneTime;

    private void Start() {
        hourPointer = gameObject.transform.GetChild(1).gameObject;
        minutePointer = gameObject.transform.GetChild(2).gameObject;

        sceneTime = GObject.GetComponent<SceneTime>().Value;

        float minuteAngle = 6 * sceneTime[1];
        float hourAngle = (30 * sceneTime[0]) + (sceneTime[1] / 2);
        hourPointer.transform.Rotate(0f, 0f, hourAngle);
        minutePointer.transform.Rotate(0f, 0f, minuteAngle);
    }
}
