using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource AudioSource;

    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI ScoreText;

    public CanvasGroup GameOverUIGroup;

    public Animator ScoreAnimator;

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
        GameOverUIGroup.alpha = 0;
        GameOverUIGroup.interactable = false;
        GameOverUIGroup.blocksRaycasts = false;
    }
	
	void Update ()
    {
        if (GameIsOver())
            return;
        
        _gameTimer += Time.deltaTime;
        UpdateUI();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IncreaseScore(int amount)
    {
        _score += amount;
        ScoreAnimator.SetTrigger("ScoreIncreased");
    }

    public void GameOver(bool notAPackageDeath = false)
    {
        if (notAPackageDeath)
        {
            Debug.Log("Warning, you can't be considered as a package!");
        }

        Debug.Log("Game Over!");
        _gameIsOver = true;

        // Show game over UI
        GameOverUIGroup.alpha = 1;
        GameOverUIGroup.interactable = true;
        GameOverUIGroup.blocksRaycasts = true;
    }

    public bool GameIsOver()
    {
        return _gameIsOver;
    }

    private void UpdateUI()
    {
        TimerText.text = TimeSpan.FromSeconds(_gameTimer).ToString(@"mm\:ss");
        ScoreText.text = _score.ToString();
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource.clip = clip;
        AudioSource.Play();
    }
}
