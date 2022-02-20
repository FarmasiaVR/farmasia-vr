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
        p_soyCaseineBottle, p_peptonWaterBottle, p_tioglykolateBottle, p_pump, p_pump_filter, p_filledSterileBag;

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

        }.Select(InstantiateObject).ToList();

        List<Transform> transforms = gameObjects.Select(go => go.transform).ToList();

        yield return Wait();

        //AllowCollisionsBetween(all, false);

        yield return Wait();

        Hand hand = VRInput.Hands[0].Hand;

        // --- Set to correct positions in throughput cabinet ---

        for (int i = 0; i < transforms.Count; i++) { 
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
        return Wait(0.25f);
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
