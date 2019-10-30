using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintText : MonoBehaviour {

    #region Fields
    private float spawnTime = 0.5f;

    private Vector3 targetSize;
    private HintCloseButton button;
    #endregion

    private void Awake() {
        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void Start() {
        button = transform.GetComponentInChildren<HintCloseButton>();
        button.Disabled = true;
        StartCoroutine(InitSpawn());
    }

    private IEnumerator InitSpawn() {

        float time = spawnTime;

        Vector3 buttonPos = button.transform.localPosition;

        while (time > 0) {
            time -= Time.deltaTime;
            float factor = 1 - time / spawnTime;
            transform.localScale = targetSize * factor;
            yield return null;
        }
        transform.localScale = targetSize;
        button.transform.localPosition = buttonPos;

        button.Disabled = false;
        button.Initialize();
    }
    private IEnumerator DestroyCoroutine() {

        float time = spawnTime;

        while (time > 0) {
            time -= Time.deltaTime;
            float factor = time / spawnTime;
            transform.localScale = targetSize * factor;
            yield return null;
        }

        Destroy(gameObject);
    }

    public void DestroyHint() {
        StartCoroutine(DestroyCoroutine());
    }
}
