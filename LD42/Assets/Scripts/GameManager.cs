using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI ScoreText;

    private int _score;
    private float _gameTimer;

    private static GameManager _instance;

    public void Awake()
    {
        _instance = this;
    }

    public static GameManager Instance()
    {
        return _instance;
    }

    void Start ()
    {
	}
	
	void Update ()
    {
        _gameTimer += Time.deltaTime;

        UpdateUI();
    }

    public void IncreaseScore(int amount)
    {
        _score += amount;
    }

    private void UpdateUI()
    {
        TimerText.text = TimeSpan.FromSeconds(_gameTimer).ToString(@"mm\:ss\.fff");
        ScoreText.text = _score.ToString();
    }
}
