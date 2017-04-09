using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour {

	protected Rail next;
	protected SpriteRenderer spriteRenderer;

	void Start () {
		gameObject.name = "rail";
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		transform.Translate(Vector2.up * -RailSpawner.speed * Time.deltaTime);

		if (next == null && transform.position.y <= RailSpawner.spawnPoint.y) {
			next = SpawnRail();
		}

		if (transform.position.y <= RailSpawner.destroyPoint.y) {
			Destroy(gameObject);
			RailSpawner.RemoveRail(this);
		}
	}

	protected virtual Rail SpawnRail (Vector3 position) {
		GameObject railTypeToSpawn = RailSpawner.rail;

		// Rails that branch away from the player cannot be reached by them, so they should have a higher chance of becoming dead ends
		bool isInPlay = RailSpawner.player.currentRail.CanRailBeReached(this);

		// TODO spawn dead ends more often on paths that the player can never go down, and don't branch on those paths

		if (Random.value >= 0.8 && !IsARailToRight()) {
			railTypeToSpawn = RailSpawner.branchRight;
		} else if (Random.value >= 0.4 && RailSpawner.player.currentRail.GetPathCount() > 2 || !isInPlay) {
			railTypeToSpawn = RailSpawner.deadEnd;
		}

		GameObject railInstance = Instantiate(railTypeToSpawn, transform.position + position, Quaternion.identity);

		Rail nextRail = railInstance.GetComponent<Rail>();
		RailSpawner.AddRail(nextRail);

		return nextRail;
	}

	public virtual bool CanRailBeReached (Rail rail) {
		if (this == rail) {
			return true;
		}

		if (next == null) {
			return false;
		}

		return next.CanRailBeReached(rail);
	}

	protected virtual Rail SpawnRail () {
		return SpawnRail(new Vector3(0, 1.25f, 0));
	}

	public bool IsHead () {
		// The head car won't have a next rail
		return next == null;
	}

	public virtual bool HasPath () {
		if (next == null) {
			return true;
		}

		return next.HasPath();
	}

	public virtual int GetPathCount () {
		if (IsHead()) {
			return 1;
		}

		return next.GetPathCount();
	}

	public Rail GetNext () {
		return next;
	}

	public bool Intersects (Bounds bounds) {
		// TODO why is spriterenderer null sometimes?
		if (spriteRenderer == null) {
			return false;
		}

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

	public virtual float GetX () {
		return transform.position.x;
	}
}
