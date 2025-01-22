using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoReset : MonoBehaviour {
    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("DemoScene");
        }
    }
}
