using System.Collections.Generic;

public class TaskConfig {

    /// 
    /// Define description, hint, success-text and points, in this order, here.
    /// Note that task classes may override these values.
    /// 
    private static Dictionary<TaskType, Info> dict = new Dictionary<TaskType, Info>{

        { TaskType.CorrectItemsInLaminarCabinetMembrane, i(
            "Siirrä valitsemasi työvälineet laminaarikaappiin",
            "Varmista välineitä kaappiin viedessäsi, että välineet ovat puhtaita ja että kaapissa on kaikki oikeat välineet.",
            "Oikea määrä työvälineitä laminaarikaapissa.",
            1
        )},
        
        { TaskType.WriteTextsToItems, i(
            "Kirjoita tarvittavat tiedot pulloihin ja maljoihin",
            "Kosketa kyn�ll� esinett�, johon haluat kirjoittaa, valitse kirjoitettavat tekstit (max 4) klikkaamalla niit�. Voit perua kirjoituksen painamalla teksti� uudestaan ennen kuin painat vihre�� nappia",
            "Hyvin kirjoitettu.",
            1
        )},

        { TaskType.OpenAgarplates, i(
            "Avaa laskeumamaljat",
            "Avaa yksi soijakaseiinimalja sek� yksi sabouradekstrosimalja",
            "Hienosti avattu!",
            1
        )},

        { TaskType.FillBottles, i(
            "Täytä pullot",
            "Just do it",
            "Hienoa, pullot täytetty",
            1
        )},

        { TaskType.AssemblePump, i(
            "Kokoa pumppu!",
            "Kiinnitä filtteri ja letku pumppuun.",
            "Hienoa, pumppu on koossa",
            1
        )},

        { TaskType.WetFilter, i(
            "Kostuta filtteri :D",
            "Caman kyl s� osaat",
            "Hienosti kostutettu!",
            1
        )}
    };

    public static Info For(TaskType type) {
        return dict[type];
    }

    private static Info i(string desc, string hint, string success = null, int points = 1) => 
        new Info() { Description = desc, Hint = hint, Success = success, Points = points };

    public class Info {
        public string Description;
        public string Hint;
        public string Success;
        public int Points;
    }
}