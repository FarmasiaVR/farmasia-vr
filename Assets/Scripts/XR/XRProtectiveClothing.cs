using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRProtectiveClothing : MonoBehaviour
{
    private GameObject[] posters;

    public ClothingType type;

    protected void Start()
    {
        posters = GameObject.FindGameObjectsWithTag("AsepticClothingPoster");
        Debug.Log(posters);
        foreach (GameObject poster in posters) poster.GetComponent<AsepticClothingPoster>();
    }

    // OnTriggerEnter is called when two GameObjects collide
    private void OnTriggerEnter(Collider other)
    {
        // Checking if we are colliding with PlayerCollider and updating insideCollider to be true
        // PlayerCollider can be found attached to the VRPlayers camera object
        if (other.CompareTag("PlayerCollider"))
        {
            Events.FireEvent(EventType.ProtectiveClothingEquipped, CallbackData.Object(this));

            // Highlights the equipped item in every aseptic clothing poster found throughout the scene
            foreach (GameObject poster in posters) poster.GetComponent<AsepticClothingPoster>().HighlightText(type);

            // Play a sound to indicate a piece of clothing was succesfully equipped
            G.Instance.Audio.Play(AudioClipType.TaskCompletedBeep);

            Destroy(gameObject);
        }
    }
}
