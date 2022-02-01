using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NullCheck {

    /// <summary>
    /// Throws error if parameters are null
    /// </summary>
    /// <param name="objs"></param>
    public static void Check(params object[] objs) {
        for (int i = 0; i < objs.Length; i++) {
            if (objs[i] == null) {
                throw new System.NullReferenceException("Null check object was null. Object index " + i.ToString() + " was null");
            }
        }
    }

    /// <summary>
    /// Returns false is parameters are null
    /// </summary>
    /// <param name="objs"></param>
    /// <returns></returns>
    public static bool CheckSafe(params object[] objs) {
        foreach (object o in objs) {
            if (o == null) {
                return true;
            }
        }
        return false;
    }
}