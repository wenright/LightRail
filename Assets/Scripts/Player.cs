using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Rail currentRail;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Transfer player to next rail
		if (currentRail.transform.position.y <= -0.3) {
			Rail nextRail = currentRail.GetNext();

			if (nextRail is DeadEndRail) {
				// TODO gameover
			} else {
				currentRail = nextRail;
			}
		}
	}
}
