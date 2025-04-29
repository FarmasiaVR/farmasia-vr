using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YesNoController : MonoBehaviour
{
    public GameObject userInput;
    private GameObject parent;
    private GameObject confirmation;
    private GameObject returnToMenu;
    private bool isActive = true;
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        confirmation = GameObject.Find("/System/Scorescreen/Confirmation");
        returnToMenu = GameObject.Find("/System/Scorescreen/ReturnToMenu");
    }
    public void HandleYesPressed()
    {
        Logger.Print("Yes button was pressed.");
        ToggleVisibility();
    }

    public void HandleNoPressed()
    {
        Logger.Print("No button was pressed.");
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleVisibility() {
        if (isActive) {
            parent.SetActive(false);
            userInput.SetActive(true);
            isActive = false;
        } else {
            confirmation.SetActive(false);
            parent.SetActive(true);
            returnToMenu.SetActive(true);
            userInput.SetActive(false);
            isActive = true;
        }
    
    }
}
