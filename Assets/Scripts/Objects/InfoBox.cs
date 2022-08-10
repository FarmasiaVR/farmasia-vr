using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoBox : MonoBehaviour {

    private const string PREPARATION_ROOM_MESSAGE = "Puhdastilaan vietävät ruiskut, neulat ja muut tarvikkeet ovat steriilejä ja pakattu suojapusseihin. VR-pelissä suojapussi puuttuu.";
    private const string WORKSPACE_ROOM_MESSAGE = "Tässä kohtaa laminaarikaappiin siirrettävät työvälineet ruiskutetaan etanoliliuoksella, ja kaappi pyyhitään steriilillä liinalla. Voit olettaa ne jo tehdyksi.";
    private const string CHANGING_ROOM_MESSAGE = "Tässä vaiheessa valmisteltaisiin työvälineet.";
    private const string WASHING_HANDS_MESSAGE = "Käsiä kuuluisi pestä vähintään 30 sekuntia. VR-pelissä riittää alle 10 sekuntia.";
    private const string CLEANING_LAMINAR_CABINET_MESSAGE = "Ruiskutuksen jälkeen seinät kuivattaisiin paperilla. Tätä vaihetta pelissä ei ole.";

    [Header("Children")]
    public GameObject text;
    public RectTransform planeContainer;

    [Header("Configuration")]
    public Transform cam;
    public Vector3 offset = new Vector3(0.0f, 0.2f, 0.5f);
    public float centerOffset = 0.06f;
    public float defaultLerpAmount = 0.3f;
    public float padding = 0.1f;

    private TextMeshPro textField;
    // CameraCenter is where the center of your head is, not the headset (eyes) is
    private Vector3 CameraCenter { get => cam.position - cam.forward * centerOffset; }
    private float activeLerpAmount;

    private void Start() {
        Subscribe();
        textField = text.GetComponent<TextMeshPro>();
        text.SetActive(false);
    }

    private void Update() {
        if (textField.enabled && activeLerpAmount > 0.01f) {
            textField.transform.position = Vector3.Lerp(textField.transform.position, GetTargetPosition(), Time.deltaTime / activeLerpAmount);
            textField.transform.LookAt(CameraCenter);
        }
    }

    public void Subscribe() {
        Events.SubscribeToEvent(ObjectPickedUp, EventType.PickupObject);
        Events.SubscribeToEvent(GrabbedRoomDoor, EventType.RoomDoor);
        Events.SubscribeToEvent(TrackProgress, EventType.ProtectiveClothingEquipped);
        Events.SubscribeToEvent(HandsTouched, EventType.WashingHands);
        Events.SubscribeToEvent(TrackWallsCleaned, EventType.CleaningBottleSprayed);
    }

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
        await System.Threading.Tasks.Task.Delay(10);

        if (G.Instance.Progress.CurrentPackage.name == PackageName.Workspace) {
            ShowInfoBox(WORKSPACE_ROOM_MESSAGE);
            Events.UnsubscribeFromEvent(GrabbedRoomDoor, EventType.RoomDoor);
        }
    }

    private async void TrackProgress(CallbackData data) {
        await System.Threading.Tasks.Task.Delay(10);

        if (G.Instance.Progress.CurrentPackage.doneTypes.Contains(TaskType.WearHeadCoverAndFaceMask)) {
            ShowInfoBox(CHANGING_ROOM_MESSAGE);
            Events.UnsubscribeFromEvent(TrackProgress, EventType.ProtectiveClothingEquipped);
        }
    }

    private async void HandsTouched(CallbackData data) {
        await System.Threading.Tasks.Task.Delay(10);

        var liquidUsed = (data.DataObject as HandWashingLiquid);
        if (liquidUsed.type == "Water" && G.Instance.Progress.CurrentPackage.doneTypes.Contains(TaskType.WashGlasses)) {
            ShowInfoBox(WASHING_HANDS_MESSAGE);
            Events.UnsubscribeFromEvent(HandsTouched, EventType.WashingHands);
        }
    }

    private async void TrackWallsCleaned(CallbackData data) {
        await System.Threading.Tasks.Task.Delay(10);

        if (G.Instance.Progress.CurrentPackage.name == PackageName.CleanUp) {
            ShowInfoBox(CLEANING_LAMINAR_CABINET_MESSAGE);
            Events.UnsubscribeFromEvent(TrackWallsCleaned, EventType.CleaningBottleSprayed);
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
