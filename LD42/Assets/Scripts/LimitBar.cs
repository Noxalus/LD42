using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LimitBar : MonoBehaviour
{
    public float SecondsBeforeGameOver = 5f;
    public TextMeshProUGUI GameOverTimerText;

    private List<Rigidbody2D> _detectedPackages = new List<Rigidbody2D>();
    private bool _gameWillBeOver = false;
    private float _gameOverTimer;
    private bool _gameIsOver = false;

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
            GameOverTimerText.text = TimeSpan.FromSeconds(_gameOverTimer).ToString(@"s\.fff");

            if (_gameOverTimer < 0)
            {
                _gameIsOver = true;
                GameOverTimerText.alpha = 1f;
                GameOverTimerText.text = "Game Over";
                GameManager.Instance().GameOver();
            }
        }
        else
        {
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
}
