using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTeleport : MonoBehaviour {

    public Transform player;
    public Transform playerDestination;
    public Animator doorHandleAnimation;


    private CameraFadeController fadeController;

    /// <summary>
    /// Teleports player to the next room
    /// </summary>
    public void TeleportPlayer() {
        if (playerDestination == null) {
            Logger.Error("Player teleportation spot missing");
            return;
        }

        fadeController = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<CameraFadeController>();
        if (fadeController != null)
        {
            //Play the fade in animation
            fadeController.onFadeOutComplete.AddListener(TeleportPlayerAfterFadeOut);
            fadeController.BeginFadeOut();
            //Play the door handle animation
            doorHandleAnimation.SetBool("isUp", false);
        }
        //If the animator for the level changer and the fade animations isn't found, then that probably means we are
        //using legacy code and we can just teleport the player
        else
        {
            player.position = playerDestination.position;
        }

        // Making sure we don't bring any grabbed items with us when teleporting
        //this code is depricated
        /*foreach (VRActionsMapper hand in VRInput.Hands) {
            hand.Hand.GrabUninteract();
            hand.Hand.Uninteract();
        }
        */

        /*MedicinePreparationScene m = G.Instance.Scene as MedicinePreparationScene;
        if (!m.Restarted || MedicinePreparationScene.SavedScoreState == null) {
            m.SaveProgress();
        }*/
    }

    public void TeleportPlayerAfterFadeOut()
    {
        player.position = playerDestination.position;
        //Update the position of the hint so that it is in the right place.
        G.Instance.Progress.UpdateHint();
        fadeController.BeginFadeIn();
    }
}
