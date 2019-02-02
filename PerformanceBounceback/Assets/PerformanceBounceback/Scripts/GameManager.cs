using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private int _initialScore;
	private float _timeLeftSeconds;
	private BallSpawner _ballSpawner;
	private string _requiredScoreString;
	private string _scoreFormatString = "Score: {0:D2}/{1:D2}";
	private string _timerFormatString = "{0:D2}:{1:D2}";
	private string _winString = "YOU WIN!";
	private string _loseString = "YOU LOSE :(";
	private bool isRestarting = false;
	public int levelTimeSeconds = 30;
    public int score;
	public int requiredScore = 10;
	public List<Text> scoreTextList;

	public Text timeLeftText;

	public Text winLoseText;

	public GameObject ballSpawnerContainer;

	public GameObject winLosePanelUI;

	[Tooltip("Next level scene name")]
	public string NextLevel = "Scene 0";

	void Start () {
		_initialScore = score;
		_ballSpawner = ballSpawnerContainer.GetComponent<BallSpawner>();
		_requiredScoreString = requiredScore.ToString();

		InitializeCounters();
	}

	void Update() {
		if (isRestarting) {
			return;
		}

		_timeLeftSeconds -= Time.deltaTime;
		if (_timeLeftSeconds < 0) {
			handleGameFinished();
		} else
        {
            updateGuiTimer();
        }
    }

	public void IncrementScore() {
		if (isRestarting) {
			return;
		}

		score++;

		RefreshUI();

		if (score >= requiredScore) {
			handleGameFinished();
		}
	}

    private void RefreshUI()
    {
		var scoreText = string.Format(_scoreFormatString, score.ToString(), _requiredScoreString);

		foreach(var text in scoreTextList) {
			text.text = scoreText;
		}
    }

	private void handleGameFinished()
    {
		isRestarting = true;

        _ballSpawner.IsSpawningActive = false;

		if (score < requiredScore) {
			winLoseText.text = _loseString;
		} else {
			winLoseText.text = _winString;
		}

		winLosePanelUI.SetActive(true);

		StartCoroutine(RestartGame());
    }

    private void updateGuiTimer()
    {
 		var timeSpan = TimeSpan.FromSeconds(_timeLeftSeconds);
 		string timeText = string.Format(_timerFormatString, timeSpan.Minutes, timeSpan.Seconds);

		if (timeLeftText)
			timeLeftText.text = String.Format("{0}", timeText);
    }

	IEnumerator RestartGame() {
		yield return new WaitForSeconds(3);

		score = _initialScore;
		_ballSpawner.Initialize();

		InitializeCounters();
	}

	void InitializeCounters() {
		_timeLeftSeconds = levelTimeSeconds;
		score = _initialScore;

		RefreshUI();

		winLosePanelUI.SetActive(false);

		isRestarting = false;
	}
}
