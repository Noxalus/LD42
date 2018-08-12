using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LimitBar : MonoBehaviour
{
    public float SecondsBeforeGameOver = 5f;
    public TextMeshProUGUI GameOverTimerText;
    public Image GameOverWarningOverlay;

    private List<Rigidbody2D> _detectedPackages = new List<Rigidbody2D>();
    private bool _gameWillBeOver = false;
    private float _gameOverTimer;
    private bool _gameIsOver = false;

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
        else
        {
            _warningOverlayColor.a = 0;
            GameOverWarningOverlay.color = _warningOverlayColor;
            GameOverTimerText.alpha = 0;
            _gameOverTimer = SecondsBeforeGameOver;
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
        newPosition.y += amount;
        transform.position = newPosition;
    }
}
