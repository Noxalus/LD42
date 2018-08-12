using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource AudioSource;

    public AudioClip ScoreSound;

    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GameOverSubText;
    public LimitBar LimitBar;

    public CanvasGroup GameOverUIGroup;

    public Animator ScoreAnimator;

    public List<Transform> SpawnerHolders;
    public GameObject SpawnerPrefab;

    public CanvasGroup PickupTutorial;
    public CanvasGroup DeliveryTutorial;

    private int _score;
    private float _gameTimer;
    private bool _gameIsOver = false;
    private int _difficultyLevel = 0;
    private static GameManager _instance;
    private bool _firstScore = true;
    private List<PackageSpawner> _spawners = new List<PackageSpawner>();

    private static bool _tutorialFinished = false;

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

        PickupTutorial.alpha = _tutorialFinished ? 0 : 1;
        DeliveryTutorial.alpha = 0;
    }
	
	void Update ()
    {
        if (GameIsOver())
        {
            if (Input.anyKeyDown)
                Restart();

            return;
        }
        
        _gameTimer += Time.deltaTime;

        LimitBar.IncreaseHeight(0.0001f);

        UpdateUI();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IncreaseScore(int amount)
    {
        if (!_tutorialFinished)
        {
            PickupTutorial.alpha = 0;
            DeliveryTutorial.alpha = 0;
            _tutorialFinished = true;
        }

        if (_firstScore)
        {
            _firstScore = false;
            AddSpawner();
        }

        _score += amount;
        ScoreAnimator.SetTrigger("ScoreIncreased");

        PlaySound(ScoreSound);
    }

    public void AddSpawner()
    {
        if (SpawnerHolders.Count >= _spawners.Count)
            Instantiate(SpawnerPrefab, SpawnerHolders[_spawners.Count]);
    }

    public void GameOver(bool notAPackageDeath = false)
    {
        GameOverSubText.text = "You died drowned under a mountain of packages...";

        if (notAPackageDeath)
        {
            Debug.Log("Warning, you can't be considered as a package!");
            GameOverSubText.text = "You died from a bad fall, sad story...";
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

    public int GetDifficultyLevel()
    {
        return _difficultyLevel;
    }

    public void OnPackagePickedUp()
    {
        if (!_tutorialFinished)
        {
            PickupTutorial.alpha = 0;
            DeliveryTutorial.alpha = 1;
        }
    }
}
