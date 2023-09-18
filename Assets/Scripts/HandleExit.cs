using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleExit : MonoBehaviour
{
    private SceneLoader levelChanger;
    // Start is called before the first frame update
    private void OnValidate()
    {
        if(!levelChanger) {
            GameObject levelChangerObject = GameObject.FindGameObjectWithTag("LevelChanger");
            if(levelChangerObject)
            {
                levelChanger = levelChangerObject.GetComponent<SceneLoader>();
            }
            else
            {
                Debug.Log("WARNING HandleExit did not find sceneloader!");
            }
            
        }
    }


    public void ExitToMenu()
    {
        Debug.Log("Exiting to menu");
        levelChanger.SwapScene(SceneTypes.MainMenu);
    }
}
