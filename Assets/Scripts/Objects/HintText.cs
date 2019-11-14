using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintText : MonoBehaviour {

    #region Fields
    private float spawnTime = 0.5f;

    private Vector3 targetSize;
    private Transform bt;
    #endregion

    private void Awake() {
        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void Start() {
        bt = transform.Find("CloseButton");
        StartCoroutine(InitSpawn());
    }

    private IEnumerator InitSpawn() {

        float time = spawnTime;

        Vector3 buttonPos = bt.localPosition;

        while (time > 0) {
            time -= Time.deltaTime;
            float factor = 1 - time / spawnTime;
            transform.localScale = targetSize * factor;
            yield return null;
        }
        transform.localScale = targetSize;
        bt.localPosition = buttonPos;
        bt.transform.parent = null;

        Logger.Print("Adding close button to button");
        HintCloseButton button = bt.gameObject.AddComponent<HintCloseButton>();
        button.LookAtPlayer = true;
        button.Hint = this;
        button.Disabled = false;
    }
    private IEnumerator DestroyCoroutine() {

        float time = spawnTime;

        while (time > 0) {
            time -= Time.deltaTime;
            float factor = time / spawnTime;
            transform.localScale = targetSize * factor;
            yield return null;
        }

        GetComponent<Interactable>().DestroyInteractable();
    }

    public void DestroyHint() {
        StartCoroutine(DestroyCoroutine());
    }
}
