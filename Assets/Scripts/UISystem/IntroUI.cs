using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour {

    #region fields
    [SerializeField]
    private InputField name, number;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Text infoLabel, notification;
    #endregion

    private void Start() {
        button.onClick.AddListener(StartGame);
    }

    private async void StartGame() {
        button.interactable = false;
        name.interactable = false;
        number.interactable = false;

        Player.Initialize(name.text, number.text);

        infoLabel.gameObject.SetActive(false);
        notification.gameObject.SetActive(true);

        await Task.Delay(5000);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Destroy(gameObject);
    }
}