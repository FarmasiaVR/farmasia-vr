using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MedicinePreparationScene : SceneScript {

    #region fields
    public enum AutoPlayStrength {
        None = 0,
        ItemsToPassThrough,
        WorkspaceRoom,
        ItemsToLaminarCabinet,
        CheckCabinetItems,
        DisinfectBottle,
        TakeMedicine,
        BigSyringeToLuerlock,
        MedicineToSmallSyringes,
        AddSyringeCaps,
        SyringesToSterileBag,
        CloseSterileBag,
        Cleanup,
    }

    [SerializeField]
    public AutoPlayStrength autoPlayStrength;

    [Tooltip("Prefabs")]
    [SerializeField]
    private GameObject p_syringeCapBag, p_luerlock, p_needle, p_smallSyringe, p_bigSyringe, p_bottle, p_sterileCloth;

    [Tooltip("Scene items")]
    [SerializeField]
    private Transform correctPositions;

    [SerializeField]
    private Transform correctPositionLaminarCabinet;

    [SerializeField]
    private Interactable teleportDoorKnob, laminarCabinetCheckButton;

    [SerializeField]
    private SterileBag sterileBag;

    [SerializeField]
    private TrashBin regularTrash, sharpTrash;

    private bool played;
    public bool IsAutoPlaying { get; private set; }

    private Vector3 spawnPos = new Vector3(1000, 1000, 1000);

    public bool InSecondRoom { get; set; }

    public bool NeedleUsed { get; set; }

    public bool Restarted { get; set; }

    public static byte[] SavedScoreState;
    #endregion

    protected override void Start() {
        base.Start();
        //NullCheck.Check(p_syringeCapBag, p_luerlock, p_needle, p_smallSyringe, p_bigSyringe, p_bottle, p_sterileCloth);
        //NullCheck.Check(correctPositions, teleportDoorKnob, laminarCabinetCheckButton, sterileBag, regularTrash, sharpTrash);
        PlayFirstRoom(autoPlayStrength);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            DebugTasks();
        }
    }

    public void SaveProgress(bool overwrite = false) {
        if (SavedScoreState != null || overwrite) {
            SavedScoreState = DataSerializer.Serializer(G.Instance.Progress.Calculator);
        }
    }

    private void DebugTasks() {

        Logger.Print("All tasks");
        foreach (var asd in G.Instance.Progress.GetAllTasks()) {
            Logger.Print(asd.ToString());
        }
        Logger.Print("Active:");
        foreach (var asd in G.Instance.Progress.CurrentPackage.activeTasks) {
            Logger.Print(asd.ToString());
        }
    }

    public void PlayFirstRoom(AutoPlayStrength strength = AutoPlayStrength.None) {

        if (IsAutoPlaying || played || strength == 0) {
            return;
        }
        played = true;
        IsAutoPlaying = true;

        StartCoroutine(PlayCoroutine(strength));
    }

    private Vector3 SpawnPos {
        get {
            spawnPos += Vector3.one;
            return spawnPos;
        }
    }

    public override void Restart() {
        if (InSecondRoom) {
            GameObject g = new GameObject();
            MedicinePreparationSceneRestarter r = g.AddComponent<MedicinePreparationSceneRestarter>();
            DontDestroyOnLoad(g);

            r.ScoreState = SavedScoreState;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator PlayCoroutine(AutoPlayStrength autoPlay) {

        yield return null;
        yield return null;
        yield return null;

        // Create objects

        GameObject g_syringeCapBag = Instantiate(p_syringeCapBag, SpawnPos, Quaternion.Euler(Vector3.zero));
        GameObject g_luerlock = Instantiate(p_luerlock, SpawnPos, Quaternion.Euler(Vector3.zero));
        GameObject g_needle = Instantiate(p_needle, SpawnPos, Quaternion.Euler(Vector3.zero));
        GameObject g_bigSyringe = Instantiate(p_bigSyringe, SpawnPos, Quaternion.Euler(Vector3.zero));
        GameObject g_bottle = Instantiate(p_bottle, SpawnPos, Quaternion.Euler(Vector3.zero));

        GameObject[] g_smallSyringes = new GameObject[6];

        Transform[] all = new Transform[11];
        all[0] = g_syringeCapBag.transform;
        all[1] = g_luerlock.transform;
        all[2] = g_needle.transform;
        all[3] = g_bigSyringe.transform;
        all[4] = g_bottle.transform;


        for (int i = 0; i < 6; i++) {
            g_smallSyringes[i] = Instantiate(p_smallSyringe, SpawnPos, Quaternion.Euler(Vector3.zero));
            all[5 + i] = g_smallSyringes[i].transform;
        }

        IgnoreCollisions(all, true);

        yield return null;
        yield return null;
        yield return null;

        Interactable capBag = ToInteractable(g_syringeCapBag);
        LuerlockAdapter luerlock = ToInteractable(g_luerlock) as LuerlockAdapter;
        Needle needle = ToInteractable(g_needle) as Needle;
        Syringe bigSyringe = ToInteractable(g_bigSyringe) as Syringe;
        MedicineBottle bottle = ToInteractable(g_bottle) as MedicineBottle;

        Syringe[] smallSyringes = new SmallSyringe[6];


        for (int i = 0; i < 6; i++) {
            smallSyringes[i] = ToInteractable(g_smallSyringes[i]) as Syringe;
            NullCheck.Check(smallSyringes[i]);
        }

        NullCheck.Check(capBag, luerlock, needle, bigSyringe, bottle);
        // Select tools task

        Hand hand = VRInput.Hands[0].Hand;

        yield return null;
        hand.InteractWith(capBag);
        yield return null;
        hand.Uninteract();
        yield return null;
        hand.InteractWith(luerlock);
        yield return null;
        hand.Uninteract();
        yield return null;
        hand.InteractWith(needle);
        yield return null;
        hand.Uninteract();
        yield return null;
        hand.InteractWith(bigSyringe);
        yield return null;
        hand.Uninteract();
        yield return null;
        hand.InteractWith(smallSyringes[0]);
        yield return null;
        hand.Uninteract();
        yield return null;

        // Select bottle task
        hand.InteractWith(bottle);
        yield return null;
        hand.Uninteract();
        yield return null;

        // Correct items in throughput task

        yield return null;

        capBag.transform.position = correctPositions.GetChild(0).position;
        capBag.transform.up = correctPositions.right;
        capBag.Rigidbody.velocity = Vector3.zero;
        yield return null;
        luerlock.transform.position = correctPositions.GetChild(1).position;
        luerlock.transform.up = correctPositions.right;
        luerlock.Rigidbody.velocity = Vector3.zero;
        yield return null;
        needle.transform.position = correctPositions.GetChild(2).position;
        needle.transform.up = correctPositions.right;
        needle.Rigidbody.velocity = Vector3.zero;
        yield return null;
        bigSyringe.transform.position = correctPositions.GetChild(3).position;
        bigSyringe.transform.up = correctPositions.right;
        bigSyringe.Rigidbody.velocity = Vector3.zero;
        yield return null;
        bottle.transform.position = correctPositions.GetChild(4).position;
        bottle.transform.up = correctPositions.right;
        bottle.Rigidbody.velocity = Vector3.zero;

        for (int i = 0; i < 6; i++) {
            smallSyringes[i].transform.position = correctPositions.GetChild(5 + i).position;
            smallSyringes[i].transform.up = correctPositions.right;
            smallSyringes[i].Rigidbody.velocity = Vector3.zero;
            yield return null;
        }

        if (autoPlay == AutoPlayStrength.ItemsToPassThrough) {
            IgnoreCollisions(all, false);
            yield break;
        }

        yield return Wait;
        hand.InteractWith(teleportDoorKnob);


        yield return null;
        IgnoreCollisions(all, false);
        yield return null;

        IsAutoPlaying = false;

        if (autoPlay == AutoPlayStrength.WorkspaceRoom) {
            yield break;
        }

        yield return new WaitForSeconds(1);

        capBag.transform.position = correctPositionLaminarCabinet.GetChild(0).position;
        capBag.transform.up = correctPositions.right;
        capBag.Rigidbody.velocity = Vector3.zero;
        yield return null;
        luerlock.transform.position = correctPositionLaminarCabinet.GetChild(1).position;
        luerlock.transform.up = correctPositions.right;
        luerlock.Rigidbody.velocity = Vector3.zero;
        yield return null;
        needle.transform.position = correctPositionLaminarCabinet.GetChild(2).position;
        needle.transform.up = correctPositions.right;
        needle.Rigidbody.velocity = Vector3.zero;
        yield return null;
        bigSyringe.transform.position = correctPositionLaminarCabinet.GetChild(3).position;
        bigSyringe.transform.up = correctPositions.right;
        bigSyringe.Rigidbody.velocity = Vector3.zero;
        yield return null;
        bottle.transform.position = correctPositionLaminarCabinet.GetChild(4).position;
        bottle.transform.up = correctPositions.right;
        bottle.Rigidbody.velocity = Vector3.zero;

        for (int i = 0; i < 6; i++) {
            smallSyringes[i].transform.position = correctPositionLaminarCabinet.GetChild(5 + i).position;
            smallSyringes[i].transform.up = correctPositions.right;
            smallSyringes[i].Rigidbody.velocity = Vector3.zero;
            yield return null;
        }

        if (autoPlay == AutoPlayStrength.ItemsToLaminarCabinet) {
            yield break;
        }

        yield return Wait;
        hand.InteractWith(laminarCabinetCheckButton);

        if (autoPlay == AutoPlayStrength.CheckCabinetItems) {
            yield break;
        }

        yield return Wait;

        GameObject g_sterileCloth = Instantiate(p_sterileCloth);
        g_sterileCloth.transform.position = new Vector3(100, 100, 100);
        g_sterileCloth.name = "STERILE CLOTH AUTOPLAY";
        yield return null;
        Interactable sterileCloth = Interactable.GetInteractable(g_sterileCloth.transform);

        sterileCloth.transform.position = bottle.transform.position;

        yield return Wait;

        sterileCloth.transform.position = regularTrash.transform.position;
        yield return null;
        yield return null;


        if (autoPlay == AutoPlayStrength.DisinfectBottle) {
            yield break;
        }

        needle.Connector.ConnectItem(bigSyringe);
        yield return null;
        yield return null;

        needle.transform.position = bottle.transform.position;
        bottle.Container.TransferTo(bigSyringe.Container, 900);
        yield return null;
        yield return null;

        needle.transform.position = bottle.transform.position + new Vector3(0, 0.25f, 0);

        yield return null;
        yield return null;

        needle.Connector.Connection.Remove();
        bigSyringe.transform.position = needle.transform.position + new Vector3(0.05f, 0, 0);

        yield return null;
        yield return null;

        if (autoPlay == AutoPlayStrength.TakeMedicine) {
            yield break;
        }

        luerlock.LeftConnector.ConnectItem(bigSyringe);
        Events.FireEvent(EventType.AttachLuerlock, CallbackData.Object(bigSyringe.gameObject));
        Events.FireEvent(EventType.SyringeToLuerlock, CallbackData.Object(bigSyringe.gameObject));

        yield return null;
        yield return null;

        if (autoPlay == AutoPlayStrength.BigSyringeToLuerlock) {
            yield break;
        }

        foreach (Syringe smallSyringe in smallSyringes) {
            luerlock.RightConnector.ConnectItem(smallSyringe);
            Events.FireEvent(EventType.AttachLuerlock, CallbackData.Object(smallSyringe.gameObject));
            Events.FireEvent(EventType.SyringeToLuerlock, CallbackData.Object(smallSyringe.gameObject));
            yield return null;
            yield return null;

            bigSyringe.Container.TransferTo(smallSyringe.Container, 150);

            yield return null;
            yield return null;

            luerlock.RightConnector.Connection.Remove();
            smallSyringe.transform.position = luerlock.transform.position + new Vector3(0.05f, 0, 0);
            yield return null;
            yield return null;
        }

        luerlock.LeftConnector.Connection.Remove();
        bigSyringe.transform.position = luerlock.transform.position + new Vector3(-0.05f, 0, 0);
        yield return null;
        yield return null;

        if (autoPlay == AutoPlayStrength.MedicineToSmallSyringes) {
            yield break;
        }

        foreach (Syringe smallSyringe in smallSyringes) {
            SyringeCap.AddSyringeCap(smallSyringe);
        }

        yield return null;
        yield return null;

        if (autoPlay == AutoPlayStrength.AddSyringeCaps) {
            yield break;
        }

        foreach (Syringe smallSyringe in smallSyringes) {
            smallSyringe.transform.position = sterileBag.transform.position;
            yield return null;
            yield return null;
        }

        if (autoPlay == AutoPlayStrength.SyringesToSterileBag) {
            yield break;
        }

        sterileBag.CloseSterileBagFinal();

        yield return null;
        yield return null;

        if (autoPlay == AutoPlayStrength.CloseSterileBag) {
            yield break;
        }

        needle.transform.position = sharpTrash.transform.position;
        yield return null;
        yield return null;

        luerlock.transform.position = regularTrash.transform.position;
        yield return null;
        yield return null;

        bigSyringe.transform.position = regularTrash.transform.position;
        yield return null;
        yield return null;

        if (autoPlay == AutoPlayStrength.Cleanup) {
            yield break;
        }
    }

    private WaitForSeconds Wait {
        get {
            return new WaitForSeconds(0.25f);
        }
    }

    private void IgnoreCollisions(Transform[] items, bool ignore) {
        for (int i = 0; i < items.Length; i++) {
            for (int j = 0; j < items.Length; j++) {
                if (i != j) {
                    CollisionIgnore.IgnoreCollisions(items[i], items[j], ignore);
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