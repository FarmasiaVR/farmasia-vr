using UnityEngine;


public class MenuInterface : MonoBehaviour {

    [SerializeField]
    private GameObject menuContainer;



    private Hand hand;
    private VRPadClick click;

    public bool Visible => menuContainer.activeSelf;

    public void Close() {
        menuContainer.SetActive(!Visible);
    }

    private void Start() {
        hand = GameObject.FindGameObjectWithTag("ControllerLeft")?.GetComponent<Hand>();
    }

    private void Update() {
        if (hand != null && VRInput.GetControlDown(hand.HandType, ControlType.Menu)) {
            Close();
        }
    }
}
