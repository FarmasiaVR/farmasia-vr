using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NullCheck {

    /// <summary>
    /// Throws error if parameters are null
    /// </summary>
    /// <param name="objs"></param>
    public static void Check(params object[] objs) {
        foreach (object o in objs) {
            if (o == null) {
                throw new System.NullReferenceException("Null check object was null");
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