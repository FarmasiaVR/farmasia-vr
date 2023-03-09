using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mistake
{
    public string mistakeText;
    public int pointsDeducted;

    public Mistake(string mistakeText, int pointsDeducted)
    {
        this.mistakeText = mistakeText;
        this.pointsDeducted = pointsDeducted;
    }
}
