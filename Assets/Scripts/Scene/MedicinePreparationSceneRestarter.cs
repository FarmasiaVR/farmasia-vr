using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MedicinePreparationSceneRestarter : MonoBehaviour {

    #region fields
    public byte[] ScoreState { get; set; }
    #endregion

    private void Awake() {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void SceneLoaded(Scene s, LoadSceneMode m) {
        SceneManager.sceneLoaded -= SceneLoaded;
        StartCoroutine(PlayScene());
    }

    private IEnumerator PlayScene() {

        Player.Camera.enabled = false;

        yield return null;
        yield return null;
        yield return null;

        MedicinePreparationScene m = (MedicinePreparationScene)G.Instance.Scene;
        m.PlayFirstRoom(1);
        while (m.IsAutoPlaying) {
            yield return null;
        }

        Player.Camera.enabled = true;
        if (ScoreState != null) {
            G.Instance.Progress.SetProgress(ScoreState);
        }
        Destroy(gameObject);
    }
}