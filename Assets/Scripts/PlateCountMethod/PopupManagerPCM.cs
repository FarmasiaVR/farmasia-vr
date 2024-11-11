using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FarmasiaVR.New;

public class PopupManagerPCM : MonoBehaviour
{
    public GameObject popupPrefab;

    /// <summary>
    /// Used to create the popup object in the scene
    /// </summary>
    /// <returns>The popup object</returns>
    private PointPopup InstantiatePopup()
    {
        GameObject popupObject = Instantiate(popupPrefab);
        PointPopup pointPopup = popupObject.GetComponent<PointPopup>();

        pointPopup.SetCamera(GameObject.FindGameObjectWithTag("MainCamera"));

        return pointPopup;
    }

    /// <summary>
    /// Shows a green completed text popup for laboratorytour
    /// </summary>
    /// <param name="task">The task that was completed</param>
    public void TaskCompletePopup(Task task)
    {
        PointPopup pointPopup = InstantiatePopup();
        
        string popupLocal = task.key + "Popup";

        pointPopup.SetPopup(task.awardedPoints, Translator.Translate("PlateCountMethod", "MissionAccomplished") + " " + Translator.Translate("PlateCountMethod", popupLocal), MsgType.Done);

    }

    /// <summary>
    /// Shows a red error message popup
    /// </summary>
    /// <param name="mistake">The mistake that was made</param>
    public void TaskErrorPopup(Mistake mistake)
    {
        PointPopup pointPopup = InstantiatePopup();

        pointPopup.SetPopup(mistake.pointsDeducted*-1, mistake.mistakeText, MsgType.Mistake);
    }

    /// <summary>
    /// Shows a yellow notification text to the player. This should be used to control the player's behaviour without punishing them.
    /// </summary>
    /// <param name="notification">The message that should be displayed to the player</param>
   public void NotifyPopup(string notification)
    {
        PointPopup pointPopup = InstantiatePopup();

        pointPopup.SetPopup(notification, MsgType.Notify);
    }
}
