using UnityEngine;
using TMPro;
public class PointPopup : MonoBehaviour {
    #region Fields
    private AudioSource sound;
    private GameObject textObject;
    private TextMeshPro textField;
    private Color color;
    #endregion

    #region Variables
    private float timer = 0.0f;
    private float visualTime = 3.0f;
    private float fadeInAndOut = 0.7f;
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
        sound = GetComponent<AudioSource>();
        sound.enabled = false;
    }
    #endregion

    #region Private Methods
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

    /// <summary>
    /// Used for animating the Popup. Once animation is done, Popup destroys itself.
    /// </summary>
    private void Update() {
        timer += Time.deltaTime;
        if (!fadeInCompleted) {
            transparency += 1.0f / (fadeInAndOut / Time.deltaTime);
            textField.color = new Color(red, green, blue, transparency);
            textObject.transform.localPosition += new Vector3(0, 0.01f, 0);
            if (timer > fadeInAndOut) {

                textField.color = new Color(red, green, blue, transparency);
                timer -= fadeInAndOut;
                fadeInCompleted = true;
                sound.enabled = true;
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
                textObject.transform.localPosition += new Vector3(0, -0.01f, 0);
                if (timer > fadeInAndOut) {
                    timer -= fadeInAndOut;
                    Remove();
                }
            }
        }
    }


    private void SetColour(MessageType type) {
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
            case MessageType.Done:
                red = 0;
                green = 255;
                blue = 0;
                break;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Used for changing default text and type of instantiated popup.
    /// </summary>
    /// <param name="text">Message showed to player.</param>
    /// <param name="type">Message Type changes message's colour.</param>
    public void SetPopup(string text, MessageType type) {
        SetColour(type);
        textField.text = text;
    }

    /// <summary>
    /// Used for changing default point, text and type of copied popup (from prefab).
    /// </summary>
    /// <param name="point">Amount of points gained from task completion. (or failure).</param>
    /// <param name="text">Message showed to player.</param>
    /// <param name="type">Message Type changes message's colour.</param>
    public void SetPopup(int point, string text, MessageType type) {
        SetColour(type);
        textField.text = text + "\n" + point;
    }
    #endregion
}
