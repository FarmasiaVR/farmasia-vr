using System.Collections;
using UnityEngine;

public class HandEffectSpawner : MonoBehaviour {

    private string effect;
    private float speed;

    public GameObject lensFlare;
    public GameObject soapBubble;
    public Transform controller;
    public Transform rotator;
    public LayerMask layerMask;

    public void StartSpawning(string effect, float speed) {
        this.effect = effect;
        this.speed = speed;
        StartCoroutine(SpawnEffect());
    }

    private IEnumerator SpawnEffect() {
        int amount = 0;
        if (effect.Equals("LensFlare")) amount = 6;
        if (effect.Equals("SoapBubble")) amount = 450;
        for (int i = 0; i < amount; i++) {
            float randomX = Random.Range(0.0f, 360.0f);
            float randomY = Random.Range(0.0f, 360.0f);
            Vector3 rotationVector = new Vector3(randomX, randomY, 0);
            rotator.rotation = Quaternion.Euler(rotationVector);
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask)) _ = hit.point;
            if (effect.Equals("LensFlare")) Instantiate(lensFlare, hit.point, Quaternion.identity, controller.transform);
            if (effect.Equals("SoapBubble")) Instantiate(soapBubble, hit.point, Quaternion.identity, controller.transform);
            yield return new WaitForSeconds(speed);
        }
    }
}
