using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoapBubble : MonoBehaviour {

    private Vector3 targetSize;
    private float spawnTime;

    void Start() {
        float size = Random.Range(0.01f, 0.03f);
        targetSize = new Vector3(size, size, size);
        spawnTime = Random.Range(0.1f, 1.0f);
        StartCoroutine(SpawnSoapBubble());
    }

    private IEnumerator SpawnSoapBubble() {
        float time = spawnTime;

        while (time > 0) {
            time -= Time.deltaTime;
            float factor = 1 - time / spawnTime;
            transform.localScale = targetSize * factor;
            yield return null;
        }

        transform.localScale = targetSize;
    }
}
