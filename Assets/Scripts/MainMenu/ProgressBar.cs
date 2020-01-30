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
    [SerializeField]
    private float speed = 0.005f;
    [SerializeField]
    private float currentPercentage = 0;
    public bool instant = false;


    public void Start() {
        done = false;
    }

    private void Update() {
        if (instant) {
            currentPercentage = MAX;
            return;
        }
        if (grabbing) {
            currentPercentage += speed;
            if (currentPercentage >= MAX) {
                currentPercentage = MAX;
                done = true;
            }
        } else {
            if (currentPercentage > MINIMUM) {
                currentPercentage -= speed;
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
