using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class InfoBox : MonoBehaviour {

    #region Constants
    private const string PREPARATION_ROOM_MESSAGE = "Puhdastilaan vietävät ruiskut, neulat ja muut tarvikkeet ovat steriilejä ja pakattu suojapusseihin. VR-pelissä suojapussi puuttuu.";
    private const string WORKSPACE_ROOM_MESSAGE = "Tässä kohtaa laminaarikaappiin siirrettävät työvälineet ruiskutetaan etanoliliuoksella, ja kaappi pyyhitään steriilillä liinalla. Voit olettaa ne jo tehdyksi.";
    #endregion

    #region Fields
    [Header("Children")]
    [SerializeField]
    private GameObject text;
    [SerializeField]
    private RectTransform planeContainer;

    [Header("Configuration")]
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float centerOffset = 0.2f;
    [SerializeField]
    private float defaultLerpAmount;
    [SerializeField]
    private float padding;

    private float activeLerpAmount;
    private TextMeshPro textField;
    // CameraCenter is where the center of your head is, not the headset (eyes) is.
    private Vector3 CameraCenter { get => cam.position - cam.forward * centerOffset; }

    #endregion

    private void Start() {
        Subscribe();
        textField = text.GetComponent<TextMeshPro>();
        text.SetActive(false);
    }

    private void Update() {
        if (textField.enabled) {
            textField.transform.position = Vector3.Lerp(textField.transform.position, GetTargetPosition(), Time.deltaTime / activeLerpAmount);
            textField.transform.LookAt(CameraCenter);
        }
    }

    #region Event Subscriptions
    public void Subscribe() {
        Events.SubscribeToEvent(ObjectPickedUp, EventType.PickupObject);
        Events.SubscribeToEvent(GrabbedRoomDoor, EventType.RoomDoor);
    }
    #endregion

    private void ObjectPickedUp(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        }
        
        if (G.Instance.Progress.CurrentPackage.name == PackageName.EquipmentSelection) {
            ShowInfoBox(PREPARATION_ROOM_MESSAGE);
            Events.UnsubscribeFromEvent(ObjectPickedUp, EventType.PickupObject);
        }
    }

    private async void GrabbedRoomDoor(CallbackData data) {
        await Task.Delay(10);

        if (G.Instance.Progress.CurrentPackage.name == PackageName.Workspace) {
            ShowInfoBox(WORKSPACE_ROOM_MESSAGE);
            Events.UnsubscribeFromEvent(GrabbedRoomDoor, EventType.RoomDoor);
        }
    }

    private void ShowInfoBox(string message) {
        textField.text = message;
        text.SetActive(true);
        RecalculateSize();
        textField.transform.position = cam.position + cam.forward * offset.z;

        activeLerpAmount = 5;

        StartCoroutine(ReEnable());
        IEnumerator ReEnable() {
            yield return new WaitForSeconds(1);
            activeLerpAmount = defaultLerpAmount;
            yield return new WaitForSeconds(9);
            text.SetActive(false);
        }
    }

    private Vector3 GetTargetPosition() {
        Vector3 forward = new Vector3(cam.forward.x, 0, cam.forward.z).normalized;
        Vector3 right = new Vector3(cam.right.x, 0, cam.right.z).normalized;

        Vector3 targetPosition = CameraCenter + forward * offset.z + right * offset.x;
        targetPosition = new Vector3(targetPosition.x, CameraCenter.y + offset.y, targetPosition.z);
        return targetPosition;
    }

    private void RecalculateSize() {
        planeContainer.localScale = new Vector3(1 + padding, textField.preferredHeight / textField.rectTransform.rect.height + padding, 1);
    }
}