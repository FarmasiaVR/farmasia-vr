using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour {

    [Serializable]
    public struct PlayerData {
        public string Name;
        public string StartTime;
        public string EndTime;
        public string PlayTime;
        public int Score;
        public string ScoreString;
    }

    [Serializable]
    private struct DataWrapper {
        public string LatestPlaythrough;
        public int MaxScore;
        public PlayerData[] Stats;
        
        public DataWrapper(PlayerData[] stats) {
            MaxScore = MAX_POINTS;
            Stats = stats;
            LatestPlaythrough = stats[stats.Length - 1].EndTime;
        }
    }

    #region fields
    public static bool Initialized { get; private set; }
    public static PlayerData Info;

    public static Transform Transform { get; private set; }
    public static Camera Camera { get; private set; }
    const int MAX_POINTS = 25;
    #endregion

    public void Awake() {

        Transform = transform;
        Camera = Transform.Find("Camera").GetComponent<Camera>();

        if (Transform == null || Camera == null) {
            throw new System.Exception("Player init failed");
        }
    }

    public static void Initialize(string name) {

        if (name == null || name.Trim().Length == 0) {
            name = "tester";
        }

        Info = new PlayerData();

        Info.Name = name.Trim();
        Info.StartTime = DateTime.Now.ToString();
        Initialized = true;
    }

    public static void SavePlayerData(int score, string scoreString) {

        if (Info.Name == null) {
            return;
        }

        string path = Application.dataPath + "/stats.txt";

        PlayerData[] prev = LoadPlayerData(path);

        PlayerData[] toSave = new PlayerData[prev.Length + 1];

        for (int i = 0; i < prev.Length; i++) {
            toSave[i] = prev[i];
        }

        Info.EndTime = DateTime.Now.ToString();
        Info.PlayTime = (DateTime.Now - DateTime.Parse(Info.StartTime)).ToString();
        Info.Score = score;
        Info.ScoreString = scoreString;
        toSave[prev.Length] = Info;

        string json = JsonUtility.ToJson(new DataWrapper(toSave), true);

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

