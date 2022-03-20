using System.Collections.Generic;

public class TaskConfig {

    /// 
    /// Define description, hint, success-text and points, in this order, here.
    /// Note that task classes may override these values (for example, cabinet tasks may show in the hint what items are missing).
    /// As this is a dictionary, order of the tasks is not significant, but they are supposed to be approximately in the correct order for clarity
    /// 
    private static Dictionary<TaskType, Info> dict = new Dictionary<TaskType, Info>{

        { TaskType.SelectTools, info(
            "Valitse sopivat työvälineet.",
            "Huoneessa on lääkkeen valmistukseen tarvittavia työvälineitä. Valitse oikea määrä ruiskuja, neuloja ja luerlockeja.",
            "Työväline valittu.",
            0
        )},

        { TaskType.SelectMedicine, info(
            "Valitse sopiva lääkepullo.",
            "Jääkaapissa on erikokoisia lääkepulloja. Valitse näistä oikeankokoinen.",
            "Lääkepullo valittu.",
            0
        )},

        { TaskType.CorrectItemsInThroughput, info(
            "Laita tarvittavat työvälineet läpiantokaappiin ja siirry työhuoneeseen.",
            "Tarkista välineitä läpiantokaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla. Huoneesta siirrytään pois tarttumalla oveen. Puuttuvat välineet: ",
            "Oikea määrä työvälineitä läpiantokaapissa.",
            2
        )},

        { TaskType.CorrectLayoutInThroughput, info(
            "",
            "",
            "",
            0
        )},

        { TaskType.CorrectItemsInLaminarCabinet, info(
            "Siirrä valitsemasi työvälineet laminaarikaappiin ja paina kaapin tarkistusnappia.",
            "Tarkista välineitä kaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla. Tarkista valintasi painamalla laminaarikaapin tarkistusnappia. ",
            "Oikea määrä työvälineitä laminaarikaapissa.",
            2
        )},

        { TaskType.CorrectLayoutInLaminarCabinet, info(
            "",
            "",
            "",
            0
        )},

        { TaskType.DisinfectBottles, info(
            "",
            "",
            "",
            1
        )},

        { TaskType.SyringeAttach, info(
            "Yhdistä Luerlock-to-luerlock-välikappaleeseen tyhjä ruisku.",
            "Kiinnitä Luerlock-to-luerlock-välikappaleeseen 1ml ruisku.",
            "",
            3
        )},

        { TaskType.LuerlockAttach, info(
            "Kiinnitä lääkkeellinen ruisku luerlock-to-luerlock-välikappaleeseen.",
            "Kiinnitä luerlock-to-luerlock-välikappale oikein 20ml ruiskuun.",
            "Luerlockin kiinnittäminen onnistui.",
            1
        )},

        { TaskType.CorrectAmountOfMedicineSelected, info(
            "Vedä ruiskuun lääkettä.",
            "Vedä ruiskuun oikea määrä (0,15ml) lääkettä.",
            "Ruiskuun otettiin oikea määrä lääkettä.",
            6
        )},

        { TaskType.MedicineToSyringe, info(
            "Valmistele välineet ja ota ruiskulla ja neulalla lääkettä lääkeainepullosta.",
            "Valitse oikeankokoinen ruisku (20ml), jolla otat lääkettä lääkeainepullosta. Varmista, että ruiskuun on kiinnitetty neula.",
            "Lääkkeen ottaminen onnistui.",
            2
        )},

        { TaskType.ItemsToSterileBag, info(
            "Viimeistele ruiskujen kanssa työskentely.",
            "Laita täyttämäsi ruiskut steriiliin pussiin.",
            "Ruiskut laitettiin steriiliin pussiin.",
            2
        )},

        { TaskType.ScenarioOneCleanUp, info(
            "Siivoa lopuksi työtila.",
            "Vie pelin aikana lattialle pudonneet esineet roskakoriin.",
            "",
            1
        )},

        { TaskType.Finish, info(
            "",
            "",
            "",
            0
        )},

        // Membrane filtration scene starts here

        { TaskType.SelectToolsMembrane, info(
            "Valitse sopivat ty�v�lineet.",
            "Huoneessa on l��kkeen valmistukseen tarvittavia ty�v�lineit�. Valitse oikea m��r� ruiskuja, neuloja ja luerlockeja.",
            "Ty�v�line valittu.",
            0
        )},

        { TaskType.CorrectItemsInThroughputMembrane, info(
            "Laita tarvittavat työvälineet läpiantokaappiin ja siirry työhuoneeseen.",
            "Huoneessa on tarvittavat työvälineet pullot ja pipetti. \n" +
                "Mediumit = Soijakaseiini-pullo ja Tioglygolaattipullo. \n\n" +
                "Sormenpäämaljat ja toinen laskeumamalja ovat soijakaseiinimaljoja. \n" +
                "Toinen laskeumamalja on sabourad-dekstroosimalja.",
            "Oikeat työvälineet läpiantokaapissa.",
            2
        )},

        { TaskType.CorrectItemsInLaminarCabinetMembrane, info(
            "Siirrä valitsemasi työvälineet laminaarikaappiin",
            "Varmista välineitä kaappiin viedessäsi, että välineet ovat puhtaita ja että kaapissa on kaikki oikeat välineet.",
            "Oikea määrä työvälineitä laminaarikaapissa.",
            2
        )},
        
        { TaskType.WriteTextsToItems, info(
            "Kirjoita tarvittavat tiedot pulloihin ja maljoihin",
            "Kosketa kyn�ll� esinett�, johon haluat kirjoittaa, valitse kirjoitettavat tekstit (max 4) klikkaamalla niit�. Voit perua kirjoituksen painamalla teksti� uudestaan ennen kuin painat vihre�� nappia",
            "Hyvin kirjoitettu.",
            2
        )},

        { TaskType.OpenAgarplates, info(
            "Avaa laskeumamaljat",
            "Avaa yksi soijakaseiinimalja sek� yksi sabouradekstrosimalja",
            "Hienosti avattu!",
            2
        )},

        { TaskType.FillBottles, info(
            "Täytä pullot",
            "Just do it",
            "Hienoa, pullot täytetty",
            4
        )},

        { TaskType.AssemblePump, info(
            "Kokoa pumppu!",
            "Kiinnitä filtteri ja letku pumppuun.",
            "Hienoa, pumppu on koossa",
            2
        )},

        { TaskType.WetFilter, info(
            "Kostuta filtteri :D",
            "Caman kyl s� osaat",
            "Hienosti kostutettu!",
            2
        )}
    };

    public static Info For(TaskType type) {
        return dict[type];
    }

    private static Info info(string desc, string hint, string success = null, int points = 1) => 
        new Info() { Description = desc, Hint = hint, Success = success, Points = points };

    public class Info {
        public string Description;
        public string Hint;
        public string Success;
        public int Points;
    }
}