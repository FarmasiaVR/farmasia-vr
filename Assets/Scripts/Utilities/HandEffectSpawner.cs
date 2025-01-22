using System.Collections;
using UnityEngine;

public class HandEffectSpawner : MonoBehaviour {

    public GameObject lensFlare;
    public GameObject soapBubble;
    public Transform controller;
    public Transform rotator;
    public LayerMask layerMask;

    public IEnumerator SpawnSoapBubbles() {
        for (int i = 0; i < 450; i++) {
            rotator.rotation = Quaternion.Euler(GenerateRandomRotation());
            RaycastHit hit = FindRaycastHit();
            Instantiate(soapBubble, hit.point, Quaternion.identity, controller.transform);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator SpawnLensFlares() {
        for (int i = 0; i < 6; i++) {
            rotator.rotation = Quaternion.Euler(GenerateRandomRotation());
            RaycastHit hit = FindRaycastHit();
            Instantiate(lensFlare, hit.point, Quaternion.identity, controller.transform);
            yield return new WaitForSeconds(1.2f);
        }
    }

    private Vector3 GenerateRandomRotation() {
        float randomX = Random.Range(0.0f, 360.0f);
        float randomY = Random.Range(0.0f, 360.0f);
        return new Vector3(randomX, randomY, 0);
    }

    private RaycastHit FindRaycastHit() {
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, 100, layerMask);
        return hit;
    }
}
