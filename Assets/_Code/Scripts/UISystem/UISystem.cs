using UnityEngine;

public class UISystem : MonoBehaviour {
    #region Fields
    public static UISystem Instance { get; private set; }

    //private GameObject handuiInScene;
    [SerializeField]
    [Tooltip("Drag Popup prefab here!")]
    private GameObject popupPrefab;

    public GameObject player { get; private set; }
    private Hand hand;

    private string descript = "";



    public string Descript { get => TaskConfig.For(G.Instance.Progress.CurrentPackage.CurrentTask.TaskType).Description; set => descript = value; }

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
        player = GameObject.FindGameObjectWithTag("MainCamera");
        //handuiInScene = GameObject.FindGameObjectWithTag("HandUI");
        hand = GameObject.FindGameObjectWithTag("Controller (Left)").GetComponent<Hand>();
        Popups.Prefab = popupPrefab;
        Popups.Player = GameObject.FindGameObjectWithTag("MainCamera");
    }

    /// <summary>
    /// Initiates UIComponent into players hand.
    /// </summary>
    /// <returns>Reference to the instantiated GameObject</returns>
    private GameObject InitUIComponent(GameObject gobj) {
        GameObject uiComponent = Instantiate(gobj);
        uiComponent.transform.position = player.transform.position;
        return uiComponent;
    }
    #endregion

    /// <summary>
    /// Used for creating a popup.
    /// </summary>
    /// <param name="point">Amount of points for the task. Some tasks do not use this.</param>
    /// <param name="message">Message to be displayed for the player.</param>
    /// <param name="type">Type of message. Different types have different colours.</param>
    public void CreatePopup(string message, MsgType type) {
        Popups.CreatePopup(this, int.MinValue, message, type);
    }

    public void CreatePopup(int point, string message, MsgType type) {
        Popups.CreatePopup(this, point, message, type);
    }
}