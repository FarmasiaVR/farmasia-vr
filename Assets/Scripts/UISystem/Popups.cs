using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Popups {

    public static GameObject Prefab;
    private static GameObject currentPopup;
    public static GameObject Player;

    private static List<(int, string, MsgType)> popups = new List<(int, string, MsgType)>();

    private static bool showing;


    /// <summary>
    /// Initiates UIComponent into players hand.
    /// </summary>
    /// <returns>Reference to the instantiated GameObject</returns>
    private static GameObject InitUIComponent(GameObject gobj) {
        GameObject uiComponent = GameObject.Instantiate(gobj);
        uiComponent.transform.position = Player.transform.position;
        return uiComponent;
    }

    private static void SetCurrentPopup(GameObject newPopup) {
        if (currentPopup != null) {
            GameObject.Destroy(currentPopup);
        }
        currentPopup = newPopup;
    }

    public static void CreatePopup(this MonoBehaviour ui, int point, string message, MsgType type) {
        popups.Add((point, message, type));
        if (!showing) {
            showing = true;
            CoroutineUtils.StartThrowingCoroutine(ui, ShowPopups());
        }
    }

    public static IEnumerator ShowPopups() {
        
        while (popups.Count > 0) {
            var (point, message, type) = popups[0];

            switch (type) {
            case MsgType.Mistake:
                G.Instance.Audio.Play(AudioClipType.MistakeMessage);
                break;
            }

            Logger.Print(string.Format("{0} {1} {2}", point.ToString(), type.ToString(), message));

            GameObject popupMessage = InitUIComponent(Prefab);
            PointPopup popup = popupMessage.GetComponent<PointPopup>();
            popup.SetObjectPath(Player, Player);
            popup.SetCamera(Player);

            if (point == int.MinValue || point == 0) {
                popup.SetPopup(message, type);
            } else {
                popup.SetPopup(point, message, type);
            }
            SetCurrentPopup(popupMessage);

            popups.RemoveAt(0);

            yield return new WaitForSecondsRealtime(3f);
        }
        
        showing = false;

        yield break;
    }
}