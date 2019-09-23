using UnityEngine;

public class UISystem : MonoBehaviour {
    #region Fields
    public static UISystem Instance { get; private set; }
    [SerializeField]
    public GameObject handUI;
    [SerializeField]
    public GameObject popup;
    private GameObject currentPopup;
    #endregion

    #region Initialisation
    /// <summary>
    /// Used to instantiate Singleton of UISystem.
    /// </summary>
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Sets the current Popup. Removes old one if it exists.
    /// </summary>
    /// <param name="message">Reference to the object to place.</param>
    private void SetCurrentPopup(GameObject message) {
        if (currentPopup != null) {
            Destroy(currentPopup);
        }
        currentPopup = message;
    }

    private GameObject PopupInit() {
        GameObject popupMessage = Instantiate(popup, handUI.transform.position + popup.transform.position, popup.transform.rotation);
        popupMessage.transform.SetParent(handUI.transform, true);
        return popupMessage;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Sets current Popup as null.
    /// </summary>
    public void DeleteCurrent() {
        currentPopup = null;
    }

    /// <summary>
    /// Creates a Popup with given specifications and sets it as current.
    /// </summary>
    /// <param name="point">Amount of points for the task. Some tasks do not use this.</param>
    /// <param name="message">Message to be displayed for the player.</param>
    /// <param name="type">Type of message. Different types have different colours.</param>
    public void CreatePopup(int point, string message, MessageType type) {
        GameObject popupMessage = PopupInit();
        popupMessage.GetComponent<PointPopup>().SetPopup(point, message, type);
        SetCurrentPopup(popupMessage);
    }

    public void CreatePopup(string message, MessageType type) {
        GameObject popupMessage = PopupInit();
        popupMessage.GetComponent<PointPopup>().SetPopup(message, type);
        SetCurrentPopup(popupMessage);
    }
    #endregion
}
