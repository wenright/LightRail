﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Rail currentRail;
	public bool willTakeBranch = false;

	private float lastPosX;

	// Use this for initialization
	void Start () {
		lastPosX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		// Transfer player to next rail
		if (currentRail.transform.position.y <= -0.3) {
			Rail nextRail = currentRail.GetNext();

			if (nextRail is DeadEndRail) {
				// TODO gameover
				Debug.Log("Game Over!");
				RailSpawner.speed = 0.0f;
			} else {
				Rail branchToTake = null;

				if (willTakeBranch && currentRail is BranchRail) {
					willTakeBranch = false;

					branchToTake = (currentRail as BranchRail).GetBranchedRail();
				} else {
					branchToTake = nextRail;
				}

				currentRail = branchToTake;
			}
		}

		transform.position = new Vector3(currentRail.GetX(), 0, 0);

		// TODO when branches are replaced with curves, readd this code
		float vx = lastPosX - transform.position.x;
		float rotZ = -Mathf.Atan2(RailSpawner.speed * Time.deltaTime, vx);
		float rotationOffset = 90.0f;
		transform.rotation = Quaternion.Euler(0, 0, rotZ * Mathf.Rad2Deg - rotationOffset);
		lastPosX = transform.position.x;
	}

	// TODO Once 3 directional branches are added, swipe direction will become important
	public void SwipeLeft () {
		willTakeBranch = true;
	}

	public void SwipeRight () {
		willTakeBranch = true;
	}

	public Rail GetClosestBranch () {
		return currentRail.GetClosestBranch();
	}
}
