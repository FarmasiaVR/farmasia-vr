using System;
using UnityEngine;

public class ProgressBar : MonoBehaviour {
    [SerializeField]
    private MeshRenderer mesh;
    [SerializeField]
    private bool done;

    public bool Done {
        get { return done; }
    }
    public bool grabbing = false;
    private const float MAX = 1;
    private const float MINIMUM = 0;
    private const float SPEED = 0.005f;
    [SerializeField]
    private float currentPercentage = 0;

    public void Start() {
        done = false;
    }

    private void Update() {
        if (grabbing) {
            currentPercentage += SPEED;
            if (currentPercentage >= MAX) {
                currentPercentage = MAX;
                done = true;
            }
        } else {
            if (currentPercentage > MINIMUM) {
                currentPercentage -= SPEED;
                if (currentPercentage < MINIMUM) {
                    currentPercentage = 0;
                }
            }
        }
        UpdatePosition();
    }

    private void UpdatePosition() {
        transform.localScale = new Vector3(currentPercentage, transform.localScale.y, transform.localScale.z);
        if (mesh != null) {
            mesh.enabled = currentPercentage > 0;
        }
    }

}
