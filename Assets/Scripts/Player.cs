using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour {

    [Serializable]
    public struct PlayerData {
        public string Name;
        public string Number;
        public string Timestamp;
        public int Score;
    }

    [Serializable]
    private struct DataWrapper {
        public string LatestPlaythrough;
        public int MaxScore;
        public PlayerData[] Stats;
        
        public DataWrapper(PlayerData[] stats) {
            MaxScore = MAX_POINTS;
            Stats = stats;
            LatestPlaythrough = stats[stats.Length - 1].Timestamp;
        }
    }

    #region fields
    public static PlayerData Info;

    public static Transform Transform { get; private set; }
    public static Camera Camera { get; private set; }
    const int MAX_POINTS = 25;
    const string DefaultNumber = "DEFAULT";
    #endregion

    public void Awake() {

        Transform = transform;
        Camera = Transform.Find("Camera").GetComponent<Camera>();

        if (Transform == null || Camera == null) {
            throw new System.Exception("Player init failed");
        }
    }

    public static void Initialize(string name, string number) {

        if (name == null || name.Trim().Length == 0) {
            name = "tester";
        }
        if (number == null || number.Trim().Length == 0) {
            number = DefaultNumber;
        }

        Info = new PlayerData();

        Info.Name = name.Trim();
        Info.Number = number.Trim();
    }

    public static void SavePlayerData(int score) {

        if (DefaultNumber.Equals(Info.Number)) {
            return;
        }

        string path = Application.dataPath + "/stats.txt";

        PlayerData[] prev = LoadPlayerData(path);

        PlayerData[] toSave = new PlayerData[prev.Length + 1];

        for (int i = 0; i < prev.Length; i++) {
            toSave[i] = prev[i];
        }

        Info.Timestamp = DateTime.Now.ToString();
        toSave[prev.Length] = Info;

        string json = JsonUtility.ToJson(new DataWrapper(toSave), true);

        Logger.Print("Info´to json: " + JsonUtility.ToJson(Info));
        Logger.Print("Saving data: " + path);
        Logger.Print("Player count: " + toSave.Length);
        Logger.Print("DATA: " + json.Length + ", " + json);
        File.WriteAllText(path, json);
    }

    private static PlayerData[] LoadPlayerData(string path) {

        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            try {
                DataWrapper w = JsonUtility.FromJson<DataWrapper>(json);
                if (w.Stats != null) {
                    return w.Stats;
                }
            } catch {
                return new PlayerData[0];
            }
        }

        return new PlayerData[0];
    }
}

