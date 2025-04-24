using UnityEngine;
using UnityEngine.SceneManagement;

public class YesNoController : MonoBehaviour
{
    public void HandleYesPressed()
    {
        Logger.Print("Yes button was pressed.");
        // Add more logic here if needed
    }

    public void HandleNoPressed()
    {
        Logger.Print("No button was pressed.");
        SceneManager.LoadScene("MainMenu");
    }
}
