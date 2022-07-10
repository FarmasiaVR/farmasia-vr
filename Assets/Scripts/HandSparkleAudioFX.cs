using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSparkleAudioFX : MonoBehaviour {

    private AudioSource audioFX;

    public void Start() {
        audioFX = GetComponent<AudioSource>();
        Subscribe();
    }

    public void Subscribe() {
        Events.SubscribeToEvent(WashingHands, EventType.WashingHands);
    }

    private void WashingHands(CallbackData data) {
        var liquid = (data.DataObject as HandWashingLiquid);
        if (liquid.type.Equals("HandSanitizer"))
            PlayAudioFX();
    }

    private void PlayAudioFX() {
        audioFX.Play();
    }
}
