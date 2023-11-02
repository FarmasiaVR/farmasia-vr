using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

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

        // Medicine preparation scene starts here

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
            TaskType.DisinfectBottleCap,
            "Lääkepullon desinfiointi",
            "Desinfioi lääkepullon korkki steriilillä pyyhkeellä.",
            "Desinfioi lääkepullon korkki steriilillä pyyhkeellä.",
            "Lääkepullo desinfioitu!",
            1
        ),

        info(
            TaskType.MedicineToSyringe,
            "Lääkkeen ottaminen pullosta",
            "Ota ruiskulla ja neulalla lääkettä lääkeainepullosta. Irroita neula kun olet valmis.",
            "Valitse 20ml ruisku ja kiinnitä siihen neula. Ota lääkettä ruiskuun 0,9ml verran.",
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
            "Jokaisessa ruiskussa lääkettä",
            "Toista samat vaiheet muille ruiskuille.",
            "Vedä jokaiseen ruiskuun oikea määrä lääkettä (0,15ml). Älä työnnä lääkettä isosta ruiskusta.",
            "Kaikissa ruiskuissa tarpeeksi lääkettä!",
            5
        ),

        info(
            TaskType.ItemsToSterileBag,
            "Ruiskujen siirto sterilointipussiin",
            "Sulje ruiskut korkilla ja siirrä ne sterilointipussiin.",
            "Ota korkkeja korkkipussista ja sulje ruiskujen päät. Korkkien asettamisen jälkeen siirrä ruiskut sterilointipussiin.",
            "Ruiskut laitettiin sterilointipussiin!",
            2
        ),

        info(
            TaskType.CleanTrashMedicine,
            "Jätteiden lajittelu",
            "Lajittele jätteet.",
            "Laita neula terävien roskiin ja muut jätteet tavalliseen roskakoriin.",
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

        info( // redundant?
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
            "Lista tarvittavista työvälineistä löytyy pöydältä. Puhdista työvälineet 70% etanoliliuoksella (suihkepullossa) ennen läpiantokaappiin viemistä.",
            "Oikeat työvälineet läpiantokaapissa.",
            2
        ),

        info(
            TaskType.CorrectItemsInLaminarCabinetMembrane,
            "Oikeat välineet laminaarikaapissa",
            "Siirrä työvälineet laminaarikaappiin",
            "Puhdista työvälineet ennen laminaarikaappiin vientiä 70% etanoliliuoksella (suihkepullossa). Tarvitset läpiantokaapissa olevat työvälineet ja kynän.",
            "Oikea määrä työvälineitä laminaarikaapissa.",
            2
        ),

        info(
            TaskType.WriteTextsToItems,
            "Tietojen kirjoitus esineisiin",
            "Kirjoita tarvittavat tiedot pulloihin ja maljoihin",
            "Kirjoita 100ml pulloihin (2kpl soijakaseiini, 2kpl tioglykolaatti) nimesi, päivämäärä, ja elatusaine. \n Kirjoita laskeumamaljoihin (1kpl soijakaseiini, 1kpl sabouraud-dekstroosi) nimesi, päivämäärä ja kellonaika. \n Kirjoita sormenpäämaljoihin (2kpl soijakaseiini) nimesi, päivämäärä ja oikea/vasen käsi.",
            "Hyvin kirjoitettu.",
            2
        ),

        info(
            TaskType.OpenAgarplates,
            "Laskeumamaljojen avaus",
            "Avaa laskeumamaljat",
            "Avaa sabouraud-dekstroosimalja sekä se soijakaseiinimalja, johon kirjoitit kellonajan.",
            "Hienoa, agarmaljat avattu!",
            2
        ), 

        info(
            TaskType.FillBottles,
            "100ml pullojen täyttö",
            "Lisää 100ml pulloihin elatusaineita",
            "Lisää 80ml elatusainetta kuhunkin pulloon. Tarvitset 2 pulloa soijakaseiinia ja 2 pulloa tioglykolaattia. Muista katsoa, että laitat oikeaa ainetta oikeaan pulloon. \n Mittaa elatusaineet pipettorilla. Avaa mittapipetti suojapakkauksestaan ja kiinnitä pipettoriin. Muista vaihtaa mittapipetti vaihtaessasi liuosta.",
            "Hienoa, pullot täytetty",
            4
        ),

        info(
            TaskType.AssemblePump,
            "Pumpun kokoaminen",
            "Kokoa pumppu ja kiinnitä jäteletku",
            "Avaa suodatin suojapakkauksestaan ja kiinnitä se pumppuun. Kiinnitä jäteletku lääkejäteastiaan.",
            "Hienoa, pumppu on koossa",
            3
        ),

        info(
            TaskType.WetFilter,
            "Suodattimen kostutus",
            "Kostuta kalvosuodatin",
            "Avaa suodattimen kansi ja lisää suodattimeen 1ml peptonivettä finnpipetillä.",
            "Hienosti kostutettu!",
            2
        ),

        info(
            TaskType.StartPump,
            "Kostutusliuoksen suodatus",
            "Käynnistä pumppu",
            "Käynnistä pumppu painamalla käynnistysnappulaa. Muista sulkea kansi.",
            "Kostutettu!",
            2
        ),

        info(
            TaskType.MedicineToFilter,
            "Lääkkeen lisääminen",
            "Lisää lääke suodattimeen",
            "Ota lääkeruisku sterilointipussista ja lisää lääke suodattimeen.",
            "Lääke lisätty",
            2
        ),

        info(
            TaskType.StartPumpAgain,
            "Lääkevalmisteen suodatus",
            "Käynnistä pumppu",
            "Käynnistä pumppu painamalla käynnistysnappulaa. Muista sulkea kansi.",
            "Suodatettu!",
            2
        ),

        info(
            TaskType.CutFilter,
            "Kalvosuodattimen leikkaus",
            "Pura pumppu ja leikkaa suodatin",
            "Irrota suodatin pumpusta ja leikkaa sen pohjalla oleva kalvosuodatin varovasti skalpellilla kahteen osaan. Muista avata skalpellin pakkaus oikeasta päästä.",
            "Hienoa, suodatin leikattu",
            2
        ),

        info(
            TaskType.FilterHalvesToBottles,
            "Suodattimen puolikkaiden siirtäminen pulloihin",
            "Siirrä puolikkaat pulloihin pinseteillä",
            "Siirrä puolikkaat elatusliuoksiin pieniin pulloihin, toinen soijakaseiiniin ja toinen tioglykolaattiin. Muista avata pinsettien pakkaus oikeasta päästä.",
            "Puolikkaat pulloissa!",
            2
        ),

        info(
            TaskType.CloseSettlePlates,
            "Laskeumamaljojen sulkeminen",
            "Sulje laskeumamaljat",
            "Varmista, että jokainen agarmalja on suljettu.",
            "Laskeumamaljat suljettu!",
            2
        ),

        info(
            TaskType.WriteSecondTime,
            "Lopetusaikojen kirjoitus",
            "Kirjoita lopetusaika laskeumamaljoihin",
            "Kirjoita kellonaika laskeumamaljoihin (1kpl soijakaseiini ja  1 kpl sabouraud-dekstroosi). Katso oikea aika kellosta.",
            "Hyvin kirjoitettu",
            2
        ),

        info(
            TaskType.Fingerprints,
            "Sormenpäämaljat",
            "Avaa sormenpäämaljat ja anna näytteet",
            "Koske agaria oikealla kädellä kahdesti, kerran peukalolla ja kerran muilla sormilla. Toista toisella kädellä toiseen maljaan.",
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
            "Siirrä koriin: pienet pullot (4kpl), isot pullot (3kpl), maljat (4kpl), pinsetit",
            "Valmiit esineet korissa!",
            2
        ),

        info(
            TaskType.CleanLaminarCabinetMembrane,
            "Laminaarikaapin putsaus",
            "Putsaa laminaarikaapin seinät",
            "Ruiskuta etanoliliuosta (suihkepullossa) laminaarikaapin jokaiselle seinälle.",
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
            Translator.Translate("DressingRoom", "WearShoeCoversAndLabCoat1"),
            Translator.Translate("DressingRoom", "WearShoeCoversAndLabCoat2"),
            Translator.Translate("DressingRoom", "WearShoeCoversAndLabCoat3"),
            Translator.Translate("DressingRoom", "WearShoeCoversAndLabCoat4"),
            2
        ),

        info(
            TaskType.WashGlasses,
            Translator.Translate("DressingRoom", "WashGlasses1"),
            Translator.Translate("DressingRoom", "WashGlasses2"),
            Translator.Translate("DressingRoom", "WashGlasses3"),
            Translator.Translate("DressingRoom", "WashGlasses4"),
            2
        ),

        info(
            TaskType.GoToPreperationRoom,
            "",
            Translator.Translate("DressingRoom", "GoToPreperationRoom"),
            "",
            "",
            0
        ),

        info(
            TaskType.WashHandsInChangingRoom,
            Translator.Translate("DressingRoom", "WashHandsInChangingRoom1"),
            Translator.Translate("DressingRoom", "WashHandsInChangingRoom2"),
            Translator.Translate("DressingRoom", "WashHandsInChangingRoom3"),
            Translator.Translate("DressingRoom", "WashHandsInChangingRoom4"),
            2
        ),

        info(
            TaskType.WearHeadCoverAndFaceMask,
            Translator.Translate("DressingRoom", "WearHeadCoverAndFaceMask1"),
            Translator.Translate("DressingRoom", "WearHeadCoverAndFaceMask2"),
            Translator.Translate("DressingRoom", "WearHeadCoverAndFaceMask3"),
            Translator.Translate("DressingRoom", "WearHeadCoverAndFaceMask4"),
            2
        ),

        info(
            TaskType.WashHandsInPreperationRoom,
            Translator.Translate("DressingRoom", "WashHandsInPreperationRoom1"),
            Translator.Translate("DressingRoom", "WashHandsInPreperationRoom2"),
            Translator.Translate("DressingRoom", "WashHandsInPreperationRoom3"),
            Translator.Translate("DressingRoom", "WashHandsInPreperationRoom4"),
            2
        ),

        info(
            TaskType.WearSleeveCoversAndProtectiveGloves,
            Translator.Translate("DressingRoom", "WearSleeveCoversAndProtectiveGloves1"),
            Translator.Translate("DressingRoom", "WearSleeveCoversAndProtectiveGloves2"),
            Translator.Translate("DressingRoom", "WearSleeveCoversAndProtectiveGloves3"),
            Translator.Translate("DressingRoom", "WearSleeveCoversAndProtectiveGloves4"),
            2
        ),

        info(
            TaskType.FinishChangingRoom,
            Translator.Translate("DressingRoom","FinishChangingRoom"),
            "",
            "",
            "",
            0
        ),

    }.ToDictionary(pair => pair.Key, pair => pair.Value);

    public static void reInitDictionary()
    {
        dict.Clear();
        dict = new List<KeyValuePair<TaskType, Info>>{

        // Medicine preparation scene starts here

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
            TaskType.DisinfectBottleCap,
            "Lääkepullon desinfiointi",
            "Desinfioi lääkepullon korkki steriilillä pyyhkeellä.",
            "Desinfioi lääkepullon korkki steriilillä pyyhkeellä.",
            "Lääkepullo desinfioitu!",
            1
        ),

        info(
            TaskType.MedicineToSyringe,
            "Lääkkeen ottaminen pullosta",
            "Ota ruiskulla ja neulalla lääkettä lääkeainepullosta. Irroita neula kun olet valmis.",
            "Valitse 20ml ruisku ja kiinnitä siihen neula. Ota lääkettä ruiskuun 0,9ml verran.",
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
            "Jokaisessa ruiskussa lääkettä",
            "Toista samat vaiheet muille ruiskuille.",
            "Vedä jokaiseen ruiskuun oikea määrä lääkettä (0,15ml). Älä työnnä lääkettä isosta ruiskusta.",
            "Kaikissa ruiskuissa tarpeeksi lääkettä!",
            5
        ),

        info(
            TaskType.ItemsToSterileBag,
            "Ruiskujen siirto sterilointipussiin",
            "Sulje ruiskut korkilla ja siirrä ne sterilointipussiin.",
            "Ota korkkeja korkkipussista ja sulje ruiskujen päät. Korkkien asettamisen jälkeen siirrä ruiskut sterilointipussiin.",
            "Ruiskut laitettiin sterilointipussiin!",
            2
        ),

        info(
            TaskType.CleanTrashMedicine,
            "Jätteiden lajittelu",
            "Lajittele jätteet.",
            "Laita neula terävien roskiin ja muut jätteet tavalliseen roskakoriin.",
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

        info( // redundant?
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
            "Lista tarvittavista työvälineistä löytyy pöydältä. Puhdista työvälineet 70% etanoliliuoksella (suihkepullossa) ennen läpiantokaappiin viemistä.",
            "Oikeat työvälineet läpiantokaapissa.",
            2
        ),

        info(
            TaskType.CorrectItemsInLaminarCabinetMembrane,
            "Oikeat välineet laminaarikaapissa",
            "Siirrä työvälineet laminaarikaappiin",
            "Puhdista työvälineet ennen laminaarikaappiin vientiä 70% etanoliliuoksella (suihkepullossa). Tarvitset läpiantokaapissa olevat työvälineet ja kynän.",
            "Oikea määrä työvälineitä laminaarikaapissa.",
            2
        ),

        info(
            TaskType.WriteTextsToItems,
            "Tietojen kirjoitus esineisiin",
            "Kirjoita tarvittavat tiedot pulloihin ja maljoihin",
            "Kirjoita 100ml pulloihin (2kpl soijakaseiini, 2kpl tioglykolaatti) nimesi, päivämäärä, ja elatusaine. \n Kirjoita laskeumamaljoihin (1kpl soijakaseiini, 1kpl sabouraud-dekstroosi) nimesi, päivämäärä ja kellonaika. \n Kirjoita sormenpäämaljoihin (2kpl soijakaseiini) nimesi, päivämäärä ja oikea/vasen käsi.",
            "Hyvin kirjoitettu.",
            2
        ),

        info(
            TaskType.OpenAgarplates,
            "Laskeumamaljojen avaus",
            "Avaa laskeumamaljat",
            "Avaa sabouraud-dekstroosimalja sekä se soijakaseiinimalja, johon kirjoitit kellonajan.",
            "Hienoa, agarmaljat avattu!",
            2
        ),

        info(
            TaskType.FillBottles,
            "100ml pullojen täyttö",
            "Lisää 100ml pulloihin elatusaineita",
            "Lisää 80ml elatusainetta kuhunkin pulloon. Tarvitset 2 pulloa soijakaseiinia ja 2 pulloa tioglykolaattia. Muista katsoa, että laitat oikeaa ainetta oikeaan pulloon. \n Mittaa elatusaineet pipettorilla. Avaa mittapipetti suojapakkauksestaan ja kiinnitä pipettoriin. Muista vaihtaa mittapipetti vaihtaessasi liuosta.",
            "Hienoa, pullot täytetty",
            4
        ),

        info(
            TaskType.AssemblePump,
            "Pumpun kokoaminen",
            "Kokoa pumppu ja kiinnitä jäteletku",
            "Avaa suodatin suojapakkauksestaan ja kiinnitä se pumppuun. Kiinnitä jäteletku lääkejäteastiaan.",
            "Hienoa, pumppu on koossa",
            3
        ),

        info(
            TaskType.WetFilter,
            "Suodattimen kostutus",
            "Kostuta kalvosuodatin",
            "Avaa suodattimen kansi ja lisää suodattimeen 1ml peptonivettä finnpipetillä.",
            "Hienosti kostutettu!",
            2
        ),

        info(
            TaskType.StartPump,
            "Kostutusliuoksen suodatus",
            "Käynnistä pumppu",
            "Käynnistä pumppu painamalla käynnistysnappulaa. Muista sulkea kansi.",
            "Kostutettu!",
            2
        ),

        info(
            TaskType.MedicineToFilter,
            "Lääkkeen lisääminen",
            "Lisää lääke suodattimeen",
            "Ota lääkeruisku sterilointipussista ja lisää lääke suodattimeen.",
            "Lääke lisätty",
            2
        ),

        info(
            TaskType.StartPumpAgain,
            "Lääkevalmisteen suodatus",
            "Käynnistä pumppu",
            "Käynnistä pumppu painamalla käynnistysnappulaa. Muista sulkea kansi.",
            "Suodatettu!",
            2
        ),

        info(
            TaskType.CutFilter,
            "Kalvosuodattimen leikkaus",
            "Pura pumppu ja leikkaa suodatin",
            "Irrota suodatin pumpusta ja leikkaa sen pohjalla oleva kalvosuodatin varovasti skalpellilla kahteen osaan. Muista avata skalpellin pakkaus oikeasta päästä.",
            "Hienoa, suodatin leikattu",
            2
        ),

        info(
            TaskType.FilterHalvesToBottles,
            "Suodattimen puolikkaiden siirtäminen pulloihin",
            "Siirrä puolikkaat pulloihin pinseteillä",
            "Siirrä puolikkaat elatusliuoksiin pieniin pulloihin, toinen soijakaseiiniin ja toinen tioglykolaattiin. Muista avata pinsettien pakkaus oikeasta päästä.",
            "Puolikkaat pulloissa!",
            2
        ),

        info(
            TaskType.CloseSettlePlates,
            "Laskeumamaljojen sulkeminen",
            "Sulje laskeumamaljat",
            "Varmista, että jokainen agarmalja on suljettu.",
            "Laskeumamaljat suljettu!",
            2
        ),

        info(
            TaskType.WriteSecondTime,
            "Lopetusaikojen kirjoitus",
            "Kirjoita lopetusaika laskeumamaljoihin",
            "Kirjoita kellonaika laskeumamaljoihin (1kpl soijakaseiini ja  1 kpl sabouraud-dekstroosi). Katso oikea aika kellosta.",
            "Hyvin kirjoitettu",
            2
        ),

        info(
            TaskType.Fingerprints,
            "Sormenpäämaljat",
            "Avaa sormenpäämaljat ja anna näytteet",
            "Koske agaria oikealla kädellä kahdesti, kerran peukalolla ja kerran muilla sormilla. Toista toisella kädellä toiseen maljaan.",
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
            "Siirrä koriin: pienet pullot (4kpl), isot pullot (3kpl), maljat (4kpl), pinsetit",
            "Valmiit esineet korissa!",
            2
        ),

        info(
            TaskType.CleanLaminarCabinetMembrane,
            "Laminaarikaapin putsaus",
            "Putsaa laminaarikaapin seinät",
            "Ruiskuta etanoliliuosta (suihkepullossa) laminaarikaapin jokaiselle seinälle.",
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
            Translator.Translate("DressingRoom", "WearShoeCoversAndLabCoat1"),
            Translator.Translate("DressingRoom", "WearShoeCoversAndLabCoat2"),
            Translator.Translate("DressingRoom", "WearShoeCoversAndLabCoat3"),
            Translator.Translate("DressingRoom", "WearShoeCoversAndLabCoat4"),
            2
        ),

        info(
            TaskType.WashGlasses,
            Translator.Translate("DressingRoom", "WashGlasses1"),
            Translator.Translate("DressingRoom", "WashGlasses2"),
            Translator.Translate("DressingRoom", "WashGlasses3"),
            Translator.Translate("DressingRoom", "WashGlasses4"),
            2
        ),

        info(
            TaskType.GoToPreperationRoom,
            "",
            Translator.Translate("DressingRoom", "GoToPreperationRoom"),
            "",
            "",
            0
        ),

        info(
            TaskType.WashHandsInChangingRoom,
            Translator.Translate("DressingRoom", "WashHandsInChangingRoom1"),
            Translator.Translate("DressingRoom", "WashHandsInChangingRoom2"),
            Translator.Translate("DressingRoom", "WashHandsInChangingRoom3"),
            Translator.Translate("DressingRoom", "WashHandsInChangingRoom4"),
            2
        ),

        info(
            TaskType.WearHeadCoverAndFaceMask,
            Translator.Translate("DressingRoom", "WearHeadCoverAndFaceMask1"),
            Translator.Translate("DressingRoom", "WearHeadCoverAndFaceMask2"),
            Translator.Translate("DressingRoom", "WearHeadCoverAndFaceMask3"),
            Translator.Translate("DressingRoom", "WearHeadCoverAndFaceMask4"),
            2
        ),

        info(
            TaskType.WashHandsInPreperationRoom,
            Translator.Translate("DressingRoom", "WashHandsInPreperationRoom1"),
            Translator.Translate("DressingRoom", "WashHandsInPreperationRoom2"),
            Translator.Translate("DressingRoom", "WashHandsInPreperationRoom3"),
            Translator.Translate("DressingRoom", "WashHandsInPreperationRoom4"),
            2
        ),

        info(
            TaskType.WearSleeveCoversAndProtectiveGloves,
            Translator.Translate("DressingRoom", "WearSleeveCoversAndProtectiveGloves1"),
            Translator.Translate("DressingRoom", "WearSleeveCoversAndProtectiveGloves2"),
            Translator.Translate("DressingRoom", "WearSleeveCoversAndProtectiveGloves3"),
            Translator.Translate("DressingRoom", "WearSleeveCoversAndProtectiveGloves4"),
            2
        ),

        info(
            TaskType.FinishChangingRoom,
            Translator.Translate("DressingRoom","FinishChangingRoom"),
            "",
            "",
            "",
            0
        ),

    }.ToDictionary(pair => pair.Key, pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
    }



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
