using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TopScoreManager : MonoBehaviour
{
    public static TopScoreManager Instance;
    private Score[] topScores;
    private int topScoresLength = 10;
    private string currentPlayerName;
    
    private void Awake()
    {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        this.LoadTopScores();
        Instance = this;
    }

    private void LoadTopScores()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            Score[] data = JsonUtility.FromJson<Score[]>(json);
            this.topScores = data;
        } else {
            this.topScores = new Score[this.topScoresLength];
        }
    }

    public void SaveTopScores()
    {
        string json = JsonUtility.ToJson(this.topScores);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public string GetTopScoresString()
    {
        string scoreString = "";
        for (int i = 0; i < this.topScores.Length; i++) {
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

    [System.Serializable]
    class Score
    {
        public string playerName;
        public int score;

        public Score(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }
    }
}
