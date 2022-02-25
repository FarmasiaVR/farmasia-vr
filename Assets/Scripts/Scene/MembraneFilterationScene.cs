using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


class MembraneFilterationScene : SceneScript {

    public enum AutoPlayStrength {
        None = 0,
        ItemsToPassThrough,
        WorkspaceRoom,
        ItemsToWorkspace,
        PreparePump,
    }

    [SerializeField]
    public AutoPlayStrength autoPlayStrength;

    [Tooltip("Prefabs")]
    [SerializeField]
    private GameObject p_pipette, p_tweezers, p_scalpel, p_soyCaseinePlate, p_sabouradDextrosiPlate, p_bottle100ml, 
        p_soyCaseineBottle, p_peptonWaterBottle, p_tioglykolateBottle, p_pump, p_pump_filter, p_filledSterileBag, p_writingPen;

    [Tooltip("Scene items")]
    [SerializeField]
    private Transform correctPositions;

    [SerializeField]
    private Transform correctPositionsWorkspace;

    [SerializeField]
    private Interactable teleportDoorKnob;

    private bool played;
    public bool IsAutoPlaying { get; private set; }

    private Vector3 spawnPos = new Vector3(1000, 1000, 1000);

    protected override void Start() {
        base.Start();
        PlayFirstRoom(autoPlayStrength);
    }

    public void PlayFirstRoom(AutoPlayStrength strength = AutoPlayStrength.None) {

        if (IsAutoPlaying || played || strength == 0) {
            return;
        }
        played = true;
        IsAutoPlaying = true;

        CoroutineUtils.StartThrowingCoroutine(
            this,
            PlayCoroutine(strength),
            e => {
                if (e != null)
                    Logger.Error(e);
                Logger.Print("Autoplay finished");
            }
        );
    }

    private IEnumerator PlayCoroutine(AutoPlayStrength autoPlay) {
        
        // Create objects from prefabs and store in a list. They must be in the correct order here!
        List<GameObject> gameObjects = new List<GameObject>() {
            p_pipette,
            p_scalpel,
            p_tweezers,

            p_soyCaseinePlate,
            p_soyCaseinePlate,
            p_soyCaseinePlate,
            p_sabouradDextrosiPlate,

            p_bottle100ml,
            p_bottle100ml,
            p_bottle100ml,
            p_bottle100ml,

            p_soyCaseineBottle,
            p_peptonWaterBottle,
            p_tioglykolateBottle,

            p_pump, // 14
            p_pump_filter, // 15

            p_filledSterileBag, // 16

            p_writingPen, // 17

        }.Select(InstantiateObject).ToList();

        List<Transform> transforms = gameObjects.Select(go => go.transform).ToList();

        yield return Wait();

        //AllowCollisionsBetween(all, false);

        yield return Wait();

        Hand hand = VRInput.Hands[0].Hand;

        // --- Set to correct positions in throughput cabinet ---

        for (int i = 0; i < transforms.Count - 1; i++) { // -1 because no pen
            yield return Wait();
            DropAt(transforms[i], correctPositions.GetChild(i).transform);
        }

        yield return Wait();

        AllowCollisionsBetween(transforms, true);

        if (autoPlay == AutoPlayStrength.ItemsToPassThrough) {
            yield break;
        }

        yield return Wait();

        // --- Go to workspace ---

        hand.InteractWith(teleportDoorKnob);

        yield return Wait();

        hand.Uninteract();

        if (autoPlay == AutoPlayStrength.WorkspaceRoom) {
            yield break;
        }

        yield return Wait();

        // --- Set to correct positions in workspace ---

        for (int i = 0; i < transforms.Count; i++) {
            yield return Wait();
            DropAt(transforms[i], correctPositionsWorkspace.GetChild(i).transform);
        }

        if (autoPlay == AutoPlayStrength.ItemsToWorkspace) {
            yield break;
        }

        yield return Wait();

        // --- Try to connect pump filter ---

        Pump pump = ToInteractable(gameObjects[14]) as Pump;
        PumpFilter filter = ToInteractable(gameObjects[15]) as PumpFilter;

        yield return Wait();

        DropAt(filter.transform, pump.transform.position + Vector3.up * 0.12f);

        yield return Wait();

        hand.InteractWith(filter);

        yield return Wait();

        hand.transform.position -= Vector3.up * 0.04f;

        yield return Wait();

        hand.Uninteract();

        yield return Wait();

        if (filter.Connector.AttachedInteractable == null) {
            filter.Connector.ConnectItem(pump);
            Logger.Print("Autoplay forced pump filter connection");
        }

        // Write
        WritingPen pen = ToInteractable(gameObjects[17]) as WritingPen;
        Pipette pipette = ToInteractable(gameObjects[0]) as Pipette;
        MedicineBottle bottleT1 = ToInteractable(gameObjects[7]) as MedicineBottle;
        MedicineBottle bottleT2 = ToInteractable(gameObjects[8]) as MedicineBottle;
        MedicineBottle bottleS1 = ToInteractable(gameObjects[9]) as MedicineBottle;
        MedicineBottle bottleS2 = ToInteractable(gameObjects[10]) as MedicineBottle;
        MedicineBottle soycaseine = ToInteractable(gameObjects[11]) as MedicineBottle;
        MedicineBottle tioglygolate = ToInteractable(gameObjects[13]) as MedicineBottle;

        // Fill bottles
        // tioglygolate 1
        yield return Wait();
        DropAt(pipette.transform, tioglygolate.transform.position + Vector3.up * 0.12f);
        pipette.transform.eulerAngles = new Vector3(-180,0,0);
        yield return Wait();
        hand.transform.position = pipette.transform.position;
        yield return Wait();
        hand.InteractWith(pipette);
        yield return Wait();
        hand.transform.eulerAngles = Vector3.down;
        pipette.TakeMedicine();
        yield return Wait();
        hand.Uninteract();
        yield return Wait();
        DropAt(pipette.transform, bottleT1.transform.position + Vector3.up * 0.05f);
        pipette.transform.eulerAngles = new Vector3(-180,0,0);
        yield return Wait();
        hand.InteractWith(pipette);
        yield return Wait();
        pipette.SendMedicine();
        hand.Uninteract();

        // tioglygolate 1
        yield return Wait(0.5f);
        tioglygolate.transform.eulerAngles *= 0f;
        DropAt(pipette.transform, tioglygolate.transform.position + Vector3.up * 0.2f);
        pipette.transform.eulerAngles = new Vector3(-180,0,0);
        hand.transform.position = pipette.transform.position;
        hand.transform.eulerAngles = Vector3.down;
        hand.InteractWith(pipette);
        yield return Wait(0.5f);
        pipette.TakeMedicine();
        yield return Wait();
        hand.Uninteract();
        yield return Wait();
        DropAt(pipette.transform, bottleT2.transform.position + Vector3.up * 0.05f);
        pipette.transform.eulerAngles = new Vector3(-180,0,0);
        yield return Wait();
        hand.InteractWith(pipette);
        yield return Wait();
        pipette.SendMedicine();
        hand.Uninteract();

        // soycaseine 1
        yield return Wait(0.5f);
        tioglygolate.transform.eulerAngles *= 0f;
        DropAt(pipette.transform, soycaseine.transform.position + Vector3.up * 0.2f);
        pipette.transform.eulerAngles = new Vector3(-180,0,0);
        hand.transform.position = pipette.transform.position;
        hand.transform.eulerAngles = Vector3.down;
        hand.InteractWith(pipette);
        yield return Wait(0.5f);
        pipette.TakeMedicine();
        yield return Wait();
        hand.Uninteract();
        yield return Wait();
        DropAt(pipette.transform, bottleS1.transform.position + Vector3.up * 0.05f);
        pipette.transform.eulerAngles = new Vector3(-180,0,0);
        yield return Wait();
        hand.InteractWith(pipette);
        yield return Wait();
        pipette.SendMedicine();
        hand.Uninteract();

        // soycaseine 2
        yield return Wait(0.5f);
        tioglygolate.transform.eulerAngles *= 0f;
        DropAt(pipette.transform, soycaseine.transform.position + Vector3.up * 0.2f);
        pipette.transform.eulerAngles = new Vector3(-180,0,0);
        hand.transform.position = pipette.transform.position;
        hand.transform.eulerAngles = Vector3.down;
        hand.InteractWith(pipette);
        yield return Wait(0.5f);
        pipette.TakeMedicine();
        yield return Wait();
        hand.Uninteract();
        yield return Wait();
        DropAt(pipette.transform, bottleS2.transform.position + Vector3.up * 0.05f);
        pipette.transform.eulerAngles = new Vector3(-180,0,0);
        yield return Wait();
        hand.InteractWith(pipette);
        yield return Wait();
        pipette.SendMedicine();
        hand.Uninteract();

        yield break;
    }

    private GameObject InstantiateObject(GameObject prefab) {
        GameObject g = Instantiate(prefab, spawnPos, Quaternion.Euler(Vector3.zero));
        spawnPos += Vector3.one;
        if (g == null) Logger.Error("Failed instantiating " + prefab);
        return g;
    }

    private void DropAt(Transform theObject, Transform position) {
        DropAt(theObject, position.position);
    }

    private void DropAt(Transform theObject, Vector3 position) {
        theObject.position = position;
        theObject.eulerAngles = Vector3.up;
        theObject.gameObject.GetComponent<Rigidbody>().velocity *= 0f;
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
