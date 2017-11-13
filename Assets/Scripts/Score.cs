using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Score : MonoBehaviour {
	public UnityEngine.UI.Text scoreText;
	public UnityEngine.UI.Text highScoreText;
	public GooglePlay googlePlayController;

	private int score = 0;
	private int highScore = 0;

	private bool hasBeatenHighScore = false;

	void Start () {
		highScore = PlayerPrefs.GetInt("highscore", 0);

		UpdateScoreUI();
	}

	public void AddToScore (int x) {
		score += x;

		if (score > highScore) {
			if (!hasBeatenHighScore) {
				hasBeatenHighScore = true;

				// Hide current score, so it just shows the high score text
				scoreText.DOFade(0.0f, 0.25f).SetEase(Ease.OutQuad);
				scoreText.rectTransform.DOAnchorPosY(-48, 0.25f).SetEase(Ease.OutQuad);
			}

			highScore = score;
		}

		UpdateScoreUI();
	}

	private void UpdateScoreUI () {
		scoreText.text = score.ToString();
		highScoreText.text = "HI " + highScore.ToString();
	}

	public int GetScore () {
		return score;
	}
}
