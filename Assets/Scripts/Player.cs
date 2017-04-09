using System.Collections;
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

				if (willTakeBranch && currentRail is BranchRightRail) {
					willTakeBranch = false;

					branchToTake = (currentRail as BranchRightRail).GetBranchedRail();
				} else {
					branchToTake = nextRail;					
				}

				currentRail.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
				branchToTake.gameObject.GetComponent<SpriteRenderer>().color = Color.green;

				currentRail = branchToTake;
			}
		}

		transform.position = new Vector3(currentRail.GetX(), 0, 0);

		// TODO when branches are replaced with curves, readd this code
		// float vx = lastPosX - transform.position.x;
		// float rotZ = Mathf.Atan2(2 * Time.deltaTime, vx);
		// float rotationOffset = 90.0f;
		// transform.rotation = Quaternion.Euler(0, 0, rotZ * Mathf.Rad2Deg - rotationOffset);
		// lastPosX = transform.position.x;
	}

	public void SwipeLeft () {

	}

	public void SwipeRight () {
		willTakeBranch = true;
	}
}
