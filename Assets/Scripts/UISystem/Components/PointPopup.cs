using UnityEngine;
using TMPro;
using System.Collections;

public class PointPopup : MonoBehaviour {
    #region Fields
    private GameObject textObject;
    private TextMeshPro textField;
    private GameObject point1, point2;
    private Transform cam;

    private Color color;

    private const float LERP_AMOUNT = 0.05f;

    private float popupSpawnDistance = 0.5f;
    private float popupDistance = 0.5f;
    private float popupHeightOffset = -0.2f;
    private float centerOffset = 0.06f;
    // CameraCenter is where the center of your head is, not the headset (eyes) is.
    private Vector3 CameraCenter { get => cam.position - cam.forward * centerOffset; }

    #endregion

    #region Variables
    private float timer = 0.0f;
    private float visualTime = 3.0f;
    private float fadeInAndOut = 0.3f;
    private float speed = 0.007f;
    private float endPoint;
    private float distanceTravelled = 0.0f;
    private float distanceToTravel = 0.2f;
    private float startingPoint;
    private float red = 0.0f;
    private float green = 0.0f;
    private float blue = 0.0f;
    private float transparency = 0.0f;
    private bool fadeInCompleted = false;
    private bool visualCompleted = false;
    #endregion

    #region Initialisation
    private void Awake() {
        textObject = transform.gameObject;
        textField = textObject.GetComponent<TextMeshPro>();
        textField.color = new Color(red, green, blue, 0);
        timer = 0.0f;
        transparency = 0.0f;
        distanceTravelled = 0.0f;
        endPoint = transform.localPosition.z;
        startingPoint = transform.localPosition.z + distanceToTravel;
        FlipScale(-1);
    }

    private void Start() {
        CalculateStartingPosition();
    }

    public void SetObjectPath(GameObject point1, GameObject point2) {
        this.point1 = point1;
        this.point2 = point2;
    }

    #endregion


    #region Private Methods
    public void SetCamera(GameObject obj) {
        if (obj != null) {
            cam = obj.transform;
        }
    }

    private void CalculateStartingPosition() {
        textObject.transform.position = CameraCenter + cam.forward * popupSpawnDistance;
    }

    private void FlipScale(float x) {
        transform.localScale.Set(x, transform.localScale.y, transform.localScale.z);
    }

    private void Remove() {
        Destroy(transform.gameObject);
    }

    private void MoveTowardsPoint(Vector3 vector) {
        if (vector == null) {
            return;
        }
        Vector3 position = gameObject.transform.position;
        gameObject.transform.position = Vector3.Lerp(position, vector, Time.deltaTime + LERP_AMOUNT);
    }

    private void FixedUpdate() {
        Vector3 dir = new Vector3(cam.forward.x, popupHeightOffset, cam.forward.z).normalized;
        Vector3 pos = CameraCenter + dir * popupDistance;

        if (!fadeInCompleted) {
            MoveTowardsPoint(pos);
        } else {
            MoveTowardsPoint(pos);
        }

        if (cam != null) {
            transform.LookAt(cam);
        }
    }

    private void Update() {
        timer += Time.deltaTime;

        if (!fadeInCompleted) {
            textObject.transform.localPosition += textObject.transform.forward * speed;
            distanceTravelled += speed;
            transparency = distanceTravelled / distanceToTravel;
            textField.alpha = transparency;

            if (distanceTravelled > distanceToTravel) {
                transparency = 1.0f;
                textField.alpha = transparency;
                timer -= fadeInAndOut;
                fadeInCompleted = true;
                timer = 0.0f;
            }
        } else {
            if (!visualCompleted) {
                if (timer > visualTime) {
                    timer = 0.0f;
                    visualCompleted = true;
                }
            } else {
                textObject.transform.localPosition -= textObject.transform.forward * speed;
                distanceTravelled -= speed;
                transparency = distanceTravelled / distanceToTravel;
                textField.alpha = transparency;

                if (distanceTravelled <= 0.0f) {
                    timer = 0.0f;
                    Remove();
                }
            }
        }
    }

    private void SetColour(MsgType type) {
        switch (type) {
            case MsgType.Error:
                red = 0;
                green = 0;
                blue = 0;
                break;
            case MsgType.Mistake:
                red = 255;
                green = 0;
                blue = 0;
                break;
            case MsgType.Notify:
                red = 255;
                green = 255;
                blue = 0;
                break;
            case MsgType.Warning:
                red = 255;
                green = 147;
                blue = 0;
                break;
            case MsgType.Done:
                red = 0;
                green = 255;
                blue = 0;
                break;
        }
        textField.color = new Color(red, green, blue, 0);
    }
    #endregion

    #region Public Methods
    public void SetPopup(string text, MsgType type) {
        SetPopup(int.MinValue, text, type);
    }

    public void SetPopup(int point, string text, MsgType type) {
        SetColour(type);
        if (point == int.MinValue) {
            textField.text = text;
        } else if (point > 0) {
            textField.text = text + "\n+" + point.ToString();
        } else {
            textField.text = text + "\n" + point.ToString();
        }
    }
    #endregion
}