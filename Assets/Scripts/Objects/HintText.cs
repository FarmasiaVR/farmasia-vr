using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintText : MonoBehaviour {

    #region Fields
    private float spawnTime = 0.5f;

    private Vector3 targetSize;
    #endregion

    private void Awake() {
        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void Start() {

        StartCoroutine(InitSpawn());
    }

    private IEnumerator InitSpawn() {

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
