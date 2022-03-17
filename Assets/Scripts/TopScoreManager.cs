using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TopScoreManager : MonoBehaviour
{
    public static TopScoreManager Instance;
    private List<Score> topScores;
    private int topScoresSize;
    private string currentPlayerName;
    
    private void Awake()
    {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        this.topScoresSize = 10;
        DontDestroyOnLoad(this.gameObject);
        this.LoadTopScores();
        Instance = this;
    }

    private void LoadTopScores()
    {
        string path = Application.persistentDataPath + "/savedata";
        if (File.Exists(path)) {
            byte[] data = File.ReadAllBytes(path);
            this.topScores = MessagePackSerializer.Deserialize<List<Score>>(data);
        } else {
            this.topScores = new List<Score>();
        }
    }

    public void SaveTopScores()
    {
        byte[] data = MessagePackSerializer.Serialize(this.topScores);
        File.WriteAllBytes(Application.persistentDataPath + "/savedata", data);
    }

    public string GetTopScoresString()
    {
        string scoreString = "";
        for (int i = 0; i < this.topScores.Count; i++) {
            if (this.topScores[i] != null) {
                scoreString += "(" + (i+1) + ") " + this.topScores[i].playerName + " : " + this.topScores[i].score + System.Environment.NewLine;
            } else {
                break;
            }
        }

        if (scoreString == "") {
            scoreString = "No scores yet";
        }

        return scoreString;
    }

    public void SetCurrentPlayer(string name)
    {
        this.currentPlayerName = name;
    }

    public string GetCurrentPlayerName()
    {
        return this.currentPlayerName;
    }

    public string GetBestScore()
    {
        if (this.topScores.Count > 0 && this.topScores[0] != null) {
            return "Best Score: " + this.topScores[0].playerName + " : " + this.topScores[0].score;
        } else {
            return "Best Score: 0";
        }
    }

    public void AddNewScore(int points)
    {
        this.topScores.Add(new Score(this.currentPlayerName, points));
        this.topScores.Sort();
        if (this.topScores.Count > this.topScoresSize) {
            this.topScores.RemoveAt(this.topScores.Count - 1);
        }
        this.SaveTopScores();
    }

    [MessagePackObject]
    public class Score : IComparable<Score>
    {
        [Key(0)]
        public string playerName;

        [Key(1)]
        public int score;

        public Score(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }

        public int CompareTo(Score other)
        {
            return Comparer<int>.Default.Compare(other.score, this.score);
        }
    }
}
