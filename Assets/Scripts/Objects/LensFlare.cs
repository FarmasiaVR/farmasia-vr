using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensFlare : MonoBehaviour {

    IEnumerator Start() {
        float size = Random.Range(0.03f, 0.06f);
        Vector3 targetSize = new Vector3(size, size, size);
        float time = Random.Range(0.5f, 2.0f);
        yield return Lerp(Vector3.zero, targetSize, time);
        yield return Lerp(targetSize, Vector3.zero, time);
        Destroy(gameObject);
    }

    private IEnumerator Lerp(Vector3 a, Vector3 b, float time) {
        float i = 0.0f;
        float rate = (1.0f / time) * 2.0f;
        while (i < 1.0f) {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }
}
