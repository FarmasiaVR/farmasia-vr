using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour {
    #region Fields
    public static UISystem Instance { get; private set; }

    private GameObject handuiInScene;
    [SerializeField]
    [Tooltip("Drag Popup prefab here!")]
    private GameObject popupPrefab;
    [SerializeField]
    [Tooltip("Drag Description prefab here!")]
    private GameObject descriptionPrefab;

    public GameObject player { get; private set; }
    private Hand hand;
    [SerializeField]
    [Tooltip("Defines if description is hidden or not.")]
    private bool visible = true;
    private GameObject currentPopup;
    private GameObject currentDescription;
    private MeshRenderer descriptionMesh;
    private Description description;
    #endregion

    #region Actions
    private bool swipeUpStarted = false;
    private float continuousUp = 0.0f;
    private bool swipeDownStarted = false;
    private float continuousDown = 0.0f;
    private float oldPadValue = 0.0f;
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
        player = GameObject.FindGameObjectWithTag("Player");
        handuiInScene = GameObject.FindGameObjectWithTag("HandUI");
        hand = player.transform.Find("Controller (left)").gameObject.GetComponent<Hand>();
    }

    /// <summary>
    /// Initiates UIComponent into players hand.
    /// </summary>
    /// <returns>Reference to the instantiated GameObject</returns>
    private GameObject InitUIComponent(GameObject gobj) {
        GameObject uiComponent = Instantiate(gobj, handuiInScene.transform.position + gobj.transform.localPosition, handuiInScene.transform.rotation * gobj.transform.rotation);
        uiComponent.transform.SetParent(handuiInScene.transform, true);
        return uiComponent;
    }
    #endregion

    #region Private Methods
    private void ResetSwipeUpChecking() {
        swipeUpStarted = false;
        continuousUp = 0.0f;
    }
    private void ResetSwipeDownChecking() {
        swipeDownStarted = false;
        continuousDown = 0.0f;
    }
    #endregion

    #region MonoBehaviour Methods
    /// <summary>
    /// Used for handling Description change and visibility with VRInput.
    /// Swipe up puts visibility on, Swipe down hides it.
    /// Swipe left and right change descriptions.
    /// </summary>
    private void Update() {
        if (!hand.IsGrabbed) {
            if (VRInput.LeftPadValue.y == 0.0f && oldPadValue == 0.0f) {
                if (swipeDownStarted) {
                    ResetSwipeDownChecking();
                }
                if (swipeUpStarted) {
                    ResetSwipeUpChecking();
                }
            }
            if (VRInput.LeftPadValue.y < 0.0f) {
                swipeUpStarted = true;
            }
            if (VRInput.LeftPadValue.y > 0.0f) {
                swipeDownStarted = true;
            }
            if (swipeUpStarted) {
                float valueUpDifference = VRInput.LeftPadValue.y - oldPadValue;
                if (valueUpDifference >= 0) {
                    continuousUp += valueUpDifference;
                    if (continuousUp > 1.0f) {
                        SetDescriptionVisibility(true);
                        ResetSwipeUpChecking();
                    }
                }
            }
            if (swipeDownStarted) {
                float valueDownDifference = oldPadValue - VRInput.LeftPadValue.y;
                if (valueDownDifference >= 0) {
                    continuousDown += valueDownDifference;
                    if (continuousDown > 1.0f) {
                        SetDescriptionVisibility(false);
                        ResetSwipeUpChecking();
                    }
                }
            }
            oldPadValue = VRInput.LeftPadValue.y;
        }
    }
    #endregion

    #region Popup Methods
    private void SetCurrentPopup(GameObject newPopup) {
        if (currentPopup != null) {
            Destroy(currentPopup);
        }
        currentPopup = newPopup;
        description.SetTransparency(false);
    }

    /// <summary>
    /// Sets current Popup as null.
    /// </summary>
    public void DeleteCurrent() {
        currentPopup = null;
        description.SetTransparency(true);
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

    #region Description Methods
    private void SetDescriptionVisibility(bool on) {
        visible = on;
        descriptionMesh.enabled = on;
    }

    private void SetCurrentDescription(GameObject newDescription) {
        if (currentDescription != null) {
            Destroy(currentDescription);
        }
        currentDescription = newDescription;
        description = currentDescription.GetComponent<Description>();
        descriptionMesh = currentDescription.GetComponent<MeshRenderer>();
    }

    public void UpdateDescription(List<ITask> tasks) {
        GameObject desc = currentDescription;
        if (desc == null) {
            desc = InitUIComponent(descriptionPrefab);
            description = desc.GetComponent<Description>();
            SetCurrentDescription(desc);
        }
        description.SetActiveList(tasks);
        description.MovePointerAndDescToFirst();
    }
    #endregion
}
