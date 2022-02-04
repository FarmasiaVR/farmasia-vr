using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WritingOptions : MonoBehaviour {
    [SerializeField]
    private bool visible;
    private GameObject toggle;

    void Start() {
        toggle = transform.GetChild(0).gameObject;
    }

    public void SetVisible(bool visible) {
        toggle.SetActive(visible);
    }

    // Update is called once per frame
    void Update() {

    }
}
