﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour {

	public Rail currentRail;
	public bool willTakeRightBranch = false;
	public bool willTakeLeftBranch = false;
	public Text gameOverText;
	public Text tapToRetryText;

	// TODO move this into a different script that controls rotation
	private float lastPosX;

	// TODO there should be a game controller that deals with gameOver states
	public bool gameOver = false;

	private RailSpawner railSpawner;

	// Use this for initialization
	void Start () {
		railSpawner = GameObject.FindWithTag("GameController").GetComponent<RailSpawner>();

		lastPosX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentRail == null) {
			// This shouldn't ever happen, but just in case it should be dealt with
			return;
		}

		// Transfer player to next rail
		if (currentRail.transform.position.y <= -1f) {
			Rail nextRail = currentRail.GetNext();

			if (nextRail is DeadEndRail) {
				if (!gameOver) {
					gameOver = true;
					railSpawner.speed = 0.0f;

					// TODO tweening and non max white colors
					gameOverText.color = Color.white;
					tapToRetryText.color = Color.white;

					if (railSpawner.score > railSpawner.highScore) {
						PlayerPrefs.SetFloat("highscore", railSpawner.score);
					}
				}
			} else {
				Rail branchToTake = nextRail;

				if (currentRail is BranchRail) {
					if (WillTakeBranch()) {
						branchToTake = (currentRail as BranchRail).GetBranchedRail();

						willTakeLeftBranch = false;
						willTakeRightBranch = false;
					}
				}

				currentRail = branchToTake;
			}
		}

		transform.position = new Vector3(currentRail.GetX(), 0, 0);

		// Rotate player so that they follow the path of the rail
		// TODO branch rails still have some jerky parts at the beginning and the end
		float vx = lastPosX - transform.position.x;
		if (Mathf.Abs(vx) >= 0.000001f) {
			float rotZ = -Mathf.Atan2(railSpawner.speed * Time.deltaTime, vx);
			float rotationOffset = 90.0f;
			transform.rotation = Quaternion.Euler(0, 0, rotZ * Mathf.Rad2Deg - rotationOffset);
			lastPosX = transform.position.x;
		} else {
			transform.rotation = Quaternion.identity;
		}

		#if UNITY_EDITOR
			if (Input.GetKeyDown("right")) {
				Swipe(SwipeDetection.Directions.Right);
			} else if (Input.GetKeyDown("left")) {
				Swipe(SwipeDetection.Directions.Left);
			}
		#endif

	}

	// TODO Once 3 directional branches are added, swipe direction will become important
	public void Swipe (SwipeDetection.Directions direction) {
		BranchRail branchRail = null;
		if (currentRail is BranchRail) {
			branchRail = currentRail as BranchRail;
		} else if (currentRail.next is BranchRail) {
			branchRail = currentRail.next as BranchRail;
		}

		// TODO Allow undoing of swipes (Swipe right, then swipe left before reaching branch to go straight)
		// TODO how far along should changing be allowed? up to two rails away? and can you change on the current rail?
		if (branchRail) {
			if (direction == SwipeDetection.Directions.Left && !branchRail.branchRight) {
				willTakeLeftBranch = true;
				branchRail.SwapAlphas();				
			} else if (direction == SwipeDetection.Directions.Right && branchRail.branchRight) {
				willTakeRightBranch = true;
				branchRail.SwapAlphas();				
			}
		}
	}

	public Rail GetClosestBranch () {
		return currentRail.GetClosestBranch();
	}

	public bool WillTakeBranch () {
		return ((currentRail as BranchRail).branchRight && willTakeRightBranch) || (!(currentRail as BranchRail).branchRight && willTakeLeftBranch);
	}
}
