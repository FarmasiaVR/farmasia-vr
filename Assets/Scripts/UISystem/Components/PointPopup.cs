using UnityEngine;
using TMPro;

public class PointPopup : MonoBehaviour {

    AudioSource audio;

    private float timer = 0.0f;
    private float visualTime = 3.0f;
    private float fadeInAndOut = 2.0f;

    private float transparency = 0.0f;

    private bool fadeInCompleted = false;
    private bool visualCompleted = false;

    private string text;
    private int point;

    [SerializeField]
    private GameObject textObject;
    [SerializeField]
    private TextMeshPro textField;
    private Color colour;

    

    public PointPopup() {


    }

    void Start() {
        textObject = transform.gameObject;
        textField = textObject.GetComponent<TextMeshPro>();
        textField.color = new Color(255, 0, 0, 0);
        audio = GetComponent<AudioSource>();
        audio.enabled = false;

    }

    public void setPointAndText(int point, string text) {
        this.text = text;
        this.point = point;
    }

    public void setText() {
        textField.text = text + "\n" + point;
    }


    private void Update() {


        timer += Time.deltaTime;

        if (!fadeInCompleted) {
            transparency += 1.0f / (fadeInAndOut / Time.deltaTime);
            textField.color = new Color(255, 0, 0, transparency);
            textObject.transform.position += new Vector3(0, 0.01f, 0);
            if (timer > fadeInAndOut) {
                
                textField.color = new Color(255, 0, 0, transparency);
                timer -= fadeInAndOut;
                fadeInCompleted = true;
                audio.enabled = true;
            }

        } else {
            if (!visualCompleted) {

                if (timer > visualTime) {
                    timer -= visualTime;
                    visualCompleted = true;
                }

            } else {
                transparency -= 1.0f / (fadeInAndOut / Time.deltaTime);
                textField.color = new Color(255, 0, 0, transparency);
                textObject.transform.position += new Vector3(0, -0.01f, 0);
                if (timer > fadeInAndOut) {
                    timer -= fadeInAndOut;
                    Destroy(transform.gameObject);
                }
            }
        }

    }
}
