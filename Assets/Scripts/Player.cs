using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Rail currentRail;
	public bool willTakeRightBranch = false;
	public bool willTakeLeftBranch = false;

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
		float vx = lastPosX - transform.position.x;
		float rotZ = -Mathf.Atan2(RailSpawner.speed * Time.deltaTime, vx);
		float rotationOffset = 90.0f;
		transform.rotation = Quaternion.Euler(0, 0, rotZ * Mathf.Rad2Deg - rotationOffset);
		lastPosX = transform.position.x;
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
