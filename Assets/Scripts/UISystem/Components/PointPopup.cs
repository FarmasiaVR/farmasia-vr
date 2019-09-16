using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointPopup : MonoBehaviour {
    private bool executeOnce = false;
    private int lifetime = 300;
    private string text;
    private int point;
    [SerializeField]
    private GameObject textObject;
    [SerializeField]
    private Text textField;

    public PointPopup() {


    }

    void Start() {
        textObject = transform.gameObject;
        textField = textObject.GetComponent<Text>();
        UISystem.Instance.addChild(textObject);

    }

    public void setPointAndText(int point, string text) {
        this.text = text;
        this.point = point;
    }

    public void setText() {
        textField.text = text + "\n" + point;
    }


    private void Update() {
        if (!executeOnce) {
            executeOnce = true;
            setText();
        }


        if (textObject != null) {

            if (lifetime <= 0) {
                Logger.Print("DELETE!");
                Destroy(transform.gameObject);

            }
            textObject.transform.position += new Vector3(0, 2, 0);
            lifetime--;
        }

    }
}
