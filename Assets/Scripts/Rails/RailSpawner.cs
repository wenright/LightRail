using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RailSpawner : MonoBehaviour {

	public Vector3 spawnPoint = new Vector3(0, 5, 0);
	public Vector3 destroyPoint = new Vector3(0, -7, 0);
	public float speed = 4.0f;
	public GameObject rail;
	public GameObject branchRight;
	public GameObject branchLeft;
	public GameObject deadEnd;
	public GameObject coin;

	public Player player;

	public List<Rail> rails;

	public AdManager adManager;
	public FadeImage fillImage;

	// TODO sqrt(x) acceleration?
	private float acceleration = 0.075f;

	void Start () {
		// TODO remove this for actual game. Useful for debugging though
		// Random.InitState(321123);

		rails = new List<Rail>();

		GameObject playerObject = GameObject.FindWithTag("Player");
		player = playerObject.GetComponent<Player>();

		// TODO maybe wait a few seconds before setting the rail speed from 0
	}

	void Update () {
		if (!player.gameOver) {
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

	public void RestartGame () {
		// TODO maybe this should be called from the OnComplete of the fadeout, in case the fadout duration is changed in the future
		Invoke("DelayedRestartGame", 0.75f);
		fillImage.FadeIn();
	}

	private void DelayedRestartGame () {
		if (adManager.ShouldPlayAd()) {
			adManager.PlayAd();
		} else {
			ResetScene();
		}
	}

	public void ResetScene () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
