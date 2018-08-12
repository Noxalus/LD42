using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LimitBar : MonoBehaviour
{
    public float SecondsBeforeGameOver = 5f;
    public float SecondsBeforeWarningGameOver = 1f;
    public TextMeshProUGUI GameOverTimerText;
    public Image GameOverWarningOverlay;

    private const float MaxLimit = 3.5f;
    private List<Rigidbody2D> _detectedPackages = new List<Rigidbody2D>();
    private bool _gameWillBeOver = false;
    private float _gameOverTimer;
    private bool _gameIsOver = false;
    private float _timeBeforeToDisplayWarning;

    private Color _warningOverlayColor;

    public void Start()
    {
        _warningOverlayColor = GameOverWarningOverlay.color;
        _warningOverlayColor.a = 0;
        GameOverWarningOverlay.color = _warningOverlayColor;
    }

    public void Update()
    {
        if (_gameIsOver)
            return;

        // Check if a package is stable at the limit
        foreach (var packageBody in _detectedPackages)
        {
            if (!packageBody)
                continue;

            if (packageBody.velocity.magnitude < 0.1f)
            {
                _gameWillBeOver = true;
                break;
            }
        }

        if (_gameWillBeOver)
        {
            _timeBeforeToDisplayWarning -= Time.deltaTime;

            if (_timeBeforeToDisplayWarning < 0)
            {
                _gameOverTimer -= Time.deltaTime;

                GameOverTimerText.alpha = 0.5f;
                GameOverTimerText.text = Mathf.CeilToInt((float)TimeSpan.FromSeconds(_gameOverTimer).TotalSeconds).ToString();

                _warningOverlayColor.a = (1 - (_gameOverTimer / SecondsBeforeGameOver)) * 0.5f;
                GameOverWarningOverlay.color = _warningOverlayColor;

                if (_gameOverTimer < 0)
                {
                    _gameIsOver = true;
                    GameOverTimerText.alpha = 0f;
                    _warningOverlayColor.a = 0;
                    GameOverWarningOverlay.color = _warningOverlayColor;
                    GameManager.Instance().GameOver();
                }
            }
        }
        else
        {
            _warningOverlayColor.a = 0;
            GameOverWarningOverlay.color = _warningOverlayColor;
            GameOverTimerText.alpha = 0;
            _gameOverTimer = SecondsBeforeGameOver;
            _timeBeforeToDisplayWarning = SecondsBeforeWarningGameOver;
        }

        _gameWillBeOver = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Package")
        {
            _detectedPackages.Add(collision.transform.parent.gameObject.GetComponent<Rigidbody2D>());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Package")
        {
            _detectedPackages.Remove(collision.transform.parent.gameObject.GetComponent<Rigidbody2D>());
        }
    }

    public void IncreaseHeight(float amount)
    {
        var newPosition = transform.position;
        newPosition.y = Mathf.Min(newPosition.y + amount, MaxLimit);

        if (newPosition.y >= -0.4f && newPosition.y <= 0.6f)
            newPosition.y = 0.6f;

        transform.position = newPosition;
    }
}
