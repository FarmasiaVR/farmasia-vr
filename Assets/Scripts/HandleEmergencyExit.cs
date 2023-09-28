using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleExit : MonoBehaviour
{
    private SceneLoader levelChanger;
    // Start is called before the first frame update
    void Start()
    {
        levelChanger = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<SceneLoader>();
    }


    public void ExitToMenu()
    {
        Debug.Log("Exiting to fire safety menu");
        levelChanger.SwapScene(SceneTypes.EmergencyExitTutorial);
    }
}