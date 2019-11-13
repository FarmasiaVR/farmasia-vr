using System;
using System.Text;
using UnityEngine;

public static class Logger {

    public static bool LogInGame { get; set; } = true;

    public static void Print(object message) {
        string msg = ObjectToString(message);
        
        Debug.Log(msg);

        if (LogInGame) {
            GUIConsole.Log(msg);
        }
    }

    public static void PrintObjects(string delimeter, params object[] objects) {
        string msg = ObjectsToString(delimeter, objects);

        Debug.Log(msg);

        if (LogInGame) {
            GUIConsole.Log(msg);
        }
    }

    public static void PrintFormat(string format, params object[] args) {

        Debug.LogFormat(format, args);

        if (LogInGame) {
            // no format support yet
            GUIConsole.Log(format);
        }
    }

    public static void Warning(object message) {
        string msg = ObjectToString(message);

        Debug.LogWarning(msg);

        if (LogInGame) {
            GUIConsole.LogWarning(msg);
        }
    }

    public static void WarningObjects(string delimeter, params object[] objects) {
        string msg = ObjectsToString(delimeter, objects);

        Debug.LogWarning(msg);

        if (LogInGame) {
            GUIConsole.LogWarning(msg);
        }
    }

    public static void WarningFormat(string format, params object[] args) {
        Debug.LogWarningFormat(format, args);

        if (LogInGame) {
            GUIConsole.LogWarning(format);
        }
    }

    public static void Error(object message) {
        string msg = ObjectToString(message);

        Debug.LogError(msg);

        if (LogInGame) {
            GUIConsole.LogError(msg);
        }
    }

    public static void ErrorObjects(string delimeter, params object[] objects) {
        string msg = ObjectsToString(delimeter, objects);

        Debug.LogError(msg);

        if (LogInGame) {
            GUIConsole.LogError(msg);
        }
    }

    public static void ErrorFormat(string format, params object[] args) {
        Debug.LogErrorFormat(format, args);

        if (LogInGame) {
            GUIConsole.LogError(format);
        }
    }

    /// <summary>
    /// Lists the following variables as prints
    /// </summary>
    /// <param name="vars">Array with even amount of objects. Odd number is for variable name, even values are for the actual variable values</param>
    public static void PrintVariables(params object[] vars) {
        if (vars.Length % 2 != 0) {
            throw new Exception("Var count is not divisible by 2");
        }

        for (int i = 0; i < vars.Length; i += 2) {
            string msg = VarString(vars[i], vars[i + 1]);
            Debug.Log(msg);
            if (LogInGame) {
                GUIConsole.Log(msg);
            }
        }
    }

    private static string ObjectToString(object o) {
        return o?.ToString() ?? "null";
    }

    private static String ObjectsToString(string delimiter, params object[] objects) {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < objects.Length - 1; i++) {
            sb.Append(ObjectToString(objects[i])).Append(delimiter);
        }
        sb.Append(ObjectToString(objects[objects.Length - 1]));

        return sb.ToString();
    }

    private static string VarString(object o) {
        return VarString(nameof(o), o);
    }

    private static string VarString(object n, object o) {
        return n + ": " + (o?.ToString() ?? "null");
    }
}
