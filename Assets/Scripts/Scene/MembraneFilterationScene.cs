using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

class MembraneFilterationScene : SceneScript {

    public enum AutoPlayStrength {
        None = 0,
        ItemsToPassThrough,
        WorkspaceRoom,
        ItemsToWorkspace,
        WriteItems,
        OpenAgarPlates,
        FillBottles,
        AssemblePump,
        PeptoneToFilter,
        MedicineToFilter,
        DisassemblePump,
        CutFilter,
        FilterHalvesToBottles,
        CloseAgarPlates,
        WriteSecondTime,
        Fingerprints
    }

    public AutoPlayStrength autoPlayStrength;
    public bool cleanEquipment = true;
    public List<GameObject> preperationRoomObjects;
    public GameObject automaticPipette, pipette, serologicalPipet1, serologicalPipet2,
        bottle0, bottle1, bottle2, bottle3,
        plate0, plate1, plate2, plate3,
        soycaseine, thioglycolate, peptoneWater,
        tweezers, scalpel,
        pumpFilter, pump,
        sterileBag,
        cleaningBottle, writingPen;
    public Transform cleaningPosition;
    public GameObject syringe, syringeCap;

    [Header("Scene items")]
    public Transform correctPositions;
    public Transform correctPositionsWorkspace;
    public Interactable teleportDoorKnob;
    public Interactable pipeConnectorButton;

    public static byte[] SavedScoreState;

    protected override void Start() {
        base.Start();
        PlayFirstRoom(autoPlayStrength);
    }

    public void SaveProgress(bool overwrite = false) {
        if (SavedScoreState != null || overwrite) {
            SavedScoreState = DataSerializer.Serializer(G.Instance.Progress.Calculator);
        }
    }

    public void PlayFirstRoom(AutoPlayStrength strength = AutoPlayStrength.None) {
        if (strength == 0) return;

        CoroutineUtils.StartThrowingCoroutine(this, PlayCoroutine(strength),
            exception => {
                if (exception != null)
                    Logger.Error(exception);
                Logger.Print("Autoplay finished");
            }
        );
    }

    private IEnumerator PlayCoroutine(AutoPlayStrength autoPlay) {
        // Create objects from prefabs and store in a list. They must be in the correct order here!
        List<GameObject> gameObjects = new List<GameObject>() {
            automaticPipette, pipette, serologicalPipet1, serologicalPipet2,
            bottle0, bottle1, bottle2, bottle3,
            plate0, plate1, plate2, plate3,
            soycaseine, thioglycolate, peptoneWater,
            tweezers, scalpel,
            pumpFilter, pump,
            sterileBag
        };
        List<Transform> transforms = preperationRoomObjects.Select(SelectTransform).ToList();
        yield return Wait();
        Hand hand = VRInput.Hands[0].Hand;

        // --- Set to correct positions in throughput cabinet ---

        for (int i = 0; i < transforms.Count; i++) {
            yield return Wait();
            if (cleanEquipment) {
                DropAt(transforms[i], cleaningPosition);
                yield return Wait();
                cleaningBottle.GetComponent<CleaningBottle>().Clean();
            }
            DropAt(transforms[i], correctPositions.GetChild(i).transform);
        }

        yield return Wait();
        if (autoPlay == AutoPlayStrength.ItemsToPassThrough) yield break;
        yield return Wait();

        // --- Go to workspace ---

        hand.InteractWith(teleportDoorKnob);
        yield return Wait();
        hand.Uninteract();
        if (autoPlay == AutoPlayStrength.WorkspaceRoom) yield break;
        yield return Wait();

        // --- Set to correct positions in workspace ---

        for (int i = 0; i < gameObjects.Count; i++) {
            yield return Wait();
            if (cleanEquipment) {
                DropAt(gameObjects[i].transform, cleaningPosition);
                yield return Wait();
                cleaningBottle.GetComponent<CleaningBottle>().Clean();
            }
            DropAt(gameObjects[i].transform, correctPositionsWorkspace.GetChild(i).transform);
        }

        if (autoPlay == AutoPlayStrength.ItemsToWorkspace) yield break;
        yield return Wait();

        // --- Add markings to correct items ---

        WritingPen pen = ToInteractable(writingPen) as WritingPen;
        AgarPlateLid soycaseinePlateLid1 = plate0.GetComponentInChildren<AgarPlateLid>();
        AgarPlateLid soycaseinePlateLid2 = plate1.GetComponentInChildren<AgarPlateLid>();
        AgarPlateLid soycaseinePlateLid3 = plate2.GetComponentInChildren<AgarPlateLid>();
        AgarPlateLid thioglycolatePlateLid = plate3.GetComponentInChildren<AgarPlateLid>();
        Bottle bottleSoycaseine1 = bottle2.GetComponentInChildren<Bottle>();
        Bottle bottleSoycaseine2 = bottle3.GetComponentInChildren<Bottle>();
        Bottle bottleThioglycolate1 = bottle0.GetComponentInChildren<Bottle>();
        Bottle bottleThioglycolate2 = bottle1.GetComponentInChildren<Bottle>();

        // Marking plates
        var writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, "Oma nimi"},
            {WritingType.Date, "pvm"},
            {WritingType.Time, "klonaika"},
        };
        pen.SubmitWriting(soycaseinePlateLid1.GetComponent<Writable>(), soycaseinePlateLid1.gameObject, writing);

        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, "Oma nimi"},
            {WritingType.Date, "pvm"},
            {WritingType.Time, "klonaika"},
        };
        pen.SubmitWriting(thioglycolatePlateLid.GetComponent<Writable>(), thioglycolatePlateLid.gameObject, writing);

        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, "Oma nimi"},
            {WritingType.Date, "pvm"},
            {WritingType.RightHand, "oikea"},
            {WritingType.Time, "klonaika"}
        };
        pen.SubmitWriting(soycaseinePlateLid2.GetComponent<Writable>(), soycaseinePlateLid2.gameObject, writing);

        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, "Oma nimi"},
            {WritingType.Date, "pvm"},
            {WritingType.LeftHand, "vasen"},
            {WritingType.Time, "klonaika"}
        };
        pen.SubmitWriting(soycaseinePlateLid3.GetComponent<Writable>(), soycaseinePlateLid3.gameObject, writing);

        // Marking bottles
        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, "Oma nimi"},
            {WritingType.Date, "pvm"},
            {WritingType.SoyCaseine, "soijakaseiini"},
        };
        pen.SubmitWriting(bottleSoycaseine1.GetComponent<Writable>(), bottleSoycaseine1.gameObject, writing);

        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, "Oma nimi"},
            {WritingType.Date, "pvm"},
            {WritingType.SoyCaseine, "soijakaseiini"},
        };
        pen.SubmitWriting(bottleSoycaseine2.GetComponent<Writable>(), bottleSoycaseine2.gameObject, writing);

        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, "Oma nimi"},
            {WritingType.Date, "pvm"},
            {WritingType.Tioglygolate, "tioglykolaatti"},
        };
        pen.SubmitWriting(bottleThioglycolate1.GetComponent<Writable>(), bottleThioglycolate1.gameObject, writing);

        writing = new Dictionary<WritingType, string>() {
            {WritingType.Name, "Oma nimi"},
            {WritingType.Date, "pvm"},
            {WritingType.Tioglygolate, "tioglykolaatti"},
        };
        pen.SubmitWriting(bottleThioglycolate2.GetComponent<Writable>(), bottleThioglycolate2.gameObject, writing);

        if (autoPlay == AutoPlayStrength.WriteItems) yield break;
        yield return Wait();

        // --- Open agar plate lids ---

        soycaseinePlateLid1.ReleaseItem();
        thioglycolatePlateLid.ReleaseItem();
        yield return Wait();
        DropAt(soycaseinePlateLid1.transform, soycaseinePlateLid1.transform.position + new Vector3(0.06f, 0.1f, 0));
        DropAt(thioglycolatePlateLid.transform, thioglycolatePlateLid.transform.position + new Vector3(0.06f, 0.1f, 0));
        yield return Wait();
        soycaseinePlateLid1.transform.Rotate(new Vector3(180, 0, 0));
        thioglycolatePlateLid.transform.Rotate(new Vector3(180, 0, 0));
        if (autoPlay == AutoPlayStrength.OpenAgarPlates) yield break;
        yield return Wait();

        // --- Fill all bottles with correct liquids ---

        Bottle soycaseineBottle = soycaseine.GetComponentInChildren<Bottle>();
        Bottle thioglycolateBottle = thioglycolate.GetComponentInChildren<Bottle>();
        BigPipette bigPipette = ToInteractable(automaticPipette) as BigPipette;

        // Unbottle everything
        new List<GameObject> { bottle0, bottle1, bottle2, bottle3, soycaseine, thioglycolate, peptoneWater }.ForEach(g => {
            var cap = g.transform.GetComponentInChildren<BottleCap>();
            cap.Connector.Connection.Remove();
            DropAt(cap.transform, cap.transform.position + Vector3.forward * 0.2f);
        });

        serologicalPipet1.GetComponent<Cover>().OpenCover(hand);
        yield return Wait();
        DropAt(bigPipette.transform, new Vector3(-18.57f, 1.3f, 2.0f));
        yield return Wait();
        GameObject pipetteContainer = GameObject.Find("PipetteHead50ml");
        DropAt(pipetteContainer.transform, bigPipette.transform.position + Vector3.down * 0.07f);
        yield return Wait();

        // Fill bottles
        var list = new List<(Bottle, Bottle, BigPipette)>() {
            (thioglycolateBottle, bottleThioglycolate1, bigPipette),
            (thioglycolateBottle, bottleThioglycolate2, bigPipette),
            (soycaseineBottle, bottleSoycaseine1, bigPipette),
            (soycaseineBottle, bottleSoycaseine2, bigPipette),
        };
        foreach (var stuff in list) {
            var (bigBottle, bottle, pipette) = stuff;
            FreezeAt(pipette.transform, bigBottle.transform.position + Vector3.up * 0.34f);
            yield return Wait();
            hand.transform.position = pipette.transform.position;
            yield return Wait();
            hand.InteractWith(pipette);
            for (int i = 0; i < 4; i++) {
                pipette.TakeMedicine();
                yield return Wait();
            }
            FreezeAt(pipette.transform, bottle.transform.position + Vector3.up * 0.25f);
            for (int i = 0; i < 4; i++) {
                pipette.SendMedicine();
                yield return Wait();
            }
            FreezeAt(pipette.transform, new Vector3(-18.57f, 1.3f, 2.0f));
            yield return Wait();
            hand.Uninteract();
            yield return Wait();
        };

        bigPipette.GetComponent<Rigidbody>().isKinematic = false;
        DropAt(bigPipette.transform, bigPipette.transform.position + Vector3.left * 0.5f);
        hand.Uninteract();
        yield return Wait();
        if (autoPlay == AutoPlayStrength.FillBottles) yield break;
        yield return Wait();

        // --- Assemble pump ---

        pumpFilter.GetComponent<Cover>().OpenCover(hand);
        yield return Wait();
        DropAt(pumpFilter.transform, pump.transform.position + Vector3.up * 0.14f);
        hand.InteractWith(pipeConnectorButton);
        yield return Wait();
        hand.Uninteract();
        yield return Wait();
        if (autoPlay == AutoPlayStrength.AssemblePump) yield break;
        yield return Wait();

        // --- Add peptone water to filter ---

        Pipette smallPipette = ToInteractable(pipette) as Pipette;
        GameObject lidObject = pumpFilter.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Lid")?.gameObject;
        FilterPart lid = ToInteractable(lidObject) as FilterPart;
        StartCoroutine(lid.WaitForDistance(hand));
        yield return Wait();
        lid.SnapDistance = -1.0f;
        yield return Wait();
        hand.Uninteract();
        DropAt(lid.transform, lid.transform.position + Vector3.forward * 0.35f);
        lid.SnapDistance = 0.03f;
        yield return Wait();
        FreezeAt(smallPipette.transform, peptoneWater.transform.position + Vector3.up * 0.02f);
        smallPipette.transform.eulerAngles = new Vector3(-180, 0, 0);
        yield return Wait();
        hand.transform.position = smallPipette.transform.position;
        yield return Wait();
        hand.InteractWith(smallPipette);
        smallPipette.TakeMedicine();
        yield return Wait();
        smallPipette.TakeMedicine();
        yield return Wait();
        smallPipette.TakeMedicine();
        yield return Wait();
        FreezeAt(smallPipette.transform, pump.transform.position + Vector3.up * 0.5f);
        yield return Wait();
        FreezeAt(smallPipette.transform, pump.transform.position + Vector3.up * 0.24f);
        smallPipette.transform.eulerAngles = new Vector3(-180, 0, 0);
        hand.transform.position = smallPipette.transform.position;
        yield return Wait();
        smallPipette.SendMedicine();
        yield return Wait();
        smallPipette.SendMedicine();
        yield return Wait();
        FreezeAt(smallPipette.transform, pump.transform.position + Vector3.up * 0.5f);
        yield return Wait();
        hand.Uninteract();
        yield return Wait();
        smallPipette.GetComponent<Rigidbody>().isKinematic = false;
        DropAt(smallPipette.transform, smallPipette.transform.position + Vector3.left * 0.5f);
        yield return Wait();
        GameObject pumpButton = pump.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Push")?.gameObject;
        hand.transform.position = pumpButton.transform.position;
        yield return Wait();
        pumpButton.GetComponent<FilteringButton>().RunPump();
        yield return Wait();
        if (autoPlay == AutoPlayStrength.PeptoneToFilter) yield break;
        yield return Wait();

        // --- MedicineToFilter ---

        sterileBag.GetComponent<SterileBag2>().ReleaseSyringe();
        yield return Wait(2.0f);
        syringeCap.active = false;
        yield return Wait();
        FreezeAt(syringe.transform, pump.transform.position + Vector3.up * 0.24f);
        yield return Wait();
        SyringeNew syringeN = ToInteractable(syringe) as SyringeNew;
        yield return Wait();
        syringeN.SendMedicine(150);
        yield return Wait();
        pumpButton.GetComponent<FilteringButton>().RunPump();
        yield return Wait();
        if (autoPlay == AutoPlayStrength.MedicineToFilter) yield break;
        yield return Wait();

        // --- DisassemblePump ---

        yield return Wait(1.0f);
        GameObject tankObject = pumpFilter.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Tank")?.gameObject;
        FilterPart tank = ToInteractable(tankObject) as FilterPart;
        yield return Wait();
        StartCoroutine(tank.WaitForDistance(hand));
        yield return Wait();
        tank.SnapDistance = -1.0f;
        yield return Wait();
        hand.Uninteract();
        yield return Wait();
        DropAt(tank.transform, tank.transform.position + Vector3.forward * 0.3f);
        yield return Wait();
        tank.SnapDistance = 0.03f;
        yield return Wait();
        GameObject filterBaseObject = pumpFilter.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "FilterInCover")?.gameObject;
        FilterInCover filterBase = ToInteractable(filterBaseObject) as FilterInCover;
        yield return Wait();
        StartCoroutine(filterBase.WaitForDistance(hand));
        yield return Wait();
        filterBase.SnapDistance = -1.0f;
        yield return Wait();
        hand.Uninteract();
        yield return Wait();
        DropAt(filterBase.transform, pump.transform.position + Vector3.forward * 0.2f);
        yield return Wait();
        filterBase.SnapDistance = 0.03f;
        yield return Wait();
        if (autoPlay == AutoPlayStrength.DisassemblePump) yield break;
        yield return Wait();

        // --- CutFilter ---

        scalpel.GetComponent<Cover>().OpenCover(hand);
        yield return Wait();
        DropAt(scalpel.transform, filterBase.transform.position + Vector3.up * 0.06f);
        scalpel.transform.eulerAngles = new Vector3(0, 0, -90);
        yield return Wait();
        DropAt(scalpel.transform, scalpel.transform.position + Vector3.forward * 0.1f);
        yield return Wait();
        if (autoPlay == AutoPlayStrength.CutFilter) yield break;
        yield return Wait();

        // --- FilterHalvesToBottles ---

        tweezers.GetComponent<Cover>().OpenCover(hand);
        yield return Wait();
        GameObject leftHalf = GameObject.Find("FilterHalfL");
        GameObject rightHalf = GameObject.Find("FilterHalfR");
        DropAt(leftHalf.transform, bottle1.transform.position + Vector3.up * 0.1f);
        yield return Wait();
        leftHalf.transform.eulerAngles = new Vector3(90, 0, 0);
        yield return Wait();
        DropAt(rightHalf.transform, bottle3.transform.position + Vector3.up * 0.1f);
        yield return Wait();
        rightHalf.transform.eulerAngles = new Vector3(90, 0, 0);
        yield return Wait();
        if (autoPlay == AutoPlayStrength.FilterHalvesToBottles) yield break;
        yield return Wait();

        // --- CloseAgarPlates ---

        Transform soyBottom = plate0.transform.GetChild(1);
        soyBottom.parent = soycaseinePlateLid1.transform;
        soyBottom.localPosition = Vector3.zero;
        soycaseinePlateLid1.Connector.ConnectItem(soyBottom.gameObject.GetComponent<Interactable>());
        yield return Wait();
        Transform thioBottom = plate3.transform.GetChild(1);
        thioBottom.parent = thioglycolatePlateLid.transform;
        thioBottom.localPosition = Vector3.zero;
        thioglycolatePlateLid.Connector.ConnectItem(thioBottom.gameObject.GetComponent<Interactable>());
        yield return Wait();
        if (autoPlay == AutoPlayStrength.CloseAgarPlates) yield break;
        yield return Wait();

        // --- WriteSecondTime ---

        writing = new Dictionary<WritingType, string>() {
            {WritingType.SecondTime, "lopetusaika"},
        };
        pen.SubmitWriting(soycaseinePlateLid1.GetComponent<Writable>(), soycaseinePlateLid1.gameObject, writing);
        writing = new Dictionary<WritingType, string>() {
            {WritingType.SecondTime, "lopetusaika"},
        };
        pen.SubmitWriting(thioglycolatePlateLid.GetComponent<Writable>(), thioglycolatePlateLid.gameObject, writing);
        if (autoPlay == AutoPlayStrength.WriteSecondTime) yield break;
        yield return Wait();

        // --- Fingerprints ---

        soycaseinePlateLid2.ReleaseItem();
        soycaseinePlateLid3.ReleaseItem();
        yield return Wait();
        DropAt(soycaseinePlateLid2.transform, soycaseinePlateLid2.transform.position + new Vector3(0.06f, 0.1f, 0));
        DropAt(soycaseinePlateLid3.transform, soycaseinePlateLid3.transform.position + new Vector3(0.06f, 0.1f, 0));
        yield return Wait();
        soycaseinePlateLid2.transform.Rotate(new Vector3(180, 0, 0));
        soycaseinePlateLid3.transform.Rotate(new Vector3(180, 0, 0));
        Agar agar1 = plate2.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Agar")?.gameObject.GetComponent<Agar>();
        Agar agar2 = plate3.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Agar")?.gameObject.GetComponent<Agar>();
        agar1.Interact(hand);
        agar2.Interact(VRInput.Hands[1].Hand);
        yield return Wait(4.5f);
        agar1.Uninteract(hand);
        agar2.Uninteract(VRInput.Hands[1].Hand);
        agar1.Interact(hand);
        agar2.Interact(VRInput.Hands[1].Hand);
        yield return Wait(4.5f);
        agar1.Uninteract(hand);
        agar2.Uninteract(VRInput.Hands[1].Hand);
        if (autoPlay == AutoPlayStrength.Fingerprints) yield break;
        yield return Wait();
        yield break;
    }

    private Transform SelectTransform(GameObject gameObject) {
        if (gameObject.GetComponent<Rigidbody>() != null) {
            return gameObject.transform;
        } else {
            return gameObject.GetComponentInChildren<Rigidbody>().transform;
        }
    }

    private void DropAt(Transform theObject, Transform position) {
        DropAt(theObject, position.position);
    }

    private void DropAt(Transform theObject, Vector3 position) {
        theObject.position = position;
        theObject.eulerAngles = Vector3.up;
    }

    private void FreezeAt(Transform item, Vector3 position) {
        item.position = position;
        item.eulerAngles = Vector3.up;
        Rigidbody rigidBody = item.GetComponent<Rigidbody>();
        if (rigidBody == null) {
            rigidBody = item.GetComponentInChildren<Rigidbody>();
        }
        rigidBody.isKinematic = true;
    }

    private WaitForSeconds Wait() {
        return Wait(0.1f);
    }

    private WaitForSeconds Wait(float seconds) {
        return new WaitForSeconds(seconds);
    }

    private void AllowCollisionsBetween(List<Transform> items, bool allow) {
        for (int i = 0; i < items.Count; i++) {
            for (int j = 0; j < items.Count; j++) {
                if (i != j) {
                    CollisionIgnore.IgnoreCollisions(items[i], items[j], !allow);
                }
            }
        }
    }

    private Interactable ToInteractable(GameObject g) {
        var interactable = Interactable.GetInteractable(g.transform);
        if (interactable == null) {
            Logger.Warning(g.name + " converted to interactable was null");
        }
        return interactable;
    }
}
