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
            TaskType.CorrectItemsInThroughputMedicine,
            "Oikeat työvälineet läpiantokaapissa",
            "Laita tarvittavat työvälineet läpiantokaappiin ja siirry puhdastilaan.",
            "Katso pöydällä olevasta listasta tarvittavat työvälineet ja siirrä ne läpiantokaappiin. Muista myös puhdistaa työvälineet 70% etanoliliuoksella.",
            "Oikea määrä työvälineitä läpiantokaapissa!",
            2
        ),

        info(
            TaskType.CorrectItemsInLaminarCabinetMedicine,
            "Oikeat työvälineet laminaarikaapissa",
            "Siirrä valitsemasi työvälineet laminaarikaappiin.",
            "Puhdista valitsemasi työvälineet 70% etanoliliuoksella ja siirrä ne laminaarikaappiin.",
            "Oikea määrä työvälineitä laminaarikaapissa!",
            2
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
            TaskType.MedicineToSyringe,
            "Lääkkeen ottaminen pullosta",
            "Ota ruiskulla ja neulalla lääkettä lääkeainepullosta. Irroita neula kun olet valmis.",
            "Valitse 20ml ruisku ja kiinnitä siihen neula. Ota lääkettä ruiskuun 9ml verran.",
            "Lääkkeen ottaminen onnistui!",
            2
        ),

        info(
            TaskType.LuerlockAttach,
            "Luerlockin kiinnittäminen",
            "Kiinnitä lääkkeellinen ruisku luerlock-to-luerlock välikappaleeseen.",
            "Kiinnitä luerlock-to-luerlock välikappale 20ml ruiskuun.",
            "Luerlockin kiinnittäminen onnistui!",
            1
        ),

        info(
            TaskType.SyringeAttach,
            "Ruiskun kiinnittäminen",
            "Yhdistä luerlock-to-luerlock välikappaleeseen tyhjä ruisku.",
            "Kiinnitä 1ml ruisku luerlock-to-luerlock välikappaleeseen.",
            "Ruisku kiinnitetty luerlockiin!",
            1
        ),

        info(
            TaskType.CorrectAmountOfMedicineTransferred,
            "Lääkkeen mittaaminen",
            "Vedä 0,15ml verran lääkettä pieneen ruiskuun.",
            "Vedä 0,15ml verran lääkettä pieneen ruiskuun. Älä työnnä lääkettä isosta ruiskusta.",
            "Ruiskuun otettiin lääkettä!",
            1
        ),

        info(
            TaskType.AllSyringesDone,
            "Lääkkeen mittaaminen",
            "Toista samat vaiheet muille ruiskuille.",
            "Vedä jokaiseen ruiskuun oikea määrä lääkettä (0,15ml). Älä työnnä lääkettä isosta ruiskusta.",
            "Kaikissa ruiskuissa tarpeeksi lääkettä!",
            5
        ),

        info(
            TaskType.ItemsToSterileBag,
            "Ruiskujen siirto sterilointipussiin",
            "Sulje ruiskut ja siirrä ne sterilointipussiin.",
            "Ota korkkeja korkkipussista ja sulje ruiskujen päät. Korkkien asettamisen jälkeen siirrä ruiskut sterilointipussiin.",
            "Ruiskut laitettiin sterilointipussiin!",
            2
        ),

        info(
            TaskType.CleanTrashMedicine,
            "Jätteiden lajittelu",
            "Lajittele jätteet.",
            "Laita neula terävien roskakoriin ja muut jätteet tavalliseen roskakoriin.",
            "Jätteet lajiteltu!",
            2
        ),

        info(
            TaskType.CorrectItemsInBasketMedicine,
            "Valmiit esineet koriin",
            "Siirrä valmiit esineet koriin.",
            "Siirrä sterilointipussi ja lääkepullo punaiseen koriin.",
            "Valmiit esineet korissa!",
            2
        ),

        info(
            TaskType.CleanLaminarCabinetMedicine,
            "Laminaarikaapin putsaus",
            "Putsaa laminaarikaapin seinät etanolipullolla.",
            "Ruiskuta 70% etanoliliuosta laminaarikaapin seinille.",
            "Laminaarikaappi puhdistettu!",
            1
        ),

        info(
            TaskType.FinishMedicine,
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
            TaskType.CloseSettlePlates,
            "Laskeumamaljojen sulkeminen",
            "Sulje laskeumamaljat",
            "Sulje laskeumamaljat",
            "Laskeumamaljat suljettu!",
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

        info(
            TaskType.CloseFingertipPlates,
            "Sormenpäämaljojen sulkeminen",
            "Sulje sormenpäämaljat",
            "Sulje sormenpäämaljat",
            "Sormenpäämaljat suljettu!",
            2
        ),

        info(
            TaskType.CloseBottles,
            "Pullojen sulkeminen",
            "Sulje pullot",
            "Suljet isot ja pienet pullot laittamalla pullojen korkit takaisin kiinni.",
            "Pullot suljettu!",
            2
        ),

        info(
            TaskType.CleanTrashMembrane,
            "Jätteiden lajittelu",
            "Lajittele jätteet",
            "Lajittele jätteet",
            "Jätteet lajiteltu!",
            2
        ),

        info(
            TaskType.CorrectItemsInBasketMembrane,
            "Valmiit esineet koriin",
            "Siirrä valmiit esineet koriin",
            "Siirrä valmiit esineet koriin",
            "Valmiit esineet korissa!",
            2
        ),

        info(
            TaskType.EmptyLaminarCabinetMembrane,
            "Laminaarikaapin tyhjennys",
            "Tyhjennä laminaarikaappi",
            "Siirrä ylimääräiset esineet sivupöydälle",
            "Laminaarikaappi tyhjennetty!",
            2
        ),

        info(
            TaskType.CleanLaminarCabinetMembrane,
            "Laminaarikaapin putsaus",
            "Putsaa laminaarikaapin seinät",
            "Ruiskuta etanoliliuosta jokaisen laminaarikaapin seinälle.",
            "Laminaarikaappi puhdistettu!",
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

        // Changing room scene starts here

        info(
            TaskType.WearShoeCoversAndLabCoat,
            "Kengänsuojien ja laboratoriotakin pukeminen",
            "Vedä kengänsuojat ja laboratoriotakki päällesi",
            "Vedä kengänsuojat ja laboratoriotakki itseesi pukeaksesi ne päällesi.",
            "Kengänsuojat ja laboratoriotakki puettu!",
            2
        ),

        info(
            TaskType.WashGlasses,
            "Silmälasien puhdistus",
            "Puhdista silmälasit",
            "Vedä silmalaist juoksevan veden alla puhdistaaksesi ne.",
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
            "Käsienpesu pukuhuoneessa",
            "Suorita käsienpesu oikeassa järjestyksessä",
            "Käytä saippuaa, huuhtele ja lopuksi käytä käsidesiä",
            "Kädet ovat puhtaat.",
            2
        ),

        info(
            TaskType.WearHeadCoverAndFaceMask,
            "Suojapäähineen ja kasvomaskin pukeminen",
            "Vedä suojapäähine ja kasvomaski päällesi",
            "Vedä suojapäähine ja kasvomaski itseesi pukeaksesi ne päällesi.",
            "Suojapäähine ja kasvomaski puettu!",
            2
        ),

        info(
            TaskType.WashHandsInPreperationRoom,
            "Käsienpesu valmistelutilassa",
            "Suorita käsienpesu oikeassa järjestyksessä",
            "Käytä saippuaa, huuhtele ja lopuksi käytä käsidesiä",
            "Kädet ovat puhtaat.",
            2
        ),

        info(
            TaskType.WearSleeveCoversAndProtectiveGloves,
            "Hihasuojien ja suojakäsineiden pukeminen",
            "Vedä hihasuojat ja suojakäsineet päällesi",
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
