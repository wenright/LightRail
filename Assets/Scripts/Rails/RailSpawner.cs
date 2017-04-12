using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RailSpawner : MonoBehaviour {

	public Vector3 spawnPoint = new Vector3(0, 5, 0);
	public Vector3 destroyPoint = new Vector3(0, -7, 0);
	public float speed = 3f;
	public GameObject rail;
	public GameObject branchRight;
	public GameObject branchLeft;
	public GameObject deadEnd;

	public UnityEngine.UI.Text scoreText;
	public UnityEngine.UI.Text highScoreText;
	public float score = 0.0f;
	public float highScore = 0.0f;

	public Player player;

	private float acceleration = 0.1f;

	public List<Rail> rails;

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
	}

	void Update () {
		if (player.gameOver) {
			if (Input.GetMouseButtonDown(0)) {
				// Restart game
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				speed = 3.0f;
			}
		} else {
			score += (speed * Time.deltaTime) / 1.5f;
			scoreText.text = ((int) score).ToString();

			// TODO check highscore, update it if beaten

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
}
