using System.Collections.Generic;
using TMPro;
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

    #region Arrows and Number
    private TextMeshPro arrowLeft;
    private bool arrowLeftVisible;
    private TextMeshPro arrowRight;
    private bool arrowRightVisible;
    private TextMeshPro taskNumber;
    private bool taskNumberVisible;
    #endregion

    #region Actions
    private bool swipeUp = false;
    private bool swipeDown = false;
    private bool swipeLeft = false;
    private bool swipeRight = false;
    private float up = 0.0f;
    private float down = 0.0f;
    private float left = 0.0f;
    private float right = 0.0f;
    private float oldYValue = 0.0f;
    private float oldXValue = 0.0f;
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
        hand = GameObject.FindGameObjectWithTag("Controller (Left)").GetComponent<Hand>();
        arrowLeft = GameObject.FindGameObjectWithTag("Arrow (Left)").GetComponent<TextMeshPro>();
        arrowRight = GameObject.FindGameObjectWithTag("Arrow (Right)").GetComponent<TextMeshPro>();
        taskNumber = GameObject.FindGameObjectWithTag("TaskNumber").GetComponent<TextMeshPro>();
    }

    /// <summary>
    /// Initiates UIComponent into players hand.
    /// </summary>
    /// <returns>Reference to the instantiated GameObject</returns>
    private GameObject InitUIComponent(GameObject gobj) {
        GameObject uiComponent = Instantiate(gobj, handuiInScene.transform);
        uiComponent.transform.SetParent(handuiInScene.transform, true);
        return uiComponent;
    }
    #endregion

    #region Private Methods
    private void ResetSwipe(bool swipeDirection, float swipeAmount) {
        swipeDirection = false;
        swipeAmount = 0.0f;
    }
    private void ResetSwipeDownChecking() {
        swipeDown = false;
        down = 0.0f;
    }
    #endregion

    #region MonoBehaviour Methods
    /// <summary>
    /// Used for handling Description change and visibility with VRInput.
    /// Swipe up puts visibility on, Swipe down hides it.
    /// Swipe left and right change descriptions.
    /// </summary>
    private void Update() {
        KeyListener();

        if (!hand.IsGrabbed) {
            if (VRInput.LeftPadValue.y == 0.0f && oldYValue == 0.0f) {
                if (swipeDown) {
                    ResetSwipe(swipeDown, down);
                }
                if (swipeUp) {
                    ResetSwipe(swipeUp, up);
                }
            }
            if (VRInput.LeftPadValue.x == 0.0f && oldXValue == 0.0f) {
                if (swipeLeft) {
                    ResetSwipe(swipeLeft, left);
                }
                if (swipeRight) {
                    ResetSwipe(swipeRight, right);
                }
            }
            if (VRInput.LeftPadValue.y < 0.0f) {
                swipeUp = true;
            }
            if (VRInput.LeftPadValue.y > 0.0f) {
                swipeDown = true;
            }
            if (VRInput.LeftPadValue.x < 0.0f) {
                swipeRight = true;
            }
            if (VRInput.LeftPadValue.x > 0.0f) {
                swipeLeft = true;
            }
            if (swipeLeft) {
                float valueUpDifference = Mathf.Abs(oldXValue) - Mathf.Abs(VRInput.LeftPadValue.x);
                if (valueUpDifference >= 0) {
                    right += valueUpDifference;
                    if (right > 1.0f) {
                        NextDescription(true);
                        ResetSwipe(swipeLeft, left);
                    }
                }
            }
            if (swipeLeft) {
                float valueUpDifference = Mathf.Abs(VRInput.LeftPadValue.x) - Mathf.Abs(oldXValue);
                if (valueUpDifference >= 0) {
                    left += valueUpDifference;
                    if (left > 1.0f) {
                        NextDescription(false);
                        ResetSwipe(swipeLeft, left);
                    }
                }
            }

            if (swipeUp) {
                float valueUpDifference = VRInput.LeftPadValue.y - oldYValue;
                if (valueUpDifference >= 0) {
                    up += valueUpDifference;
                    if (up > 1.0f) {
                        SetDescriptionVisibility(true);
                        ResetSwipe(swipeUp, up);
                    }
                }
            }
            if (swipeDown) {
                float valueDownDifference = oldYValue - VRInput.LeftPadValue.y;
                if (valueDownDifference >= 0) {
                    down += valueDownDifference;
                    if (down > 1.0f) {
                        SetDescriptionVisibility(false);
                        ResetSwipe(swipeDown, down);
                    }
                }
            }
            oldYValue = VRInput.LeftPadValue.y;
            oldXValue = VRInput.LeftPadValue.x;
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

    private void SetArrowVisibility() {
        int value = description.CheckPointerEdge();
        if (value == -2) {
            arrowLeft.enabled = false;
            arrowRight.enabled = false;
            return;
        }
        if (value == 1) {
            arrowLeft.enabled = true;
            arrowRight.enabled = false;
            return;
        }
        if (value == -1) {
            arrowLeft.enabled = false;
            arrowRight.enabled = true;
            return;
        }
        arrowLeft.enabled = true;
        arrowRight.enabled = true;
    }

    private void setPointerNumber() {
        int value = (description.getPointer() + 1);
        int count = description.getActiveList().Count;
        if (count == 0 || count == 1) {
            taskNumber.text = "";
            return;
        }
        taskNumber.text = "" + value;
    }

    private void NextDescription(bool right) {
        if (right) {
            description.MoveDescWithPointer(1);
        } else {
            description.MoveDescWithPointer(-1);
        }
        setPointerNumber();
        SetArrowVisibility();
    }

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
        setPointerNumber();
        SetArrowVisibility();
    }
    #endregion

    #region Test Functions
    private void KeyListener() {
        if (Input.GetKeyDown(KeyCode.C)) {
            CreatePopup("THIS IS A TEST", MessageType.Mistake);
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            NextDescription(true);
        }
        if (Input.GetKeyDown(KeyCode.V)) {
            NextDescription(false);
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            description.RemoteFinishShownTask();
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            foreach (ITask task in description.getActiveList()) {
                Logger.Print(task.GetType());
            }
        }
    }
    #endregion

}