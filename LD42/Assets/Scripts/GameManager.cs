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

    public static GameManager Instance()
    {
        if (!_instance)
            _instance = new GameManager();

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

    private void UpdateUI()
    {
        TimerText.text = TimeSpan.FromSeconds(_gameTimer).ToString(@"mm\:ss\.fff");
    }
}
