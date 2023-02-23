using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTeleport : MonoBehaviour {

    public Transform player;
    public Transform playerDestination;
    public Animator doorHandleAnimation;

    private Animator blackFade;

    /// <summary>
    /// Teleports player to the next room
    /// </summary>
    public void TeleportPlayer() {
        if (playerDestination == null) {
            Logger.Error("Player teleportation spot missing");
            return;
        }

        blackFade = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<Animator>();
        if (blackFade != null)
        {
            //Play the fade in animation
            blackFade.SetTrigger("FadeOut");
            //Play the door handle animation
            doorHandleAnimation.SetBool("isUp", false);

            //Get the length of the animation currently playing and wait for that time before teleporting the player and playing
            //the fade in animation
            StartCoroutine(TeleportPlayerAfterFadeIn(blackFade.GetCurrentAnimatorClipInfo(0)[0].clip.length));
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

    IEnumerator TeleportPlayerAfterFadeIn(float secondsToWaitBeforeTeleport)
    {
        yield return new WaitForSeconds(secondsToWaitBeforeTeleport);
        player.position = playerDestination.position;
        blackFade.SetTrigger("FadeIn");
    }
}
