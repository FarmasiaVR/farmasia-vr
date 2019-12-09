using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    TextMeshPro textField;
    GameObject cam;
    #endregion

    private void Awake() {
        message = PREPARATION_ROOM_MESSAGE;
        Subscribe();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        textField = this.GetComponent<TextMeshPro>();
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
        transform.position = cam.transform.position;
        textField.text = message;

        //Pipeline pipe = G.Instance.Pipeline.New().Delay(100);
        var date1 = DateTime.Now;
        var date2 = DateTime.Now;
        Logger.Print("aluksi: " + date1);

        while(Convert.ToSingle((date2-date1).TotalSeconds) < 15) {
            date2 = DateTime.Now;
            //float seconds = Convert.ToSingle((date2 - date1).TotalSeconds);
            //date1 = date2;
            Logger.Print(date2);
            //G.Instance.Pipeline.Update(seconds);
        }

        textField.text = "";
    }
    #endregion
}