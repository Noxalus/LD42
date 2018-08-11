using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI ScoreText;

    private int _score;
    private float _gameTimer;
    private bool _gameIsOver = false;

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
        _gameIsOver = false;
    }
	
	void Update ()
    {
        if (GameIsOver())
            return;
        
        _gameTimer += Time.deltaTime;
        UpdateUI();
    }

    public void IncreaseScore(int amount)
    {
        _score += amount;
    }

    public void GameOver(bool notAPackageDeath = false)
    {
        if (notAPackageDeath)
        {
            Debug.Log("Warning, you can't be considered as a package!");
        }

        Debug.Log("Game Over!");
        _gameIsOver = true;
    }

    public bool GameIsOver()
    {
        return _gameIsOver;
    }

    private void UpdateUI()
    {
        TimerText.text = TimeSpan.FromSeconds(_gameTimer).ToString(@"mm\:ss\.fff");
        ScoreText.text = _score.ToString();
    }
}
