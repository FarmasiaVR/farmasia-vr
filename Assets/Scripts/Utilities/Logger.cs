using System;
using System.Text;
using UnityEngine;

public static class Logger {

    /// <summary>
    /// Prints the gives strings
    /// </summary>
    /// <param name="vars"></param>
    public static void Print(params object[] vars) {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < vars.Length - 1; i++) {
            sb.Append(ObjectToString(vars[i]) + " ");
        }
        sb.Append(ObjectToString(vars[vars.Length - 1]));

        Debug.Log(sb.ToString());
    }

    private static string ObjectToString(object o) {
        if (o == null) {
            return "null";
        } else {
            return o.ToString();
        }
    }

    public static void PrintVariablesLine(params object[] vars) {
        StringBuilder sb = new StringBuilder();

        foreach (object o in vars) {
            sb.Append(VarString(o) + ": ");
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
            Debug.Log(VarString(vars[i], vars[i + 1]));
        }
    }

    private static string VarString(object o) {
        if (o == null) {
            return nameof(o) + ": null";
        }

        return nameof(o) + ": " + o.ToString();
    }
    private static string VarString(object n, object o) {
        if (o == null) {
            return n + ": null";
        }

        return n + ": " + o.ToString();
    }
}
