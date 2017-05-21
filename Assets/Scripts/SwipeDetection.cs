using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour {

	public enum Directions {Left, Right};
	public Directions direction;

	private Player player;
	private Vector3 mouseStart;

	// Distance in pixels that mouse needs to be moved to be considered a swipe
	private int gate = 20;

	void Start () {
		player = GetComponent<Player>();
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			mouseStart = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0)) {
			CheckSwipe();
		}
	}

	private void CheckSwipe () {
		float delta = (mouseStart - Input.mousePosition).x;

		if (Mathf.Abs(delta) >= gate) {
			if (delta > 0) {
				player.Swipe(Directions.Left);
			} else {
				player.Swipe(Directions.Right);
			}
		}
	}
}