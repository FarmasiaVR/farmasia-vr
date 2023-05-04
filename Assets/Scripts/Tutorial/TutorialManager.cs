using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This is a script that handles the transitions between different tutorial scenarios in the tutorial.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Tooltip("From which tutorial the player should start. Only used for debugging purposes.")]
    public int startFromTutorialIndex;
    private int currentTutorialIndex;
    private CameraFadeController fadeController;
    private GameObject playerObject;

    private void Start()
    {
        fadeController = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<CameraFadeController>();
        playerObject = GameObject.FindGameObjectWithTag("Player");

        // Go through all of the sockets in the current tutorial and make them drop their items when a tutorial section is changed.
        // Otherwise the interactables aren't parented correctly and they remain in the scene.
        fadeController.onFadeOutComplete.AddListener(DropInteractablesInScene);

        fadeController.onFadeOutComplete.AddListener(ProgressTutorial);
        fadeController.onFadeOutComplete.AddListener(fadeController.BeginFadeIn);

        if (Debug.isDebugBuild)
        {
            currentTutorialIndex = startFromTutorialIndex;
        }
        else
        {
            currentTutorialIndex = 0;
        }

        if (currentTutorialIndex == transform.childCount - 1)
        {
            RemoveFadeListeners();
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        transform.GetChild(currentTutorialIndex).gameObject.SetActive(true);
    }

    public void FadeToNextTutorial()
    {
        fadeController.BeginFadeOut();
    }

    private void RemoveFadeListeners()
    {
        fadeController.onFadeOutComplete.RemoveListener(DropInteractablesInScene);
        fadeController.onFadeOutComplete.RemoveListener(fadeController.BeginFadeIn);
        fadeController.onFadeOutComplete.RemoveListener(ProgressTutorial);
    }

    private void ProgressTutorial()
    {
        // Debug.Log("Progressing tutorial");
        transform.GetChild(currentTutorialIndex).gameObject.SetActive(false);
        if (currentTutorialIndex < transform.childCount - 1)
        {
            transform.GetChild(currentTutorialIndex + 1).gameObject.SetActive(true);
        }
        else
        {
            // If the player has completed the tutorial then remove all listeners for the fade events.
            RemoveFadeListeners();
        }

        playerObject.transform.position = Vector3.zero;

        currentTutorialIndex++;

    }

    private void DropInteractablesInScene()
    {
        // Drop all of the interactable items in the scene including sockets and player's hands. Otherwise bugs may occur.
        foreach (XRBaseInteractable interactable in FindObjectsOfType<XRBaseInteractable>())
        {
            if (interactable.interactorsSelecting.Count > 0)
            {
                interactable.interactionManager.SelectExit(interactable.interactorsSelecting[0], interactable);
            }
        }
    }

    private void OnDestroy()
    {
        RemoveFadeListeners();
    }

}
