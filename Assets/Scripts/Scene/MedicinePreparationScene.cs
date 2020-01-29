using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MedicinePreparationScene : SceneScript {

    #region fields
    [SerializeField]
    public int autoPlayStrength;

    [Tooltip("Prefabs")]
    [SerializeField]
    private GameObject p_syringeCapBag, p_luerlock, p_needle, p_smallSyringe, p_bigSyringe, p_bottle;

    [Tooltip("Scene items")]
    [SerializeField]
    private Transform correctPositions;

    [SerializeField]
    private Interactable teleportDoorKnob;

    private bool played;
    public bool IsAutoPlaying { get; private set; }

    private Vector3 spawnPos = new Vector3(1000, 1000, 1000);

    public bool InSecondRoom { get; set; }
    #endregion

    protected override void Start() {
        base.Start();
        NullCheck.Check(p_syringeCapBag, p_luerlock, p_needle, p_smallSyringe, p_bigSyringe, p_bottle, correctPositions, teleportDoorKnob);
        PlayFirstRoom(2, autoPlayStrength);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            PlayFirstRoom(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            DebugTheShitOutOfProgressManager();
        }
    }

    private void DebugTheShitOutOfProgressManager() {

        Logger.Print("All tasks");
        foreach (var asd in G.Instance.Progress.GetAllTasks()) {
            Logger.Print(asd.ToString());
        }
        Logger.Print("Active:");
        foreach (var asd in G.Instance.Progress.CurrentPackage.activeTasks) {
            Logger.Print(asd.ToString());
        }
    }

    public void PlayFirstRoom(int points, int autoPlayeStrentgh = 0) {

        if (IsAutoPlaying || played || autoPlayStrength == 0) {
            return;
        }
        played = true;
        IsAutoPlaying = true;

        StartCoroutine(PlayCoroutine(points, autoPlayStrength));
    }

    private Vector3 SpawnPos {
        get {
            spawnPos += Vector3.one;
            return spawnPos;
        }
    }

    public override void Restart() {
        if (InSecondRoom) {
            Logger.Warning("Get first room points here");
            int points = 0;

            GameObject g = new GameObject();
            MedicinePreparationSceneRestarter r = g.AddComponent<MedicinePreparationSceneRestarter>();
            DontDestroyOnLoad(g);

            r.Points = points;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator PlayCoroutine(int points, int autoPlay) {

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

        Interactable capBag = ToIntr(g_syringeCapBag);
        Interactable luerlock = ToIntr(g_luerlock);
        Interactable needle = ToIntr(g_needle);
        Interactable bigSyringe = ToIntr(g_bigSyringe);
        Interactable bottle = ToIntr(g_bottle);

        Interactable[] smallSyringes = new Interactable[6];


        for (int i = 0; i < 6; i++) {
            smallSyringes[i] = ToIntr(g_smallSyringes[i]);
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

        // Set task CorrectItemsInThroughPutScore points here
        Logger.Warning("Set task CorrectItemsInThroughPutScore points here");

        yield return new WaitForSeconds(0.5f);
        hand.InteractWith(teleportDoorKnob);


        yield return null;
        IgnoreCollisions(all, false);
        yield return null;

        if (autoPlay == 1) {
            yield break;
        }

        IsAutoPlaying = false;

        Vector3 offSet = new Vector3(-0.4f, 0.5f, 0);
        Vector3 Offset() {
            offSet += new Vector3(0.1f, 0, 0);
            return offSet;
        }

        CabinetBase cabinet = GameObject.FindObjectOfType<CabinetBase>();

        capBag.transform.position = cabinet.transform.position + Offset();
        luerlock.transform.position = cabinet.transform.position + Offset();
        needle.transform.position = cabinet.transform.position + Offset();
        bigSyringe.transform.position = cabinet.transform.position + Offset();
        bottle.transform.position = cabinet.transform.position + Offset();

        foreach (var s in smallSyringes) {
            s.transform.position = cabinet.transform.position + Offset();
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
    private Interactable ToIntr(GameObject g) {
        return Interactable.GetInteractable(g.transform);
    }
}