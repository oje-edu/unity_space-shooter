using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI PointsText;
    public GameObject HealthContainer;
    public GameObject HealthUIItemPrefab;

    private int _points;

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
