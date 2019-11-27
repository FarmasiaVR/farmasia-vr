using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBox : MonoBehaviour {

    #region Constants
    private const string PREPARATION_ROOM_MESSAGE = "Työtilaan vietävät työvälineet ruiskutetaan etanoliliuoksella ennen, kuin ne voidaan siirtää huoneesta toiseen. Voit olettaa sen jo tehdyksi.";
    private const string WORKSPACE_ROOM_MESSAGE = "Tässä kohtaa laminaarikaappiin siirrettävät työvälineet ruiskutetaan etanoliliuoksella. Voit olettaa sen jo tehdyksi.";
    #endregion

    #region Fields
    private string message;
    private bool preparationMessageShown = false;
    private bool workspaceMessageShown = false;
    #endregion

    private void Awake() {
        message = PREPARATION_ROOM_MESSAGE;
        Subscribe();
    }

    #region Event Subscriptions
    public void Subscribe() {
        Events.SubscribeToEvent(ObjectPickedUp, EventType.PickupObject);
        Events.SubscribeToEvent(HandleGrabbed, EventType.HandleGrabbed);
    }

    private void ObjectPickedUp(CallbackData data) {
        GameObject g = data.DataObject as GameObject;
        GeneralItem item = g.GetComponent<GeneralItem>();
        if (item == null) {
            return;
        } else if ((G.Instance.Progress.CurrentPackage.name == PackageName.EquipmentSelection)  && !preparationMessageShown) {
            ShowInfoBox();
            Events.UnsubscribeFromEvent(ObjectPickedUp, EventType.PickupObject);
            preparationMessageShown = true;
            message = WORKSPACE_ROOM_MESSAGE;
        }
    }

    private void HandleGrabbed(CallbackData data) {
        if ((G.Instance.Progress.CurrentPackage.name == PackageName.Workspace) && !workspaceMessageShown) {
            ShowInfoBox();
            Events.UnsubscribeFromEvent(HandleGrabbed, EventType.HandleGrabbed);
            workspaceMessageShown = true;  
        }
    }

    private void ShowInfoBox() {
    }
    #endregion

    protected void Activate() {
    }
}