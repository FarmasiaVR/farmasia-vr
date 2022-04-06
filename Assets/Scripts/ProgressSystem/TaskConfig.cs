using System.Collections.Generic;
using System.Linq;

public static class TaskConfig {

    /// 
    /// Define task information here.
    /// Note that task classes may override these values (for example, cabinet tasks may show in the hint what items are missing).
    /// As this is a dictionary, order of the tasks is not significant, but they are supposed to be approximately in the correct order for clarity
    /// 
    /// Format:
    /// TaskType,
    /// Name - shown on scoreboard
    /// Description - shown on the info board for current task
    /// Hint - can be shown by pressing the hint button
    /// Success - text in the popup when task completed
    /// Points
    /// 

    private static Dictionary<TaskType, Info> dict = new List<KeyValuePair<TaskType, Info>>{ 

        info(
            TaskType.SelectTools,
            "Työvälineiden valinta",
            "Valitse sopivat työvälineet.",
            "Huoneessa on lääkkeen valmistukseen tarvittavia työvälineitä. Valitse oikea määrä ruiskuja, neuloja ja luerlockeja.",
            "Työväline valittu.",
            0
        ),

        info(
            TaskType.SelectMedicine,
            "Lääkepullon valinta",
            "Valitse sopiva lääkepullo.",
            "Jääkaapissa on erikokoisia lääkepulloja. Valitse näistä oikeankokoinen.",
            "Lääkepullo valittu.",
            0
        ),

        info(
            TaskType.CorrectItemsInThroughput,
            "Oikeat välineet läpiantokaapissa",
            "Laita tarvittavat työvälineet läpiantokaappiin ja siirry työhuoneeseen.",
            "Tarkista välineitä läpiantokaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla. Huoneesta siirrytään pois tarttumalla oveen. Puuttuvat välineet: ",
            "Oikea määrä työvälineitä läpiantokaapissa.",
            2
        ),

        info(
            TaskType.CorrectLayoutInThroughput,
            "Esineiden asettelu läpiantokaapissa",
            "",
            "",
            "",
            0
        ),

        info(
            TaskType.CorrectItemsInLaminarCabinet,
            "Oikeat välineet laminaarikaapissa",
            "Siirrä valitsemasi työvälineet laminaarikaappiin ja paina kaapin tarkistusnappia.",
            "Tarkista välineitä kaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla. Tarkista valintasi painamalla laminaarikaapin tarkistusnappia. ",
            "Oikea määrä työvälineitä laminaarikaapissa.",
            2
        ),

        info(
            TaskType.CorrectLayoutInLaminarCabinet,
            "Esineiden asettelu laminaarikaapissa",
            "",
            "",
            "",
            0
        ),

        info(
            TaskType.DisinfectBottles,
            "Pullon desinfiointi",
            "",
            "",
            "",
            1
        ),

        info(
            TaskType.SyringeAttach,
            "Ruiskun kiinnittäminen",
            "Yhdistä Luerlock-to-luerlock-välikappaleeseen tyhjä ruisku.",
            "Kiinnitä Luerlock-to-luerlock-välikappaleeseen 1ml ruisku.",
            "",
            3
        ),

        info(
             TaskType.LuerlockAttach,
             "Luerlockin kiinnittäminen",
            "Kiinnitä lääkkeellinen ruisku luerlock-to-luerlock-välikappaleeseen.",
            "Kiinnitä luerlock-to-luerlock-välikappale oikein 20ml ruiskuun.",
            "Luerlockin kiinnittäminen onnistui.",
            1
        ),

        info(
            TaskType.CorrectAmountOfMedicineSelected,
            "Lääkkeen mittaaminen",
            "Vedä ruiskuun lääkettä.",
            "Vedä ruiskuun oikea määrä (0,15ml) lääkettä.",
            "Ruiskuun otettiin oikea määrä lääkettä.",
            6
        ),

        info(
            TaskType.MedicineToSyringe,
            "Lääkkeen otto pullosta",
            "Valmistele välineet ja ota ruiskulla ja neulalla lääkettä lääkeainepullosta.",
            "Valitse oikeankokoinen ruisku (20ml), jolla otat lääkettä lääkeainepullosta. Varmista, että ruiskuun on kiinnitetty neula.",
            "Lääkkeen ottaminen onnistui.",
            2
        ),

        info(
            TaskType.ItemsToSterileBag,
            "Ruiskujen siirto steriilipussiin",
            "Viimeistele ruiskujen kanssa työskentely.",
            "Laita täyttämäsi ruiskut steriiliin pussiin.",
            "Ruiskut laitettiin steriiliin pussiin.",
            2
        ),

        info(
            TaskType.ScenarioOneCleanUp,
            "Siivoaminen",
            "Siivoa lopuksi työtila.",
            "Vie pelin aikana lattialle pudonneet esineet roskakoriin.",
            "",
            1
        ),

        info(
            TaskType.Finish,
            "Lopetus",
            "",
            "",
            "",
            0
        ),

        // Membrane filtration scene starts here

        info(
            TaskType.SelectToolsMembrane,
            "Työvälineiden valinta",
            "Valitse sopivat ty�v�lineet.",
            "Huoneessa on l��kkeen valmistukseen tarvittavia ty�v�lineit�. Valitse oikea m��r� ruiskuja, neuloja ja luerlockeja.",
            "Ty�v�line valittu.",
            0
        ),

        info(
            TaskType.CorrectItemsInThroughputMembrane,
            "Oikeat välineet läpiantokaapissa",
            "Laita tarvittavat työvälineet läpiantokaappiin ja siirry työhuoneeseen.",
            "Huoneessa on tarvittavat työvälineet pullot ja pipetti. \n" +
                "Mediumit = Soijakaseiini-pullo ja Tioglygolaattipullo. \n\n" +
                "Sormenpäämaljat ja toinen laskeumamalja ovat soijakaseiinimaljoja. \n" +
                "Toinen laskeumamalja on sabourad-dekstroosimalja.",
            "Oikeat työvälineet läpiantokaapissa.",
            2
        ),

        info(
            TaskType.CorrectItemsInLaminarCabinetMembrane,
            "Oikeat välineet laminaarikaapissa",
            "Siirrä valitsemasi työvälineet laminaarikaappiin",
            "Varmista välineitä kaappiin viedessäsi, että välineet ovat puhtaita ja että kaapissa on kaikki oikeat välineet.",
            "Oikea määrä työvälineitä laminaarikaapissa.",
            2
        ),
        
        info(
            TaskType.WriteTextsToItems,
            "Tietojen kirjoitus esineisiin",
            "Kirjoita tarvittavat tiedot pulloihin ja maljoihin",
            "Kosketa kyn�ll� esinett�, johon haluat kirjoittaa, valitse kirjoitettavat tekstit (max 4) klikkaamalla niit�. Voit perua kirjoituksen painamalla teksti� uudestaan ennen kuin painat vihre�� nappia",
            "Hyvin kirjoitettu.",
            2
        ),

        info(
            TaskType.OpenAgarplates,
            "Laskeumamaljojen avaus",
            "Avaa laskeumamaljat",
            "Avaa yksi soijakaseiinimalja sek� yksi sabouradekstrosimalja",
            "Hienoa, agarmaljat avattu!",
            2
        ),     

        info(
            TaskType.FillBottles,
            "100ml pullojen täyttö",
            "Täytä pullot",
            "Lisää pieniin pulloihin 80 ml elatusaineita, 2 pulloa kutakin",
            "Hienoa, pullot täytetty",
            4
        ),

        info(
            TaskType.OpenCovers,
            "Suojamuovien avaus",
            "Avaa suojamuovit",
            "Avaa suojamuovit oikeasta päästä",
            "Hienoa, suojamuovit avattu!",
            2
        ),

        info(
            TaskType.AssemblePump,
            "Pumpun kokoaminen",
            "Kokoa pumppu",
            "Kiinnitä suodatin ja jäteletku pumppuun.",
            "Hienoa, pumppu on koossa",
            2
        ),

        info(
            TaskType.WetFilter,
            "Suodattimen kostutus",
            "Kostuta suodatin",
            "Kostuta suodatin lisäämällä 1ml peptonivettä",
            "Hienosti kostutettu!",
            2
        ),

        info(
            TaskType.StartPump,
            "Kostutusliuoksen suodatus",
            "Käynnistä pumppu",
            "Käynnistä pumppu painamalla käynnistysnappulaa",
            "Kostutettu!",
            2
        ),

        info(
            TaskType.StartPumpAgain,
            "Lääkevalmisteen suodatus",
            "Käynnistä pumppu",
            "Käynnistä pumppu painamalla käynnistysnappulaa",
            "Suodatettu!",
            2
        ),

        info(
            TaskType.CutFilter,
            "Kalvosuodattimen leikkaus",
            "Leikkaa suodatin",
            "Avaa suodattimen tankki ja leikkaa kalvosuodatin varovasti skalpellilla",
            "Hienoa, suodatin leikattu",
            2
        ),

        info(
            TaskType.FilterHalvesToBottles,
            "Suodattimen puolikkaiden siirtäminen pulloihin",
            "Siirrä puolikkaat pulloihin",
            "Siirrä puolikkaat eri liuoksiin",
            "Puolikkaat pulloissa!",
            2
        ),

        info(
            TaskType.CloseAgarplates,
            "Laskeumamaljojen sulkeminen",
            "Sulje laskeumamaljat",
            "Sulje laskeumamaljat",
            "Maljat suljettu!",
            2
        ),

        info(
            TaskType.Fingerprints,
            "Sormenpäämaljat",
            "Valmistele sormenpäämaljat",
            "Avaa sormenpäämaljat ja koske ainetta",
            "Sormenpäämaljat valmistettu!",
            2
        ),
        
        info(
            TaskType.FinishMembrane,
            "Lopetus",
            "",
            "",
            "",
            0
        ),

    }.ToDictionary(pair => pair.Key, pair => pair.Value);

    public static Info For(TaskType type) {
        return dict[type];
    }

    private static KeyValuePair<TaskType, Info> info(TaskType type, string name, string desc, string hint, string success, int points) => 
        new KeyValuePair<TaskType, Info>(type, new Info() { Name = name, Description = desc, Hint = hint, Success = success, Points = points });

    public class Info {
        public string Name;
        public string Description;
        public string Hint;
        public string Success;
        public int Points;
    }
}