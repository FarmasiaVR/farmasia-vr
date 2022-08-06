using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour {

    public Vector3 targetPos = new Vector3(-18.14f, -0.736f, 3.644f);
    public float moveTime = 3.0f;

    public void MoveCart() {
        StartCoroutine(EaseInOut());
    }

    public IEnumerator EaseInOut() {
        Vector3 startingPos = transform.position;
        float elapsedTime = 0;
        while (elapsedTime <= 1.0f) {
            transform.position = Vector3.Lerp(startingPos, targetPos, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, elapsedTime)));
            elapsedTime += Time.deltaTime / moveTime;
            yield return null;
        }
    }
}
