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

    private const int MaxDifficulty = 6;

    private int _score;
    private float _gameTimer;
    private bool _gameIsOver = false;
    private int _difficultyLevel = 0;
    private static GameManager _instance;
    private bool _firstScore = true;
    private List<PackageSpawner> _spawners = new List<PackageSpawner>();
    private float _tutorialTimeOffset;

    private static bool _tutorialFinished = false;

    private float _delayBeforeRestart = 1f;

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
            _delayBeforeRestart -= Time.deltaTime;

            if (_delayBeforeRestart < 0 && Input.anyKeyDown)
            {
                Restart();
            }

            return;
        }

        if (_tutorialFinished)
        {
            _gameTimer += Time.deltaTime;
            LimitBar.IncreaseHeight(0.0001f * (_difficultyLevel + 1));
            UpdateDifficulty();

            UpdateUI();
        }
    }

    public void UpdateDifficulty()
    {
        if (_difficultyLevel == 0 && (_score >= 5 || GetRealGameTime() >= 30) ||
            _difficultyLevel == 1 && (_score >= 15 || GetRealGameTime() >= 60) ||
            _difficultyLevel == 2 && (_score >= 30 || GetRealGameTime() >= 90) ||
            _difficultyLevel == 3 && (_score >= 40 || GetRealGameTime() >= 120) ||
            _difficultyLevel == 4 && (_score >= 50 || GetRealGameTime() >= 140) ||
            _difficultyLevel == 5 && (_score >= 60 || GetRealGameTime() >= 160) ||
            _difficultyLevel == 6 && (_score >= 100 || GetRealGameTime() >= 300))
        {
            IncreaseDifficulty();
        }
    }

    private void IncreaseDifficulty()
    {
        var previousDifficulty = _difficultyLevel;
        _difficultyLevel = Mathf.Min(_difficultyLevel + 1, MaxDifficulty);

        if (previousDifficulty != _difficultyLevel)
        {
            if (_difficultyLevel == 1)
            {
                _spawners[0].SetDifficultyLevel(1);
            }
            if (_difficultyLevel == 2)
            {
                _spawners[0].SetDifficultyLevel(2);
                AddSpawner();
            }
            if (_difficultyLevel == 3)
            {
                _spawners[1].SetDifficultyLevel(1);
            }
            if (_difficultyLevel == 4)
            {
                AddSpawner();
                _spawners[2].SetDifficultyLevel(1);
            }
            if (_difficultyLevel == 5)
            {
                _spawners[2].SetDifficultyLevel(2);
                _spawners[1].SetDifficultyLevel(2);
            }
            if (_difficultyLevel == 6)
            {
                _spawners[0].SetDifficultyLevel(3);
            }
            if (_difficultyLevel == 7)
            {
                _spawners[1].SetDifficultyLevel(3);
                _spawners[2].SetDifficultyLevel(3);
            }
        }
    }

    private float GetRealGameTime()
    {
        return _gameTimer - _tutorialTimeOffset;
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
            _tutorialTimeOffset = _gameTimer;
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
        if (SpawnerHolders.Count > _spawners.Count)
            _spawners.Add(Instantiate(SpawnerPrefab, SpawnerHolders[_spawners.Count]).GetComponent<PackageSpawner>());
    }

    public void GameOver(bool notAPackageDeath = false)
    {
        GameOverSubText.text = "You died drowned under a mountain of packages...";

        if (notAPackageDeath)
        {
            Debug.Log("Warning, you can't be considered as a package!");
            GameOverSubText.text = "You died from a bad fall, sad story...\nThis place is for packages only, please be careful";
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
