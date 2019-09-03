using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts : MonoBehaviour {

    #region fields
    private static Scripts instance;
    public static Scripts Instance {
        get {
            if (instance == null) {
                GameObject g = GameObject.FindGameObjectWithTag("Scripts");

                if (g == null) {
                    throw new System.Exception("Scripts object not present in the scene");

                }

                instance = g.GetComponent<Scripts>();

                if (instance == null) {
                    throw new System.Exception("Scripts object not present in the scene");

                }
            }

            return instance;
        }
    }

    #endregion

    public static T GetScriptComponent<T>() {
        return Instance.GetComponent<T>();
    }


}
