using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRProtectiveClothing : MonoBehaviour
{
    private AsepticClothingPoster[] posters;

    public ClothingType type;

    private XRBaseInteractable interactable;
    private ProtectiveClothing legacyObject;

    protected void Start()
    {
        posters = FindObjectsOfType<AsepticClothingPoster>();
        interactable = GetComponent<XRBaseInteractable>();
        legacyObject = new ProtectiveClothing();
    }

    // OnTriggerEnter is called when two GameObjects collide
    private void OnTriggerEnter(Collider other)
    {
        // PlayerCollider can be found attached to the VRPlayers camera object
        if (other.CompareTag("PlayerCollider") && interactable.isSelected)
        {
            //Convert the current object to a legacy ProtectiveClothing object so that all the events are fired correctly
            legacyObject.type = type;

            Events.FireEvent(EventType.ProtectiveClothingEquipped, CallbackData.Object(legacyObject));

            // Highlights the equipped item in every aseptic clothing poster found throughout the scene
            foreach (AsepticClothingPoster poster in posters) poster.HighlightText(type);

            // Play a sound to indicate a piece of clothing was succesfully equipped
            G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);

            Destroy(gameObject);
        }
    }
}
