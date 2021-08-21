using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI PointsText;
    public GameObject HealthContainer;
    public GameObject HealthUIItemPrefab;

    public GameObject GameOverUI;
    public GameObject HighScoreUI;
    public TMP_InputField NameInputField;
    public HighScoreManager HighScoreManager;

    private int _points;

    private void Awake()
    {
        GameOverUI.SetActive(false);
        HighScoreUI.SetActive(false);
    }

    public void GameOver()
    {
        GameOverUI.SetActive(true);

        var isNewHighScore = HighScoreManager.IsNewHighScore(_points);
        HighScoreUI.SetActive(isNewHighScore);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddToHighScore()
    {
        var playerName = NameInputField.text;

        if (string.IsNullOrWhiteSpace(playerName))
        {
            return;
        }

        HighScoreManager.Add(playerName, _points);
        HighScoreUI.SetActive(false);
    }

    public void AddPoints(int points)
    {
        _points += points;
        PointsText.text = _points.ToString();
    }

    public void SetHealth(int health)
    {
        // Rückwärtsschleife
        for (var i =  HealthContainer.transform.childCount -1; i >= 0; i--)
        {
            var child = HealthContainer.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        // Normal
        for (var i = 0; i < health; i++)
        {
            Instantiate(HealthUIItemPrefab, HealthContainer.transform);
        }
    }
}
