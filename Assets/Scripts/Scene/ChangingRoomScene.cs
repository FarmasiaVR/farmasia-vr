using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ChangingRoomScene : SceneScript {

    public enum AutoPlayStrength {
        None = 0,
        HandsWashed,
    }

    [SerializeField]
    public AutoPlayStrength autoPlayStrength;

    [SerializeField]
    private GameObject soapDispencer
        ;
}
