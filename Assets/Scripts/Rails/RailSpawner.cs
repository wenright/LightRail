using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;
using DG.Tweening;

public class RailSpawner : MonoBehaviour {

	public Vector3 spawnPoint = new Vector3(0, 5, 0);
	public Vector3 destroyPoint = new Vector3(0, -7, 0);
	public float speed = 3.0f;
	public GameObject rail;
	public GameObject branchRight;
	public GameObject branchLeft;
	public GameObject deadEnd;
	public PostProcessingBehaviour postProcessingStack;

	public UnityEngine.UI.Text scoreText;
	public UnityEngine.UI.Text highScoreText;
	public float score = 0.0f;
	public float highScore = 0.0f;

	public Player player;

	public List<Rail> rails;

	private float acceleration = 0.1f;
	private bool hasBeatenHighScore = false;
	private float savedSpeed = 0.0f;

	void Start () {
		// TODO remove this for actual game. Useful for debugging though
		// Random.InitState(321123);

		rails = new List<Rail>();

		GameObject playerObject = GameObject.FindWithTag("Player");
		player = playerObject.GetComponent<Player>();

		rail = Resources.Load("rail") as GameObject;
		branchRight = Resources.Load("branchRight") as GameObject;
		branchLeft = Resources.Load("branchLeft") as GameObject;
		deadEnd = Resources.Load("deadEnd") as GameObject;

		highScore = PlayerPrefs.GetFloat("highscore", 0.0f);
		highScoreText.text = "HI " + ((int) highScore).ToString();

		if (!PlayerPrefs.HasKey("HighQuality")) {
			postProcessingStack.enabled = true;
		}
	}

	void Update () {
		if (!player.gameOver) {
			score += (speed * Time.deltaTime) / 1.5f;
			scoreText.text = ((int) score).ToString();

			// TODO check highscore, update it if beaten
			if (score > highScore) {
				if (!hasBeatenHighScore) {
					hasBeatenHighScore = true;

					scoreText.DOFade(0.0f, 0.25f).SetEase(Ease.OutQuad);
					scoreText.rectTransform.DOAnchorPosY(-48, 0.25f).SetEase(Ease.OutQuad);
				}

				highScoreText.text = "HI " + ((int) score).ToString();
			}

			speed += acceleration * Time.deltaTime;
		}
	}

	// Add rail to rails list
	public void AddRail (Rail obj) {
		rails.Add(obj);
	}

	public void RemoveRail (Rail obj) {
		rails.Remove(obj);
	}

	public void Pause () {
		savedSpeed = speed;
		speed = 0;
		acceleration = 0;
	}

	public void Unpause () {
		// Wait a little to start the game up again
		Invoke("UnpauseInvoke", 0.25f);
	}

	private void UnpauseInvoke () {
		speed = savedSpeed;
		acceleration = 0.1f;
	}

	public void RestartGame () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		speed = 3.0f;
	}
}
