using System;
using UnityEngine;

public class ChildTools {
    public static void ToggleParentAndChilds(GameObject obj) {
        bool active = !obj.activeSelf;
        obj.SetActive(active);
        RecursiveToggle(obj);
    }

    private static void RecursiveToggle(GameObject gobj) {
        gobj.SetActive(gobj.activeInHierarchy);
        for (int i = 0; i < gobj.transform.childCount; i++) {
            RecursiveToggle(gobj.transform.GetChild(i).gameObject);
        }
    }
}
