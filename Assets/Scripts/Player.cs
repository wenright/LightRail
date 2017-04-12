using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Rail currentRail;
	public bool willTakeRightBranch = false;
	public bool willTakeLeftBranch = false;
	public Text gameOverText;
	public Text tapToRetryText;

	private float lastPosX;

	// TODO there should be a game controller that deals with gameOver states
	public bool gameOver = false;

	// Use this for initialization
	void Start () {
		lastPosX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentRail == null) {
			// This shouldn't ever happen, but just in case it should be dealt with
			return;
		}

		// Transfer player to next rail
		if (currentRail.transform.position.y <= -0.3) {
			Rail nextRail = currentRail.GetNext();

			if (nextRail is DeadEndRail) {
				if (!gameOver) {
					gameOver = true;
					RailSpawner.speed = 0.0f;

					// TODO tweening and non max white colors
					gameOverText.color = Color.white;
					tapToRetryText.color = Color.white;
				}
			} else {
				Rail branchToTake = nextRail;

				if (currentRail is BranchRail) {
					if (((currentRail as BranchRail).branchRight && willTakeRightBranch) || (!(currentRail as BranchRail).branchRight && willTakeLeftBranch)) {
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
			float rotZ = -Mathf.Atan2(RailSpawner.speed * Time.deltaTime, vx);
			float rotationOffset = 90.0f;
			transform.rotation = Quaternion.Euler(0, 0, rotZ * Mathf.Rad2Deg - rotationOffset);
			lastPosX = transform.position.x;
		} else {
			transform.rotation = Quaternion.identity;
		}
	}

	// TODO Once 3 directional branches are added, swipe direction will become important
	public void Swipe (SwipeDetection.Directions direction) {
		if (currentRail is BranchRail || currentRail.next is BranchRail) {
			if (direction == SwipeDetection.Directions.Left) {
				willTakeLeftBranch = true;
			} else {
				willTakeRightBranch = true;
			}
		}
	}

	public Rail GetClosestBranch () {
		return currentRail.GetClosestBranch();
	}
}
