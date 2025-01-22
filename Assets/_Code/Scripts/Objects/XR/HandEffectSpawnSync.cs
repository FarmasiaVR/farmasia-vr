using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandEffectSpawnSync : MonoBehaviour
{
    ///<summary>
    ///This script is used to place the effect spawners from the hand prefab to the hand state manager that is attached to the VR player.
    ///This is because the hands are spawned as an instance on runtime and it isn't possible to make references to objects inside of a prefab.
    /// </summary>
    /// 

    public HandEffectSpawner effectSpawner;
    public enum HandSide {
        Left,
        Right,
    }
    public HandSide handSide;

    private void Start() {
        HandStateManager manager = FindObjectOfType<HandStateManager>();
        if (handSide == HandSide.Left) effectSpawner.controller = GameObject.FindGameObjectWithTag("Controller (Left)").transform;
        else effectSpawner.controller = GameObject.FindGameObjectWithTag("Controller (Right)").transform;
        if (manager && handSide == HandSide.Left) {
            manager.leftHandEffectSpawner = effectSpawner;
        }
        else if (manager && handSide == HandSide.Right) {
            manager.rightHandEffectSpawner = effectSpawner;
        }
    }
}
