using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEmergencyExit : MonoBehaviour
{
    private SceneLoader levelChanger;
    // Start is called before the first frame update
    private void OnValidate()
    {
        if(!levelChanger) {
            levelChanger = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<SceneLoader>();
        }
    }


    public void ExitToMenu()
    {
        Debug.Log("Exiting to emergency exit menu");
        levelChanger.SwapScene(SceneTypes.EmergencyExitTutorial);
    }
}
