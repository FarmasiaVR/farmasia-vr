using UnityEngine;

public class UISystem : MonoBehaviour {
    #region Fields
    public static UISystem Instance { get; private set; }
    [SerializeField]
    [Tooltip("If empty, drag the HandUI object from left hand into here.")]
    private GameObject handuiInScene;
    [SerializeField]
    [Tooltip("Drag Popup prefab here!")]
    private GameObject popupPrefab;
    [SerializeField]
    [Tooltip("Drag Description prefab here!")]
    private GameObject descriptionPrefab;

    private GameObject currentPopup;
    private GameObject currentDescription;
    private Description description;

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
    private void SetCurrentPopup(GameObject newPopup) {
        if (currentPopup != null) {
            Destroy(currentPopup);
        }
        currentPopup = newPopup;
        description.SetTransparency(false);
    }

    private void SetCurrentDescription(GameObject newDescription) {
        if (currentDescription != null) {
            Destroy(currentDescription);
        }
        currentDescription = newDescription;
        description = currentDescription.GetComponent<Description>();
    }

    /// <summary>
    /// Initiates UIComponent into players hand.
    /// </summary>
    /// <returns>Reference to the instantiated GameObject</returns>
    private GameObject InitUIComponent(GameObject gobj) {
        GameObject uiComponent = Instantiate(gobj, handuiInScene.transform.position + gobj.transform.position, Quaternion.Euler(handuiInScene.transform.eulerAngles + gobj.transform.eulerAngles));
        uiComponent.transform.SetParent(handuiInScene.transform, true);
        return uiComponent;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Sets current Popup as null.
    /// </summary>
    public void DeleteCurrent() {
        currentPopup = null;
        description.SetTransparency(true);
    }

    public void ChangeDescription(string descript, Color color) {
        GameObject desc = currentDescription;
        if (currentDescription == null) {
            desc = InitUIComponent(descriptionPrefab);
            description = desc.GetComponent<Description>();
            SetCurrentDescription(desc);
        }
        description.SetDescription(descript, color);
    }

    /// <summary>
    /// Used for creating a popup.
    /// </summary>
    /// <param name="point">Amount of points for the task. Some tasks do not use this.</param>
    /// <param name="message">Message to be displayed for the player.</param>
    /// <param name="type">Type of message. Different types have different colours.</param>
    public void CreatePopup(int point, string message, MessageType type) {
        GameObject popupMessage = InitUIComponent(popupPrefab);
        popupMessage.GetComponent<PointPopup>().SetPopup(point, message, type);
        SetCurrentPopup(popupMessage);
    }

    public void CreatePopup(string message, MessageType type) {
        GameObject popupMessage = InitUIComponent(popupPrefab);
        popupMessage.GetComponent<PointPopup>().SetPopup(message, type);
        SetCurrentPopup(popupMessage);
    }
    #endregion
}
