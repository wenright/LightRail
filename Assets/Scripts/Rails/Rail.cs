using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour {

	public Rail next;
	public GameObject railObject;

	void Start () {
		gameObject.name = "rail";
	}
	
	void Update () {
		transform.Translate(Vector2.up * -RailSpawner.speed * Time.deltaTime);

		if (next == null && transform.position.y <= RailSpawner.spawnPoint.y) {
			GameObject railInstance = Instantiate(railObject, transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity);
			next = railInstance.GetComponent<Rail>();
		}

		if (transform.position.y <= RailSpawner.destroyPoint.y) {
			Destroy(gameObject);
		}
	}

	public bool IsHead () {
		// Thead car won't have a next rail
		return next == null;
	}

	public bool HasPath () {
		if (next == null) {
			return this is DeadEndRail;
		}

		return next.HasPath();
	}
}
