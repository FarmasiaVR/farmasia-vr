using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Description : MonoBehaviour {
    #region Fields
    private List<ITask> activeTasks;
    private int pointer = 0;
    private GameObject textObject;
    private TextMeshPro textField;
    private float transVisible = 1.0f;
    private float transHidden = 0.5f;
    #endregion

    private void Awake() {
        textObject = transform.gameObject;
        textField = textObject.GetComponent<TextMeshPro>();
    }

    public List<ITask> getActiveList() {
        return activeTasks;
    }

    public int getPointer() {
        return pointer;
    }

    public void SetActiveList(List<ITask> tasks) {
        activeTasks = tasks;
    }

    private void SetColor(Color color) {
        textField.color = new Color(color.r, color.g, color.b, transVisible);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>1: if at the top, -1: if near the start, 0: in the middle, -2: if both 0</returns>
    public int CheckPointerEdge() {
        if (activeTasks.Count == 0 || activeTasks.Count == 1) {
            return -2;
        }
        if ((pointer + 1) >= activeTasks.Count) {
            return 1;
        }
        if ((pointer - 1) < 0) {
            return -1;
        }
        return 0;
    }

    /// <summary>
    /// Checks if pointer is over or under activetask range.
    /// </summary>
    /// <param name="i">Amount of steps pointer takes</param>
    /// <returns>Returns: 1 - Over the range, 0 - inside range, -1 - Under range</returns>
    private int CheckPointerPosition(int i) {
        if ((pointer + i) > (activeTasks.Count - 1)) {
            return 1;
        }
        if ((pointer + i) < 0) {
            return -1;
        }
        return 0;
    }

    public void MovePointerAndDescToFirst() {
        pointer = 0;
        SetDescriptionByPointer();
    }

    public bool MoveDescWithPointer(int i) {
        bool returnValue = true;
        int position = CheckPointerPosition(i);
        if (position == 1) {
            pointer = (activeTasks.Count - 1);
            returnValue = false;
        } else if (position == -1) {
            pointer = 0;
            returnValue = false;
        } else {
            pointer += i;
        }

        SetDescriptionByPointer();
        return returnValue;
    }

    private void SetDescriptionByPointer() {
        if (activeTasks.Count == 0) {
            SetDescription("", Color.white);
            return;
        }
        string description = activeTasks[pointer].GetDescription();
        SetDescription(description, Color.white);
    }

    public void SetTransparency(bool visible) {
        if (visible) {
            textField.color = new Color(textField.color.r, textField.color.g, textField.color.b, transVisible);
            return;
        }
        textField.color = new Color(textField.color.r, textField.color.g, textField.color.b, transHidden);
    }

    public void SetDescription(string description, Color color) {
        textField.text = description;
        SetColor(color);
    }

    public void RemoteFinishShownTask() {
        if (activeTasks.Count != 0) {
            activeTasks[pointer].FinishTask();
        }
    }

}
