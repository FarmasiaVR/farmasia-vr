using UnityEngine;
using TMPro;

public class PointPopup : MonoBehaviour {

    #region fields
    private AudioSource sfx;

    private float timer = 0.0f;
    private float visualTime = 3.0f;
    private float fadeInAndOut = 2.0f;

    private float red = 0.0f;
    private float green = 0.0f;
    private float blue = 0.0f;
    private float transparency = 0.0f;

    private bool fadeInCompleted = false;
    private bool visualCompleted = false;

    private GameObject textObject;
    private TextMeshPro textField;
    private Color colour;
    #endregion

    void Awake() {
        textObject = transform.gameObject;
        textField = textObject.GetComponent<TextMeshPro>();
        textField.color = new Color(red, green, blue, 0);

        sfx = GetComponent<AudioSource>();
        sfx.enabled = false;
    }

    public void setPopup(int point, string text, MessageType type) {
        switch (type) {
            case MessageType.Error:
                red = 0;
                green = 0;
                blue = 0;
                break;
            case MessageType.Mistake:
                red = 255;
                green = 0;
                blue = 0;
                break;
            case MessageType.Notify:
                red = 255;
                green = 255;
                blue = 0;
                break;
            case MessageType.Warning:
                red = 255;
                green = 147;
                blue = 0;
                break;
        }
        textField.text = text + "\n" + point;
    }

    private void Update() {
        timer += Time.deltaTime;

        if (!fadeInCompleted) {
            transparency += 1.0f / (fadeInAndOut / Time.deltaTime);
            textField.color = new Color(red, green, blue, transparency);
            textObject.transform.position += new Vector3(0, 0.01f, 0);
            if (timer > fadeInAndOut) {

                textField.color = new Color(red, green, blue, transparency);
                timer -= fadeInAndOut;
                fadeInCompleted = true;
                sfx.enabled = true;
            }

        } else {
            if (!visualCompleted) {

                if (timer > visualTime) {
                    timer -= visualTime;
                    visualCompleted = true;
                }

            } else {
                transparency -= 1.0f / (fadeInAndOut / Time.deltaTime);
                textField.color = new Color(red, green, blue, transparency);
                textObject.transform.position += new Vector3(0, -0.01f, 0);
                if (timer > fadeInAndOut) {
                    timer -= fadeInAndOut;
                    Destroy(transform.gameObject);
                }
            }
        }
    }
}
