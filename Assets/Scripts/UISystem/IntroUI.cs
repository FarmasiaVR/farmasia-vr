using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour {

    #region fields
    [SerializeField]
    private InputField nameText;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Text infoLabel, notification;
    #endregion

    private void Start() {
        if (Player.Initialized) {
            Destroy(gameObject);
        }

        button.onClick.AddListener(StartGame);
    }

    private async void StartGame() {
        button.interactable = false;
        nameText.interactable = false;

        Player.Initialize(nameText.text);

        infoLabel.gameObject.SetActive(false);
        notification.gameObject.SetActive(true);

        await System.Threading.Tasks.Task.Delay(5000);

        Destroy(gameObject);
    }
}