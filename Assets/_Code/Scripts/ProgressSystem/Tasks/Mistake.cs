using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mistake
{
    public string mistakeText;
    public int pointsDeducted;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mistakeText">What is the mistake that was made. This text is shown to the player to provide feedback.</param>
    /// <param name="pointsDeducted">How many points should be deducted from the player for making this mistake</param>
    public Mistake(string mistakeText, int pointsDeducted)
    {
        this.mistakeText = mistakeText;
        this.pointsDeducted = pointsDeducted;
    }
}
