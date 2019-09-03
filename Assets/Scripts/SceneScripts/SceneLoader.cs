using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    public string sceneLocation = "_Scenes/MembraneFiltering";


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(AsyncSceneLoader());
        }
    }

    IEnumerator AsyncSceneLoader()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneLocation);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
    }
}
