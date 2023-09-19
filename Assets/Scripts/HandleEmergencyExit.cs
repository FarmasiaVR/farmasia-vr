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
            GameObject levelChangerObject = GameObject.FindGameObjectWithTag("LevelChanger");
            if (levelChangerObject)
            {
                levelChanger = levelChangerObject.GetComponent<SceneLoader>();
            }
            else
            {
                Debug.Log("WARNING HandleEmergencyExit did not find sceneloader!");
            }

        }
    }


    public void ExitToMenu()
    {
        Debug.Log("Exiting to emergency exit menu");
        levelChanger.SwapScene(SceneTypes.EmergencyExitTutorial);
    }
}
