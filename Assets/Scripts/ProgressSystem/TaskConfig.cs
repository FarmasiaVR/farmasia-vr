using System.Collections.Generic;
using System.Linq;

public static class TaskConfig {

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

    private static Dictionary<TaskType, Info> dict = new List<KeyValuePair<TaskType, Info>>{

        info(
            TaskType.SelectTools,
            "Työvälineiden valinta",
            "Valitse sopivat työvälineet.",
            "Valitse pöydällä olevan listan perusteella oikeat välineet sekä oikea määrä välineitä.",
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
            "Tarkista välineitä kaappiin viedessäsi, että olet valinnut oikean määrän välineitä ensimmäisellä hakukerralla. Tarkista valintasi painamalla tarkistusnappia laminaarikaapin yläosassa.",
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
            "Valitse oikeankokoinen ruisku (20ml), jolla otat lääkettä lääkeainepullosta. Varmista, että ruiskuun on kiinnitetty neula. Puhdista lääkepullon korkki desinfiointiliinalla.",
            "Lääkkeen ottaminen onnistui.",
            2
        ),

        info(
            TaskType.ItemsToSterileBag,
            "Ruiskujen siirto steriilipussiin",
            "Viimeistele ruiskujen kanssa työskentely.",
            "Sulje ruiskut korkeilla. Laita täyttämäsi ruiskut steriiliin pussiin.",
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
            "Valitse pöydän välinelistassa olevat ty�v�lineet.",
            "Huoneessa on tarvittavat työvälineet. \n" +
            "Ravintoalustat = Soijakaseiini-pullo ja Tioglygolaattipullo. Laskeumamaljat = soijakaseiinimalja ja sabourad-dekstrosimalja. Sormenpäämaljat = soijakaseiinimaljat 2 kpl. Kostutusliuos = peptonivesi",
            "Työväline valittu.",
            0
        ),

        info(
            TaskType.CorrectItemsInThroughputMembrane,
            "Oikeat välineet läpiantokaapissa",
            "Laita tarvittavat työvälineet läpiantokaappiin ja siirry työhuoneeseen.",
            "Puhdista työvälineet 70% etanoliliuoksella. Huoneessa on tarvittavat työvälineet. Laskeumamaljat = soijakaseiinimalja ja sabourad-dekstrosimalja. Sormenpäämaljat = soijakaseiinimaljat 2 kpl. Kostutusliuos = peptonivesi.",
            "Oikeat työvälineet läpiantokaapissa.",
            2
        ),

        info(
            TaskType.CorrectItemsInLaminarCabinetMembrane,
            "Oikeat välineet laminaarikaapissa",
            "Siirrä valitsemasi työvälineet laminaarikaappiin",
            "Varmista välineitä kaappiin viedessäsi, että välineet ovat puhtaita ja että kaapissa on kaikki oikeat välineet. Puhdista työvälineet 70% etanoliliuoksella.",
            "Oikea määrä työvälineitä laminaarikaapissa.",
            2
        ),

        info(
            TaskType.WriteTextsToItems,
            "Tietojen kirjoitus esineisiin",
            "Kirjoita tarvittavat tiedot pulloihin ja maljoihin",
            "Kosketa kyn�ll� esinett�, johon haluat kirjoittaa, valitse kirjoitettavat tekstit (max 4) klikkaamalla niit�. Voit perua kirjoituksen painamalla teksti� uudestaan ennen kuin painat vihre�� nappia. Muista katsoa oikea kellonaika kellosta.",
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
            "Täytä pullot elatusaineilla",
            "Lisää pieniin pulloihin 80 ml elatusaineita, 2 pulloa kutakin",
            "Hienoa, pullot täytetty",
            4
        ),

        info(
            TaskType.AssemblePump,
            "Pumpun kokoaminen",
            "Kokoa pumppu ja kiinnitä jäteletku",
            "Kiinnitä suodatin ja jäteletku pumppuun.",
            "Hienoa, pumppu on koossa",
            3
        ),

        info(
            TaskType.WetFilter,
            "Suodattimen kostutus",
            "Kostuta kalvosuodatin",
            "Kostuta suodatin lisäämällä 1ml peptonivettä Finnpipettilla",
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
            TaskType.MedicineToFilter,
            "Lääkkeen lisääminen",
            "Lisää lääke suodattimeen",
            "Irrota ruiskun korkki ja lisää ruiskun neste suodattimeen",
            "Lääke lisätty",
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
            "Pura pumppu ja leikkaa suodatin",
            "Irrota kalvosuodatin pumpusta ja leikkaa varovasti skalpellilla. Muista avata skalpellin pakkaus oikeasta päästä!",
            "Hienoa, suodatin leikattu",
            2
        ),

        info(
            TaskType.FilterHalvesToBottles,
            "Suodattimen puolikkaiden siirtäminen pulloihin",
            "Siirrä puolikkaat pulloihin pinseteillä",
            "Siirrä puolikkaat eri liuoksiin. Siirrä yksi puolikas soija-kaseiiniliuokseen ja toinen tioglykolaattiin. Muista avata pinsettien pakkaus oikeasta päästä!",
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
            TaskType.WriteSecondTime,
            "Lopetusaikojen kirjoitus",
            "Ota ajat ylös",
            "Kirjoita lopetusaika laskeumamaljoihin. Katso oikea aika kellosta.",
            "Hyvin kirjoitettu",
            2
        ),

        info(
            TaskType.Fingerprints,
            "Sormenpäämaljat",
            "Valmistele sormenpäämaljat",
            "Avaa sormenpäämaljat ja koske agaria kaksi kertaa (kerran peukalolla ja kerran muilla sormilla). Toista toisella kädellä.",
            "Sormenpäämaljat valmistettu!",
            2
        ),
        
        // TODO: Siivousinfo

        info(
            TaskType.FinishMembrane,
            "Lopetus",
            "",
            "",
            "",
            0
        ),

        // Changing room scene starts here

        info(
            TaskType.WearShoeCoversAndLabCoat,
            "Kengänsuojien ja laboratoriotakin pukeminen",
            "Pue kengänsuojat ja laboratoriotakki",
            "Vedä kengänsuojat ja laboratoriotakki itseesi pukeaksesi ne päällesi.",
            "Kengänsuojat ja laboratoriotakki puettu!",
            2
        ),

        info(
            TaskType.WashGlasses,
            "Silmälasien puhdistus",
            "Huuhtele silmälasit vedellä",
            "Huuhtele silmälasit juoksevan veden alla.",
            "Silmälasit puhdistettu!",
            2
        ),

        info(
            TaskType.GoToPreperationRoom,
            "",
            "Siirry valmistelutilaan",
            "",
            "",
            0
        ),

        info(
            TaskType.WashHandsInChangingRoom,
            "Käsienpesu",
            "Suorita käsienpesu oikeassa järjestyksessä",
            "Käytä saippuaa, huuhtele ja lopuksi käytä käsidesiä",
            "Kädet ovat puhtaat.",
            0
        ),

        info(
            TaskType.WearHeadCoverAndFaceMask,
            "Suojapäähineen ja kasvomaskin pukeminen",
            "Pue suojapäähine ja kasvomaski",
            "Vedä suojapäähine ja kasvomaski itseesi pukeaksesi ne päällesi.",
            "Suojapäähine ja kasvomaski puettu!",
            2
        ),

        info(
            TaskType.WashHandsInPreperationRoom,
            "Käsienpesu",
            "Suorita käsienpesu oikeassa järjestyksessä",
            "Käytä saippuaa, huuhtele ja lopuksi käytä käsidesiä",
            "Kädet ovat puhtaat.",
            0
        ),

        info(
            TaskType.WearSleeveCoversAndProtectiveGloves,
            "Hihasuojien ja suojakäsineiden pukeminen",
            "Pue hihasuojat ja suojakäsineet",
            "Vedä hihasuojat ja suojakäsineet itseesi pukeaksesi ne päällesi.",
            "Hihasuojat ja suojakäsineet puettu!",
            2
        ),

        info(
            TaskType.FinishChangingRoom,
            "Lopetus",
            "",
            "",
            "",
            0
        ),

    }.ToDictionary(pair => pair.Key, pair => pair.Value);

    public static Info For(TaskType type) {
        try {
            return dict[type];
        } catch (KeyNotFoundException e) {
            Logger.Error(type);
            throw e;
        }
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
