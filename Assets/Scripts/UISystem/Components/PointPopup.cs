using UnityEngine;
using TMPro;
public class PointPopup : MonoBehaviour {
    #region Fields
    private GameObject textObject;
    private TextMeshPro textField;
    private Color color;
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
    #endregion

    #region Private Methods
    /// <summary>
    /// Calculates and sets transform starting position. Used for animation.
    /// </summary>
    private void CalculateStartingPosition() {
        textObject.transform.localPosition = new Vector3(textObject.transform.localPosition.x, textObject.transform.localPosition.y, textObject.transform.localPosition.z + startingPoint);
    }

    /// <summary>
    /// Flips the scale of the Popup. This is required depending on does it follow rotation or camera.
    /// </summary>
    /// <param name="x">-1 = Inverted, 1 = Normal</param>
    private void FlipScale(float x) {
        transform.localScale.Set(x, transform.localScale.y, transform.localScale.z);
    }

    /// <summary>
    /// Called only if popup lifespan is at the end.
    /// Removes current Popup from UISystem and then destroys itself.
    /// Object might be destoyed earlier by another popup in UISystem.
    /// </summary>
    private void Remove() {
        UISystem.Instance.DeleteCurrent();
        Destroy(transform.gameObject);
    }

    private void Start() {
        CalculateStartingPosition();
    }

    /// <summary>
    /// Used for animating the Popup. Once animation is done, Popup destroys itself.
    /// </summary>
    private void Update() {
        timer += Time.deltaTime;
        if (!fadeInCompleted) {
            textObject.transform.localPosition += new Vector3(0, 0, -speed);
            distanceTravelled += speed;
            transparency = distanceTravelled / distanceToTravel;
            textField.alpha = transparency;

            if (distanceTravelled > distanceToTravel) {
                transparency = 1.0f;
                textField.alpha = transparency;
                timer -= fadeInAndOut;
                fadeInCompleted = true;
                AudioManager.Play(AudioClipType.Diu);
                timer = 0.0f;
            }
        } else {
            if (!visualCompleted) {
                if (timer > visualTime) {
                    timer = 0.0f;
                    visualCompleted = true;
                }
            } else {
                textObject.transform.localPosition += new Vector3(0, 0, speed);
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
    /// <summary>
    /// Used for changing default text and type of instantiated popup.
    /// </summary>
    /// <param name="text">Message showed to player.</param>
    /// <param name="type">Message Type changes message's colour.</param>
    public void SetPopup(string text, MsgType type) {
        SetColour(type);
        textField.text = text;
    }

    /// <summary>
    /// Used for changing default point, text and type of copied popup (from prefab).
    /// </summary>
    /// <param name="point">Amount of points gained from task completion. (or failure).</param>
    /// <param name="text">Message showed to player.</param>
    /// <param name="type">Message Type changes message's colour.</param>
    public void SetPopup(int point, string text, MsgType type) {
        SetColour(type);
        if (point > 0) {
            textField.text = text + "\n+" + point;
        } else {
            textField.text = text + "\n" + point;
        }
    }
    #endregion
}
