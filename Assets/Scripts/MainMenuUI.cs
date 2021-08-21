using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public HighScoreManager HighScoreManager;
    public GameObject HighScoreEntries;
    public GameObject HighScoreEntryUIPrefab;

    private void Start()
    {
        ShowHighScores();
    }

    private void ShowHighScores()
    {
        for (var i = HighScoreEntries.transform.childCount -1; i >= 0; i--)
        {
            var child = HighScoreEntries.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        var highscores = HighScoreManager.List();
        foreach (var highscore in highscores)
        {
            var highscoreEntry = Instantiate(HighScoreEntryUIPrefab, HighScoreEntries.transform);
            var textMeshPro = highscoreEntry.GetComponent<TextMeshProUGUI>();
            textMeshPro.text = $"{highscore.Score} - {highscore.Name}";
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void StopGame()
    {
        if (Application.isEditor)
        {
            #if UNITY_EDITOR 
            EditorApplication.isPlaying = false;
            #endif
        } else
        {
            Application.Quit();
        }

    }
}
