using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Rail : MonoBehaviour {

	public Rail next;
	protected SpriteRenderer spriteRenderer;

	protected RailSpawner railSpawner;

	void Start () {
		railSpawner = GameObject.FindWithTag("GameController").GetComponent<RailSpawner>();

		gameObject.name = "rail";
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		// Highlights the player's current rail in red
		if (railSpawner.player.currentRail == this) {
			spriteRenderer.color = Color.red;
		} else {
			spriteRenderer.color = Color.white;
		}

		transform.Translate(Vector2.up * -railSpawner.speed * Time.deltaTime);

		if (next == null && transform.position.y <= railSpawner.spawnPoint.y) {
			next = SpawnRail();
		}

		if (transform.position.y <= railSpawner.destroyPoint.y) {
			Destroy(gameObject);
			railSpawner.RemoveRail(this);
		}
	}

	protected virtual Rail SpawnRail (Vector3 position) {
		if (ShouldSpawnBranchRight()) {
			return SpawnRail(position, railSpawner.branchRight);
		} else if (ShouldSpawnBranchLeft()) {
			return SpawnRail(position, railSpawner.branchLeft);
		} else if (ShouldSpawnDeadEnd()) {
			return SpawnRail(position, railSpawner.deadEnd);
		}

		return SpawnRail(position, railSpawner.rail);
	}

	protected virtual Rail SpawnRail (Vector3 position, GameObject railTypeToSpawn) {
		GameObject railInstance = Instantiate(railTypeToSpawn, transform.position + position, Quaternion.identity);

		Rail nextRail = railInstance.GetComponent<Rail>();
		railSpawner.AddRail(nextRail);

		return nextRail;
	}

	protected bool ShouldSpawnDeadEnd () {
		// Rails that branch away from the player cannot be reached by them, so they should have a higher chance of becoming dead ends
		bool isInPlay = railSpawner.player.currentRail.CanRailBeReached(this);

		// Prevents rails from branching at inconvenien times where a crazy fast reaction time is required
		Rail closestBranch = railSpawner.player.GetClosestBranch();
		float closestBranchDist = 5;
		if (closestBranch != null) {
			closestBranchDist = closestBranch.transform.position.y;			
		}

		if ((Random.value >= 0.2 && railSpawner.player.currentRail.GetPathCount() >= 2) || (!isInPlay && Random.value >= 0.1)) {
			// TODO generalize for all branch types
			if (!(this is BranchRail) && closestBranchDist >= 2) {
				return true;
			}
		}

		return false;
	}

	protected bool ShouldSpawnBranchRight () {
		return Random.value >= 0.75 && !IsARailToRight();
	}

	protected bool ShouldSpawnBranchLeft () {
		return Random.value >= 0.75 && !IsARailToLeft();
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
		return SpawnRail(new Vector3(0, 1f, 0));
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

	public Rail GetClosestBranch () {
		if (this is BranchRail) {
			return this;
		}

		if (next == null) {
			return null;
		}

		return next.GetClosestBranch();
	}

	public bool Intersects (Bounds bounds) {
		// TODO why is spriterenderer null sometimes?
		if (spriteRenderer == null) {
			return false;
		}

		return spriteRenderer.bounds.Intersects(bounds);
	}

	public bool IsARailToRight () {
		return IsARailToDir(1);
	}

	public bool IsARailToLeft () {
		return IsARailToDir(-1);
	}

	private bool IsARailToDir (int dir) {
		return GetRailToDir(dir) != null;
	}

	public Rail GetRailToRight () {
		return GetRailToDir(1);
	}

	public Rail GetRailToLeft () {
		return GetRailToDir(-1);
	}

	private Rail GetRailToDir (int dir) {
		Vector2 pos = new Vector2(transform.position.x, transform.position.y);
		Vector2 offset = new Vector2(dir / 2.0f, 0.75f);

		Collider2D[] collisions = Physics2D.OverlapCircleAll(pos + offset, 0.25f);

		// Draws a small line showing where the test point is
		// Debug.DrawLine(pos + offset, pos + offset + new Vector2(0.25f, 0.0f), Color.red, 0.25f);

		if (collisions.Length > 0) {
			return collisions[0].gameObject.GetComponent<Rail>();
		}

		return null;
	}

	public virtual float GetX () {
		return transform.position.x;
	}
}
