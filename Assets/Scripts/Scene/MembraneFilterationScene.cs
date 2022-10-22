using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine;

class MembraneFilterationScene : SceneScript {

    public enum AutoPlayStrength {
        None = 0,
        ItemsToPassThroughCabinet,
        GoToWorkspaceRoom,
        ItemsToLaminarCabinet,
        WriteItems,
        OpenAgarPlates,
        FillBottles,
        AssemblePump,
        PeptoneToFilter,
        MedicineToFilter,
        DisassemblePump,
        CutFilter,
        FilterHalvesToBottles,
        CloseSettlePlates,
        WriteItemsAgain,
        TakeFingerprints,
        CloseFingertipPlates,
        CloseBottles,
        CleanTrash,
        ItemsToBasket,
        CleanLaminarCabinet
    }

    public AutoPlayStrength autoPlayStrength;
    public bool skipFingertips;
    public List<GameObject> preperationRoomObjects;
    public GameObject automaticPipette, pipette, pump, tweezers, scalpel, pipetteInCover1, pipetteInCover2, filterInCover,
        soycaseinePlate1, soycaseinePlate2, soycaseinePlate3, sabouraudDextrosePlate, bottle1, bottle2, bottle3, bottle4, sterileBag,
        tioglycolateBottle, peptoneWaterBottle, soycaseineBottle,
        cleaningBottle, writingPen, syringe, syringeCap;
    [Header("Scene items")]
    public TrashCan smallTrashCan;
    public TrashCan sharpTrashCan;
    public Transform preperationRoomPassThroughCabinetPositions;
    public Transform workspaceRoomLaminarCabinetPositions;
    public Interactable doorHandle;
    public Interactable pipeConnectorButton;
    public static byte[] SavedScoreState;

    // testing
    private bool played;
    public bool IsAutoPlaying { get; private set; }

    protected override void Start() {
        base.Start();
        // if (!MainMenuFunctions.startFromBeginning) autoPlayStrength = AutoPlayStrength.ItemsToPassThroughCabinet;
        // GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
        // PlayFirstRoom(autoPlayStrength);

        if (MainMenuFunctions.selectedAutoplay == MainMenuFunctions.SelectedAutoplay.Workspace) {
            autoPlayStrength = AutoPlayStrength.ItemsToPassThroughCabinet;
        } else if (MainMenuFunctions.selectedAutoplay == MainMenuFunctions.SelectedAutoplay.CloseSettlePlates) {
            autoPlayStrength = AutoPlayStrength.CloseSettlePlates;
            // GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
        } else if (MainMenuFunctions.selectedAutoplay == MainMenuFunctions.SelectedAutoplay.CloseFingertipPlates) {
            autoPlayStrength = AutoPlayStrength.CloseFingertipPlates;
            // GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
        } else if (MainMenuFunctions.selectedAutoplay == MainMenuFunctions.SelectedAutoplay.FilterHalvesToBottles) {
            autoPlayStrength = AutoPlayStrength.FilterHalvesToBottles;
            GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
        }


        PlayFirstRoom(autoPlayStrength);

    }

    
    public void PlayFirstRoom(AutoPlayStrength strength = AutoPlayStrength.None) {
    if (autoPlayStrength == 0) return;
    CoroutineUtils.StartThrowingCoroutine(this, PlayCoroutine(autoPlayStrength),
        exception => {
            if (exception != null)
                Logger.Error(exception);
            Logger.Print("Autoplay finished");
            }
        );
    }

    /*
    public void PlayFirstRoom(AutoPlayStrength strength = AutoPlayStrength.None) {

        if (IsAutoPlaying || played || strength == 0) {
            return;
        }
        played = true;
        IsAutoPlaying = true;

        StartCoroutine(PlayCoroutine(strength));
    }
    */

    private IEnumerator PlayCoroutine(AutoPlayStrength strength) {
        Hand leftHand = VRInput.Hands[0].Hand;
        Hand rightHand = VRInput.Hands[1].Hand;
        // Create objects from prefabs and store in a list. They must be in the correct order here!
        List<GameObject> workspaceRoomObjects = new List<GameObject>() {
            automaticPipette, pipette, pump, tweezers, scalpel, pipetteInCover1, pipetteInCover2, filterInCover,
            soycaseinePlate1, soycaseinePlate2, soycaseinePlate3, sabouraudDextrosePlate, bottle1, bottle2, bottle3, bottle4, sterileBag,
            tioglycolateBottle, peptoneWaterBottle, soycaseineBottle
        };

        // --- ItemsToPassThroughCabinet ---

        cleaningBottle.transform.GetChild(1).gameObject.transform.localScale = new Vector3(5.0f, 1.0f, 1.0f);
        for (int i = 0; i < preperationRoomObjects.Count; i++) {
            DropAt(preperationRoomObjects[i].transform, new Vector3(-0.15f, 0.94f, 2.0f));
            cleaningBottle.GetComponent<CleaningBottle>().Clean();
            yield return Wait();
            DropAt(preperationRoomObjects[i].transform, preperationRoomPassThroughCabinetPositions.GetChild(i).transform.position);
        }

        /*
        if (strength == AutoPlayStrength.ItemsToPassThroughCabinet) yield break;
        yield return Wait();
        */

        if (strength == AutoPlayStrength.ItemsToPassThroughCabinet) {
            if (!MainMenuFunctions.startFromBeginning)
                GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
            yield break;
        }
        /*
        // --- Teleport if strength is AutoPlayStrenght.closeSettlePlates ---
        if (strength == AutoPlayStrength.CloseSettlePlates) {
            GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
            yield break;
        }

        // --- Teleport if strength is AutoPlayStrenght.closeFingertipPlates ---
        if (strength == AutoPlayStrength.CloseFingertipPlates) {
            GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
            yield break;
        }

  

        //--- Teleport if strength is AutoPlayStrenght.FilterHalvesToBottles ---
        if (strength == AutoPlayStrength.FilterHalvesToBottles) {
            GameObject.Find("GObject").GetComponent<RoomTeleport>().TeleportPlayer();
            yield break;
        }

         */


        // --- GoToWorkspaceRoom ---

        leftHand.InteractWith(doorHandle);
        leftHand.Uninteract();
        if (strength == AutoPlayStrength.GoToWorkspaceRoom) yield break;
        yield return Wait();



        // --- ItemsToLaminarCabinet ---

        for (int i = 0; i < workspaceRoomObjects.Count; i++) {
            DropAt(workspaceRoomObjects[i].transform, new Vector3(-0.15f, 0.94f, 2.0f));
            cleaningBottle.GetComponent<CleaningBottle>().Clean();
            yield return Wait();
            DropAt(workspaceRoomObjects[i].transform, workspaceRoomLaminarCabinetPositions.GetChild(i).transform.position);
        }
        cleaningBottle.transform.GetChild(1).gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        if (strength == AutoPlayStrength.ItemsToLaminarCabinet) yield break;
        yield return Wait();

        // --- WriteItems ---

        WritingPen pen = ToInteractable(writingPen) as WritingPen;
        AgarPlateLid soycaseinePlateLid1 = soycaseinePlate1.GetComponentInChildren<AgarPlateLid>();
        AgarPlateLid soycaseinePlateLid2 = soycaseinePlate2.GetComponentInChildren<AgarPlateLid>();
        AgarPlateLid soycaseinePlateLid3 = soycaseinePlate3.GetComponentInChildren<AgarPlateLid>();
        AgarPlateLid sabouraudDextrosePlateLid = sabouraudDextrosePlate.GetComponentInChildren<AgarPlateLid>();
        Bottle bottleSoycaseine1 = bottle1.GetComponentInChildren<Bottle>();
        Bottle bottleSoycaseine2 = bottle2.GetComponentInChildren<Bottle>();
        Bottle bottleTioglycolate1 = bottle3.GetComponentInChildren<Bottle>();
        Bottle bottleTioglycolate2 = bottle4.GetComponentInChildren<Bottle>();
        // Mark lids
        var writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, Player.Info.Name ?? "Nimi"},
            {WritingType.Date, DateTime.UtcNow.ToLocalTime().ToShortDateString()},
            {WritingType.Time, DateTime.UtcNow.ToLocalTime().ToShortTimeString()}
        };
        pen.SubmitWriting(soycaseinePlateLid1.GetComponent<Writable>(), soycaseinePlateLid1.gameObject, writing);
        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, Player.Info.Name ?? "Nimi"},
            {WritingType.Date, DateTime.UtcNow.ToLocalTime().ToShortDateString()},
            {WritingType.Time, DateTime.UtcNow.ToLocalTime().ToShortTimeString()}
        };
        pen.SubmitWriting(sabouraudDextrosePlateLid.GetComponent<Writable>(), sabouraudDextrosePlateLid.gameObject, writing);
        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, Player.Info.Name ?? "Nimi"},
            {WritingType.Date, DateTime.UtcNow.ToLocalTime().ToShortDateString()},
            {WritingType.RightHand, "Vasen käsi"},
            {WritingType.Time, DateTime.UtcNow.ToLocalTime().ToShortTimeString()}
        };
        pen.SubmitWriting(soycaseinePlateLid2.GetComponent<Writable>(), soycaseinePlateLid2.gameObject, writing);
        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, Player.Info.Name ?? "Nimi"},
            {WritingType.Date, DateTime.UtcNow.ToLocalTime().ToShortDateString()},
            {WritingType.LeftHand, "Oikea käsi"},
            {WritingType.Time, DateTime.UtcNow.ToLocalTime().ToShortTimeString()}
        };
        pen.SubmitWriting(soycaseinePlateLid3.GetComponent<Writable>(), soycaseinePlateLid3.gameObject, writing);
        // Mark bottles
        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, Player.Info.Name ?? "Nimi"},
            {WritingType.Date, DateTime.UtcNow.ToLocalTime().ToShortDateString()},
            {WritingType.SoyCaseine, "Soijakaseiini"},
        };
        pen.SubmitWriting(bottleSoycaseine1.GetComponent<Writable>(), bottleSoycaseine1.gameObject, writing);
        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, Player.Info.Name ?? "Nimi"},
            {WritingType.Date, DateTime.UtcNow.ToLocalTime().ToShortDateString()},
            {WritingType.SoyCaseine, "Soijakaseiini"},
        };
        pen.SubmitWriting(bottleSoycaseine2.GetComponent<Writable>(), bottleSoycaseine2.gameObject, writing);
        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, Player.Info.Name ?? "Nimi"},
            {WritingType.Date, DateTime.UtcNow.ToLocalTime().ToShortDateString()},
            {WritingType.Tioglygolate, "Tioglykolaatti"},
        };
        pen.SubmitWriting(bottleTioglycolate1.GetComponent<Writable>(), bottleTioglycolate1.gameObject, writing);
        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, Player.Info.Name ?? "Nimi"},
            {WritingType.Date, DateTime.UtcNow.ToLocalTime().ToShortDateString()},
            {WritingType.Tioglygolate, "Tioglykolaatti"},
        };
        pen.SubmitWriting(bottleTioglycolate2.GetComponent<Writable>(), bottleTioglycolate2.gameObject, writing);
        if (strength == AutoPlayStrength.WriteItems) yield break;
        yield return Wait();

        // --- OpenAgarPlates ---

        soycaseinePlateLid1.ReleaseItem();
        sabouraudDextrosePlateLid.ReleaseItem();
        DropAt(soycaseinePlateLid1.transform, soycaseinePlateLid1.transform.position + new Vector3(0.06f, 0.1f, 0.0f));
        DropAt(sabouraudDextrosePlateLid.transform, sabouraudDextrosePlateLid.transform.position + new Vector3(0.06f, 0.1f, 0.0f));
        soycaseinePlateLid1.transform.eulerAngles = new Vector3(180.0f, 0.0f, 0.0f);
        sabouraudDextrosePlateLid.transform.transform.eulerAngles = new Vector3(180.0f, 0.0f, 0.0f);
        if (strength == AutoPlayStrength.OpenAgarPlates) yield break;
        yield return Wait();

        // --- FillBottles ---

        // Unbottle everything
        new List<GameObject> { bottle1, bottle2, bottle3, bottle4, soycaseineBottle, tioglycolateBottle, peptoneWaterBottle }.ForEach(gameObject => {
            BottleCap cap = gameObject.transform.GetComponentInChildren<BottleCap>();
            cap.Connector.Connection.Remove();
            DropAt(cap.transform, cap.transform.position + Vector3.forward * 0.1f);
        });
        // Attach serological pipette to automatic pipette
        pipetteInCover1.GetComponent<Cover>().OpenCover(leftHand);
        pipetteInCover2.GetComponent<Cover>().OpenCover(leftHand);
        FreezeAt(automaticPipette.transform, new Vector3(-18.57f, 1.3f, 2.0f));
        yield return Wait(0.1f);
        GameObject serologicalPipette = GameObject.Find("PipetteHead50ml");
        DropAt(serologicalPipette.transform, automaticPipette.transform.position + Vector3.down * 0.08f);
        yield return Wait();
        // Fill bottles
        Bottle soycaseine = soycaseineBottle.GetComponentInChildren<Bottle>();
        Bottle tioglycolate = tioglycolateBottle.GetComponentInChildren<Bottle>();
        BigPipette bigPipette = ToInteractable(automaticPipette) as BigPipette;
        var sets = new List<(Bottle, Bottle, BigPipette)>() {
            (soycaseine, bottleSoycaseine1, bigPipette),
            (soycaseine, bottleSoycaseine2, bigPipette),
            (tioglycolate, bottleTioglycolate1, bigPipette),
            (tioglycolate, bottleTioglycolate2, bigPipette),
        };
        foreach ((Bottle, Bottle, BigPipette) set in sets) {
            var (liquid, fillable, tool) = set;
            FreezeAt(tool.transform, liquid.transform.position + Vector3.up * 0.34f);
            yield return Wait();
            for (int i = 0; i < 3; i++) {
                tool.TakeMedicine();
                yield return Wait();
            }
            FreezeAt(tool.transform, fillable.transform.position + Vector3.up * 0.25f);
            yield return Wait();
            for (int i = 0; i < 3; i++) {
                tool.SendMedicine();
                yield return Wait();
            }
            FreezeAt(tool.transform, new Vector3(-18.57f, 1.3f, 2.0f));
            yield return Wait();
        };
        Vector3 corner = new Vector3(-19.15f, 1.3f, 2.15f);
        DropAt(automaticPipette.transform, corner);
        if (strength == AutoPlayStrength.FillBottles) yield break;
        yield return Wait();


        // --- AssemblePump ---

        filterInCover.GetComponent<Cover>().OpenCover(leftHand);
        DropAt(filterInCover.transform, pump.transform.position + Vector3.up * 0.12f);
        yield return Wait();
        leftHand.InteractWith(pipeConnectorButton);
        leftHand.Uninteract();
        if (strength == AutoPlayStrength.AssemblePump) yield break;
        yield return Wait();

        // --- PeptoneToFilter ---

        // Remove filter lid
        FilterPart lid = ToInteractable(filterInCover.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Lid")?.gameObject) as FilterPart;
        StartCoroutine(lid.WaitForDistance(leftHand));
        lid.SnapDistance = -1.0f;
        yield return Wait();
        leftHand.Uninteract();
        DropAt(lid.transform, pump.transform.position + Vector3.forward * 0.1f);
        lid.SnapDistance = 0.03f;
        // Add peptone water
        Pipette p = ToInteractable(pipette) as Pipette;
        FreezeAt(pipette.transform, peptoneWaterBottle.transform.position + Vector3.up * 0.02f);
        pipette.transform.eulerAngles = new Vector3(-180.0f, 0.0f, 0.0f);
        yield return Wait();
        p.TakeMedicine();
        yield return Wait();
        FreezeAt(pipette.transform, new Vector3(-18.57f, 1.3f, 2.0f));
        yield return Wait();
        FreezeAt(pipette.transform, pump.transform.position + Vector3.up * 0.24f);
        pipette.transform.eulerAngles = new Vector3(-180.0f, 0.0f, 0.0f);
        yield return Wait();
        p.SendMedicine();
        yield return Wait();
        DropAt(pipette.transform, corner);
        // Activate pump
        FilteringButton pumpButton = pump.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Push")?.gameObject.GetComponent<FilteringButton>();
        pumpButton.RunPump();
        if (strength == AutoPlayStrength.PeptoneToFilter) yield break;
        yield return Wait();

        // --- MedicineToFilter ---

        sterileBag.GetComponent<SterileBag2>().ReleaseSyringe();
        // Waiting for syringes to be released out of sterile bag
        yield return Wait(1.0f);
        // Remove syringe cap
        SyringeCapConnect syringeCapConnect = ToInteractable(syringeCap) as SyringeCapConnect;
        StartCoroutine(syringeCapConnect.WaitForDistance(leftHand));
        syringeCapConnect.SnapDistance = -1.0f;
        yield return Wait();
        leftHand.Uninteract();
        DropAt(syringeCapConnect.transform, corner);
        syringeCapConnect.SnapDistance = 0.03f;
        // Add medicine
        SyringeNew s = ToInteractable(syringe) as SyringeNew;
        FreezeAt(syringe.transform, pump.transform.position + Vector3.up * 0.24f);
        syringe.transform.eulerAngles = new Vector3(-180.0f, 0.0f, 0.0f);
        yield return Wait();
        s.SendMedicine(150);
        yield return Wait();
        DropAt(syringe.transform, corner);
        // Activate pump
        pumpButton.RunPump();
        if (strength == AutoPlayStrength.MedicineToFilter) yield break;
        yield return Wait();

        // --- DisassemblePump ---

        // Waiting for filtering process to finish before starting to disassemble
        yield return Wait(1.0f);
        // Remove filter tank
        FilterPart tank = ToInteractable(filterInCover.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Tank")?.gameObject) as FilterPart;
        StartCoroutine(tank.WaitForDistance(leftHand));
        tank.SnapDistance = -1.0f;
        yield return Wait();
        leftHand.Uninteract();
        DropAt(tank.transform, pump.transform.position + Vector3.forward * 0.2f);
        tank.SnapDistance = 0.03f;
        // Remove filter from pump
        FilterInCover filter = ToInteractable(filterInCover) as FilterInCover;
        StartCoroutine(filter.WaitForDistance(leftHand));
        filter.SnapDistance = -1.0f;
        yield return Wait();
        leftHand.Uninteract();
        DropAt(filter.transform, pump.transform.position + Vector3.right * 0.15f);
        filter.SnapDistance = 0.03f;
        if (strength == AutoPlayStrength.DisassemblePump) yield break;
        yield return Wait();

        // --- CutFilter ---

        scalpel.GetComponent<Cover>().OpenCover(leftHand);
        DropAt(scalpel.transform, filter.transform.position + Vector3.up * 0.07f);
        scalpel.transform.eulerAngles = new Vector3(0, 0, -90);
        yield return Wait();
        DropAt(scalpel.transform, corner);
        if (strength == AutoPlayStrength.CutFilter) yield break;
        yield return Wait();

        // --- FilterHalvesToBottles ---



        tweezers.GetComponent<Cover>().OpenCover(leftHand);
        GameObject filterHalfL = GameObject.Find("FilterHalfL");
        GameObject filterHalfR = GameObject.Find("FilterHalfR");
        DropAt(filterHalfL.transform, bottle1.transform.position + Vector3.down * 0.1f);
        DropAt(filterHalfR.transform, bottle3.transform.position + Vector3.down * 0.1f);
        if (strength == AutoPlayStrength.FilterHalvesToBottles) yield break;
        yield return Wait();

        // --- CloseSettlePlates ---

        Transform soycaseinePlateBottom1 = soycaseinePlate1.transform.GetChild(1);
        soycaseinePlateBottom1.parent = soycaseinePlateLid1.transform;
        soycaseinePlateBottom1.localPosition = Vector3.zero;
        soycaseinePlateLid1.Connector.ConnectItem(soycaseinePlateBottom1.gameObject.GetComponent<Interactable>());
        Transform sabouraudDextrosePlateBottom = sabouraudDextrosePlate.transform.GetChild(1);
        sabouraudDextrosePlateBottom.parent = sabouraudDextrosePlateLid.transform;
        sabouraudDextrosePlateBottom.localPosition = Vector3.zero;
        sabouraudDextrosePlateLid.Connector.ConnectItem(sabouraudDextrosePlateBottom.gameObject.GetComponent<Interactable>());
        if (strength == AutoPlayStrength.CloseSettlePlates) yield break;
        yield return Wait();

        // --- WriteItemsAgain ---

        writing = new Dictionary<WritingType, string>() {
            {WritingType.SecondTime, DateTime.UtcNow.ToLocalTime().ToShortTimeString()},
        };
        pen.SubmitWriting(soycaseinePlateLid1.GetComponent<Writable>(), soycaseinePlateLid1.gameObject, writing);
        writing = new Dictionary<WritingType, string>() {
            {WritingType.SecondTime, DateTime.UtcNow.ToLocalTime().ToShortTimeString()},
        };
        pen.SubmitWriting(sabouraudDextrosePlateLid.GetComponent<Writable>(), sabouraudDextrosePlateLid.gameObject, writing);
        if (strength == AutoPlayStrength.WriteItemsAgain) yield break;
        yield return Wait();

        // --- TakeFingerprints ---

        // Open agar plates
        soycaseinePlateLid2.ReleaseItem();
        soycaseinePlateLid3.ReleaseItem();
        DropAt(soycaseinePlateLid2.transform, soycaseinePlateLid2.transform.position + new Vector3(0.06f, 0.1f, 0));
        DropAt(soycaseinePlateLid3.transform, soycaseinePlateLid3.transform.position + new Vector3(0.06f, 0.1f, 0));
        soycaseinePlateLid2.transform.Rotate(new Vector3(180.0f, 0.0f, 0.0f));
        soycaseinePlateLid3.transform.Rotate(new Vector3(180.0f, 0.0f, 0.0f));
        // Take fingerprints
        if (skipFingertips) {
            Events.FireEvent(EventType.FingerprintsGivenL);
            Events.FireEvent(EventType.FingerprintsGivenR);
        } else {
            Agar leftAgar = soycaseinePlate2.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Agar")?.gameObject.GetComponent<Agar>();
            Agar rightAgar = soycaseinePlate3.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Agar")?.gameObject.GetComponent<Agar>();
            leftAgar.Interact(leftHand);
            rightAgar.Interact(rightHand);
            yield return Wait(4.1f);
            leftAgar.Uninteract(leftHand);
            rightAgar.Uninteract(rightHand);
            leftAgar.Interact(leftHand);
            rightAgar.Interact(rightHand);
            yield return Wait(4.1f);
            leftAgar.Uninteract(leftHand);
            rightAgar.Uninteract(rightHand);
        }
        if (strength == AutoPlayStrength.TakeFingerprints) yield break;
        yield return Wait();

        // --- CloseFingertipPlates ---

        Transform soycaseinePlateBottom2 = soycaseinePlate2.transform.GetChild(1);
        soycaseinePlateBottom2.parent = soycaseinePlateLid2.transform;
        soycaseinePlateBottom2.localPosition = Vector3.zero;
        soycaseinePlateLid2.Connector.ConnectItem(soycaseinePlateBottom2.gameObject.GetComponent<Interactable>());
        Transform soycaseinePlateBottom3 = soycaseinePlate3.transform.GetChild(1);
        soycaseinePlateBottom3.parent = soycaseinePlateLid3.transform;
        soycaseinePlateBottom3.localPosition = Vector3.zero;
        soycaseinePlateLid3.Connector.ConnectItem(soycaseinePlateBottom3.gameObject.GetComponent<Interactable>());

        if (strength == AutoPlayStrength.CloseFingertipPlates) yield break;
        yield return Wait();

        // --- CloseBottles ---

        new List<GameObject> { bottle1, bottle2, bottle3, bottle4, soycaseineBottle, tioglycolateBottle, peptoneWaterBottle }.ForEach(gameObject => {
            BottleCap cap = gameObject.transform.GetComponentInChildren<BottleCap>();
            cap.AttachCap();
        });
        if (strength == AutoPlayStrength.CloseBottles) yield break;
        yield return Wait();

        // --- CleanTrash ---

        // Detach serological pipette from automatic pipette
        PipetteContainer pipetteContainer = ToInteractable(serologicalPipette) as PipetteContainer;
        StartCoroutine(pipetteContainer.WaitForDistance(leftHand));
        pipetteContainer.SnapDistance = -1.0f;
        yield return Wait();
        leftHand.Uninteract();
        DropAt(pipetteContainer.transform, corner);
        pipetteContainer.SnapDistance = 0.03f;
        // Put trash in the correct trash can
        smallTrashCan.OnTrashEnter(serologicalPipette.GetComponentInChildren<Collider>());
        smallTrashCan.OnTrashEnter(lid.GetComponentInChildren<Collider>());
        smallTrashCan.OnTrashEnter(tank.GetComponentInChildren<Collider>());
        smallTrashCan.OnTrashEnter(filterInCover.GetComponentInChildren<Collider>());
        smallTrashCan.OnTrashEnter(syringe.GetComponentInChildren<Collider>());
        sharpTrashCan.OnTrashEnter(scalpel.GetComponentInChildren<Collider>());
        smallTrashCan.OnTrashEnter(sterileBag.GetComponentInChildren<Collider>());
        yield return Wait(0.1f);
        GameObject otherSerologicalPipette = GameObject.Find("PipetteHead50ml");
        smallTrashCan.OnTrashEnter(otherSerologicalPipette.GetComponentInChildren<Collider>());
        if (syringe == null) smallTrashCan.OnTrashEnter(syringeCap.GetComponentInChildren<Collider>());
        if (strength == AutoPlayStrength.CleanTrash) yield break;
        yield return Wait();

        // --- ItemsToBasket ---

        if (strength == AutoPlayStrength.ItemsToBasket) yield break;
        yield return Wait();

        // --- CleanLaminarCabinet

        if (strength == AutoPlayStrength.CleanLaminarCabinet) yield break;
        yield return Wait();

        yield break;
    }

    private void DropAt(Transform equipment, Vector3 position) {
        equipment.position = position;
        equipment.eulerAngles = Vector3.up;
        if (equipment.GetComponent<Rigidbody>() != null) equipment.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void FreezeAt(Transform equipment, Vector3 position) {
        equipment.position = position;
        equipment.eulerAngles = Vector3.up;
        if (equipment.GetComponent<Rigidbody>() != null) equipment.GetComponent<Rigidbody>().isKinematic = true;
    }

    private WaitForSeconds Wait() {
        return new WaitForSeconds(0.04f);
    }

    private WaitForSeconds Wait(float seconds) {
        return new WaitForSeconds(seconds);
    }

    private Interactable ToInteractable(GameObject g) {
        var interactable = Interactable.GetInteractable(g.transform);
        if (interactable == null) {
            Logger.Warning(g.name + " converted to interactable was null");
        }
        return interactable;
    }
}
