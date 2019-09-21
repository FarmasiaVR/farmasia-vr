using UnityEngine;
using UnityEngine.UI;

public class UIWriter : MonoBehaviour {

    #region fields
    private GameObject canvas;
    #endregion

    public string ToggleChild(string childName) {
        GameObject child = ReturnChild(childName);
        Text text = child.GetComponent<Text>();
        text.enabled = !text.enabled;
        return "Text is set to " + text.enabled;
    }

    public string WriteToName(string childName) {
        return "null";
    }

    private GameObject ReturnChild(string childName) {
        GameObject child = canvas.gameObject.transform.Find(childName).gameObject;
        return child;
    }

    private void Start() {
        canvas = GameObject.Find("Canvas");
    }
}
