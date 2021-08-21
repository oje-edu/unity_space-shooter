using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class HighScoreManager : MonoBehaviour
{
    [Serializable]
    public class HighScoreContainer
    {
        public HighScoreEntry[] Highscores;
    }

    [Serializable]
    public class HighScoreEntry
    {
        public string Name;
        public int Score;
    }
    private const string FileName = "highscore.json";
    private const int MaxHighScore = 10;

    private string HighScoreFilePath => Path.Combine(Application.persistentDataPath, FileName);

    private List<HighScoreEntry> _highscore = new List<HighScoreEntry>();

    private void Awake()
    {
        Load();
    }

    private void OnDestroy()
    {
        Save();
    }

    private void Save()
    {
        var highscoreContainer = new HighScoreContainer()
        {
            Highscores = _highscore.ToArray()
        };

        var json = JsonUtility.ToJson(highscoreContainer);
        File.WriteAllText(HighScoreFilePath, json);
    }

    private void Load()
    {
        Debug.Log($"Loading Highscores from {HighScoreFilePath}");
        if (!File.Exists(HighScoreFilePath))
        {
            return;
        }
        var json = File.ReadAllText(HighScoreFilePath);
        var highscoreContainer = JsonUtility.FromJson<HighScoreContainer>(json);

        if (highscoreContainer == null || highscoreContainer.Highscores == null)
        {
            return;
        }

        _highscore = new List<HighScoreEntry>(highscoreContainer.Highscores);
    }

    private void Add(HighScoreEntry entry)
    {
        _highscore.Insert(0, entry);
        _highscore = _highscore.Take(MaxHighScore).ToList();
    }

    public bool IsNewHighScore(int score)
    {
        if (score <= 0)
        {
            return false;
        }

        if (_highscore.Count == 0)
        {
            return true;
        }

        var firstEntry = _highscore[0];
        return score > firstEntry.Score;
    }

    public void Add(string playerName, int score)
    {
        if (!IsNewHighScore(score))
        {
            return;
        }
        var entry = new HighScoreEntry()
        {
            Name = playerName,
            Score = score
        };

        Add(entry);
    }
    public List<HighScoreEntry> List()
    {
        return _highscore;
    }
}
