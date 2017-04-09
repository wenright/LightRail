using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour {

	private Rail next;
	private SpriteRenderer spriteRenderer;

	void Start () {
		gameObject.name = "rail";
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		transform.Translate(Vector2.up * -RailSpawner.speed * Time.deltaTime);

		if (next == null && transform.position.y <= RailSpawner.spawnPoint.y) {
			SpawnRail();
		}

		if (transform.position.y <= RailSpawner.destroyPoint.y) {
			Destroy(gameObject);
			RailSpawner.RemoveRail(this);
		}
	}

	protected virtual void SpawnRail (Vector3 position) {
		GameObject railTypeToSpawn = RailSpawner.rail;

		if (Random.value >= 0.8 && !IsARailToRight()) {
			railTypeToSpawn = RailSpawner.branchRight;
		}

		GameObject railInstance = Instantiate(railTypeToSpawn, transform.position + position, Quaternion.identity);
		next = railInstance.GetComponent<Rail>();
		RailSpawner.AddRail(next);
	}

	protected virtual void SpawnRail () {
		SpawnRail(new Vector3(0, 1.25f, 0));
	}

	public bool IsHead () {
		// The head car won't have a next rail
		return next == null;
	}

	public bool HasPath () {
		if (next == null) {
			return this is DeadEndRail;
		}

		return next.HasPath();
	}

	public Rail GetNext () {
		return next;
	}

	public bool Intersects (Bounds bounds) {
		return spriteRenderer.bounds.Intersects(bounds);
	}

	public bool IsARailToRight () {
		Bounds bounds = new Bounds(spriteRenderer.bounds.center + new Vector3(0.625f, 0, 0), spriteRenderer.bounds.size);

		foreach (Rail rail in RailSpawner.rails) {
			if (rail.Intersects(bounds)) {
				return true;
			}
		}

		return false;
	}
}
