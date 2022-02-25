using System.Collections.Generic;
using UnityEngine;

public static class WritingSpecifications {

    /// <summary>
    /// Creates a list that specifies which items must have what writing, before 'pipetting' starts.
    /// 
    /// </summary>
    /// <returns>A new list which contains the specifications</returns>
    public static List<WritingSpec> GetInitialRequiredWritings() => new List<WritingSpec>() {

        new WritingSpec(ObjectType.Bottle,
            WritingType.Date,
            WritingType.Name,
            WritingType.Tioglykolate
            ),
        new WritingSpec(ObjectType.Bottle,
            WritingType.Date,
            WritingType.Name,
            WritingType.Tioglykolate
            ),
        new WritingSpec(ObjectType.Bottle,
            WritingType.Date,
            WritingType.Name,
            WritingType.SoyCaseine
            ),
        new WritingSpec(ObjectType.Bottle,
            WritingType.Date,
            WritingType.Name,
            WritingType.SoyCaseine
            ),

        // Laskeumamaljat
        new WritingSpec(ObjectType.SoycaseinePlate,
            WritingType.Name,
            WritingType.Date,
            WritingType.Time
            ),
        new WritingSpec(ObjectType.SabouradDextrosiPlate,
            WritingType.Name,
            WritingType.Date,
            WritingType.Time
            ),

        // Sormenpäämaljat (must be before soijakaseiini laskeumamalja, as it could match the same items)
        // Will be iterated backwards in WriteTextToItems -> last
        new WritingSpec(ObjectType.SoycaseinePlate,
            WritingType.Name,
            WritingType.Time,
            WritingType.Date,
            WritingType.RightHand
            ),
        new WritingSpec(ObjectType.SoycaseinePlate,
            WritingType.Name,
            WritingType.Time,
            WritingType.Date,
            WritingType.LeftHand
            ),

    };

    public static List<WritingSpec> GetFinalRequiredWritings() => new List<WritingSpec>();
}

public class WritingSpec {
    public ObjectType objectType;
    public WritingType[] requiredWritings;
    public List<WritingType> missingWritings;

    /// <summary>
    /// Constructs a writing specification from objectType and an arbitrary number of required writings
    /// </summary>
    /// <param name="objectType">The ObjectType of the item this specification applies to</param>
    /// <param name="requiredWritings">The required lines of writing</param>
    public WritingSpec(ObjectType objectType, params WritingType[] requiredWritings) {
        this.objectType = objectType;
        this.requiredWritings = requiredWritings;
        missingWritings = new List<WritingType>(requiredWritings);
    }

    /// <summary>
    /// Checks if the given gameObject has matching writing
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns>Whether the gameObject satisfies this writing specification</returns>
    public bool Matches(GameObject gameObject) {
        GeneralItem item = gameObject.GetComponent<GeneralItem>();
        Writable writable = gameObject.GetComponent<Writable>();
        if (item == null || writable == null) return false;

        foreach (var requiredWriting in requiredWritings) {
            if (!writable.WrittenLines.ContainsKey(requiredWriting)) return false;
        }
        return true;
    }
}
