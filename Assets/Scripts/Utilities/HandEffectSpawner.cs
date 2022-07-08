using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandEffectSpawner : MonoBehaviour {

    public GameObject lensFlare;
    public GameObject soapBubble;
    public Transform controller;
    public Transform rotator;
    public LayerMask layerMask;

    private void SpawnEffect(GameObject effect) {
        for (int i = 0; i < 40; i++) {
            float randomX = Random.Range(0.0f, 360.0f);
            float randomY = Random.Range(0.0f, 360.0f);
            Vector3 rotationVector = new Vector3(randomX, randomY, 0);
            rotator.rotation = Quaternion.Euler(rotationVector);
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask)) _ = hit.point;
            Instantiate(effect, hit.point, Quaternion.identity, controller.transform);
        }
    }
}
