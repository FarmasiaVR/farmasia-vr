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
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInThroughputMembrane1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInThroughputMembrane2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInThroughputMembrane3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInThroughputMembrane4"),
            2
        ),

        info(
            TaskType.CorrectItemsInLaminarCabinetMembrane,
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInLaminarCabinetMembrane1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInLaminarCabinetMembrane2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInLaminarCabinetMembrane3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInLaminarCabinetMembrane4"),
            2
        ),

        info(
            TaskType.WriteTextsToItems,
            Translator.Translate("XR MembraneFilteration 2.0", "WriteTextsToItems1"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteTextsToItems2"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteTextsToItems3"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteTextsToItems4"),
            2
        ),

        info(
            TaskType.OpenAgarplates,
            Translator.Translate("XR MembraneFilteration 2.0", "OpenAgarplates1"),
            Translator.Translate("XR MembraneFilteration 2.0", "OpenAgarplates2"),
            Translator.Translate("XR MembraneFilteration 2.0", "OpenAgarplates3"),
            Translator.Translate("XR MembraneFilteration 2.0", "OpenAgarplates4"),
            2
        ), 

        info(
            TaskType.FillBottles,
            Translator.Translate("XR MembraneFilteration 2.0", "FillBottles1"),
            Translator.Translate("XR MembraneFilteration 2.0", "FillBottles2"),
            Translator.Translate("XR MembraneFilteration 2.0", "FillBottles3"),
            Translator.Translate("XR MembraneFilteration 2.0", "FillBottles4"),
            4
        ),

        info(
            TaskType.AssemblePump,
            Translator.Translate("XR MembraneFilteration 2.0", "AssemblePump1"),
            Translator.Translate("XR MembraneFilteration 2.0", "AssemblePump2"),
            Translator.Translate("XR MembraneFilteration 2.0", "AssemblePump3"),
            Translator.Translate("XR MembraneFilteration 2.0", "AssemblePump4"),
            3
        ),

        info(
            TaskType.WetFilter,
            Translator.Translate("XR MembraneFilteration 2.0", "WetFilter1"),
            Translator.Translate("XR MembraneFilteration 2.0", "WetFilter2"),
            Translator.Translate("XR MembraneFilteration 2.0", "WetFilter3"),
            Translator.Translate("XR MembraneFilteration 2.0", "WetFilter4"),
            2
        ),

        info(
            TaskType.StartPump,
            Translator.Translate("XR MembraneFilteration 2.0", "StartPump1"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPump2"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPump3"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPump4"),
            2
        ),

        info(
            TaskType.MedicineToFilter,
            Translator.Translate("XR MembraneFilteration 2.0", "MedicineToFilter1"),
            Translator.Translate("XR MembraneFilteration 2.0", "MedicineToFilter2"),
            Translator.Translate("XR MembraneFilteration 2.0", "MedicineToFilter3"),
            Translator.Translate("XR MembraneFilteration 2.0", "MedicineToFilter4"),
            2
        ),

        info(
            TaskType.StartPumpAgain,
            Translator.Translate("XR MembraneFilteration 2.0", "StartPumpAgain1"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPumpAgain2"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPumpAgain3"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPumpAgain4"),
            2
        ),

        info(
            TaskType.CutFilter,
            Translator.Translate("XR MembraneFilteration 2.0", "CutFilter1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CutFilter2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CutFilter3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CutFilter4"),
            2
        ),

        info(
            TaskType.FilterHalvesToBottles,
            Translator.Translate("XR MembraneFilteration 2.0", "FilterHalvesToBottles1"),
            Translator.Translate("XR MembraneFilteration 2.0", "FilterHalvesToBottles2"),
            Translator.Translate("XR MembraneFilteration 2.0", "FilterHalvesToBottles3"),
            Translator.Translate("XR MembraneFilteration 2.0", "FilterHalvesToBottles4"),
            2
        ),

        info(
            TaskType.CloseSettlePlates,
            Translator.Translate("XR MembraneFilteration 2.0", "CloseSettlePlates1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseSettlePlates2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseSettlePlates3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseSettlePlates4"),
            2
        ),

        info(
            TaskType.WriteSecondTime,
            Translator.Translate("XR MembraneFilteration 2.0", "WriteSecondTime1"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteSecondTime2"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteSecondTime3"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteSecondTime4"),
            2
        ),

        info(
            TaskType.Fingerprints,
            Translator.Translate("XR MembraneFilteration 2.0", "Fingerprints1"),
            Translator.Translate("XR MembraneFilteration 2.0", "Fingerprints2"),
            Translator.Translate("XR MembraneFilteration 2.0", "Fingerprints3"),
            Translator.Translate("XR MembraneFilteration 2.0", "Fingerprints4"),
            2
        ),

        info(
            TaskType.CloseFingertipPlates,
            Translator.Translate("XR MembraneFilteration 2.0", "CloseFingertipPlates1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseFingertipPlates2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseFingertipPlates3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseFingertipPlates4"),
            2
        ),

        info(
            TaskType.CloseBottles,
            Translator.Translate("XR MembraneFilteration 2.0", "CloseBottles1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseBottles2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseBottles3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseBottles4"),
            2
        ),

        info(
            TaskType.CleanTrashMembrane,
            Translator.Translate("XR MembraneFilteration 2.0", "CleanTrashMembrane1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanTrashMembrane2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanTrashMembrane3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanTrashMembrane4"),
            2
        ),

        info(
            TaskType.CorrectItemsInBasketMembrane,
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInBasketMembrane1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInBasketMembrane2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInBasketMembrane3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInBasketMembrane4"),
            2
        ),

        info(
            TaskType.CleanLaminarCabinetMembrane,
            Translator.Translate("XR MembraneFilteration 2.0", "CleanLaminarCabinetMembrane1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanLaminarCabinetMembrane2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanLaminarCabinetMembrane3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanLaminarCabinetMembrane4"),
            2
        ),

        info(
            TaskType.FinishMembrane,
            Translator.Translate("XR MembraneFilteration 2.0", "FinishMembrane"),
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
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInThroughputMembrane1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInThroughputMembrane2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInThroughputMembrane3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInThroughputMembrane4"),
            2
        ),

        info(
            TaskType.CorrectItemsInLaminarCabinetMembrane,
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInLaminarCabinetMembrane1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInLaminarCabinetMembrane2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInLaminarCabinetMembrane3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInLaminarCabinetMembrane4"),
            2
        ),

        info(
            TaskType.WriteTextsToItems,
            Translator.Translate("XR MembraneFilteration 2.0", "WriteTextsToItems1"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteTextsToItems2"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteTextsToItems3"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteTextsToItems4"),
            2
        ),

        info(
            TaskType.OpenAgarplates,
            Translator.Translate("XR MembraneFilteration 2.0", "OpenAgarplates1"),
            Translator.Translate("XR MembraneFilteration 2.0", "OpenAgarplates2"),
            Translator.Translate("XR MembraneFilteration 2.0", "OpenAgarplates3"),
            Translator.Translate("XR MembraneFilteration 2.0", "OpenAgarplates4"),
            2
        ),

        info(
            TaskType.FillBottles,
            Translator.Translate("XR MembraneFilteration 2.0", "FillBottles1"),
            Translator.Translate("XR MembraneFilteration 2.0", "FillBottles2"),
            Translator.Translate("XR MembraneFilteration 2.0", "FillBottles3"),
            Translator.Translate("XR MembraneFilteration 2.0", "FillBottles4"),
            4
        ),

        info(
            TaskType.AssemblePump,
            Translator.Translate("XR MembraneFilteration 2.0", "AssemblePump1"),
            Translator.Translate("XR MembraneFilteration 2.0", "AssemblePump2"),
            Translator.Translate("XR MembraneFilteration 2.0", "AssemblePump3"),
            Translator.Translate("XR MembraneFilteration 2.0", "AssemblePump4"),
            3
        ),

        info(
            TaskType.WetFilter,
            Translator.Translate("XR MembraneFilteration 2.0", "WetFilter1"),
            Translator.Translate("XR MembraneFilteration 2.0", "WetFilter2"),
            Translator.Translate("XR MembraneFilteration 2.0", "WetFilter3"),
            Translator.Translate("XR MembraneFilteration 2.0", "WetFilter4"),
            2
        ),

        info(
            TaskType.StartPump,
            Translator.Translate("XR MembraneFilteration 2.0", "StartPump1"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPump2"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPump3"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPump4"),
            2
        ),

        info(
            TaskType.MedicineToFilter,
            Translator.Translate("XR MembraneFilteration 2.0", "MedicineToFilter1"),
            Translator.Translate("XR MembraneFilteration 2.0", "MedicineToFilter2"),
            Translator.Translate("XR MembraneFilteration 2.0", "MedicineToFilter3"),
            Translator.Translate("XR MembraneFilteration 2.0", "MedicineToFilter4"),
            2
        ),

        info(
            TaskType.StartPumpAgain,
            Translator.Translate("XR MembraneFilteration 2.0", "StartPumpAgain1"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPumpAgain2"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPumpAgain3"),
            Translator.Translate("XR MembraneFilteration 2.0", "StartPumpAgain4"),
            2
        ),

        info(
            TaskType.CutFilter,
            Translator.Translate("XR MembraneFilteration 2.0", "CutFilter1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CutFilter2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CutFilter3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CutFilter4"),
            2
        ),

        info(
            TaskType.FilterHalvesToBottles,
            Translator.Translate("XR MembraneFilteration 2.0", "FilterHalvesToBottles1"),
            Translator.Translate("XR MembraneFilteration 2.0", "FilterHalvesToBottles2"),
            Translator.Translate("XR MembraneFilteration 2.0", "FilterHalvesToBottles3"),
            Translator.Translate("XR MembraneFilteration 2.0", "FilterHalvesToBottles4"),
            2
        ),

        info(
            TaskType.CloseSettlePlates,
            Translator.Translate("XR MembraneFilteration 2.0", "CloseSettlePlates1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseSettlePlates2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseSettlePlates3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseSettlePlates4"),
            2
        ),

        info(
            TaskType.WriteSecondTime,
            Translator.Translate("XR MembraneFilteration 2.0", "WriteSecondTime1"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteSecondTime2"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteSecondTime3"),
            Translator.Translate("XR MembraneFilteration 2.0", "WriteSecondTime4"),
            2
        ),

        info(
            TaskType.Fingerprints,
            Translator.Translate("XR MembraneFilteration 2.0", "Fingerprints1"),
            Translator.Translate("XR MembraneFilteration 2.0", "Fingerprints2"),
            Translator.Translate("XR MembraneFilteration 2.0", "Fingerprints3"),
            Translator.Translate("XR MembraneFilteration 2.0", "Fingerprints4"),
            2
        ),

        info(
            TaskType.CloseFingertipPlates,
            Translator.Translate("XR MembraneFilteration 2.0", "CloseFingertipPlates1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseFingertipPlates2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseFingertipPlates3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseFingertipPlates4"),
            2
        ),

        info(
            TaskType.CloseBottles,
            Translator.Translate("XR MembraneFilteration 2.0", "CloseBottles1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseBottles2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseBottles3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CloseBottles4"),
            2
        ),

        info(
            TaskType.CleanTrashMembrane,
            Translator.Translate("XR MembraneFilteration 2.0", "CleanTrashMembrane1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanTrashMembrane2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanTrashMembrane3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanTrashMembrane4"),
            2
        ),

        info(
            TaskType.CorrectItemsInBasketMembrane,
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInBasketMembrane1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInBasketMembrane2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInBasketMembrane3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CorrectItemsInBasketMembrane4"),
            2
        ),

        info(
            TaskType.CleanLaminarCabinetMembrane,
            Translator.Translate("XR MembraneFilteration 2.0", "CleanLaminarCabinetMembrane1"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanLaminarCabinetMembrane2"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanLaminarCabinetMembrane3"),
            Translator.Translate("XR MembraneFilteration 2.0", "CleanLaminarCabinetMembrane4"),
            2
        ),

        info(
            TaskType.FinishMembrane,
            Translator.Translate("XR MembraneFilteration 2.0", "FinishMembrane"),
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
