using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Rail : MonoBehaviour {

	public Rail next;
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
		// TODO If player does nothing, they never lose. Dead ends are never spawned in the path of the player.

		GameObject railTypeToSpawn = RailSpawner.rail;

		// Rails that branch away from the player cannot be reached by them, so they should have a higher chance of becoming dead ends
		bool isInPlay = RailSpawner.player.currentRail.CanRailBeReached(this);

		Rail closestBranch = RailSpawner.player.GetClosestBranch();
		float closestBranchDist = 5;
		if (closestBranch != null) {
			closestBranchDist = closestBranch.transform.position.y;			
		}

		if (Random.value >= 0.5) {
			// TODO branching rails can merge into eachother, cuasing other bugs if there is a blank rail in the middle surrounded by two inward branches:
			// 	|/ \|
			// 	|  |
			// Seems like branch detection is off. Similar thing happened, but was avoided by restricting branches from spawning more branches 
			if (Random.value >= 0.5) {
				if (!IsARailToRight()) {
					railTypeToSpawn = RailSpawner.branchRight;
				}
			} else {
				if (!IsARailToLeft()) {
					railTypeToSpawn = RailSpawner.branchLeft;
				}
			}
		} 

		if (Random.value >= 0.2 && RailSpawner.player.currentRail.GetPathCount() > 2 || !isInPlay) {
			// TODO generalize for all branch types
			if (!(this is BranchRail) && closestBranchDist >= 2) {
				railTypeToSpawn = RailSpawner.deadEnd;
			}
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
		return SpawnRail(new Vector3(0, 1.5f, 0));
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
		return Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y) + new Vector2(dir * 0.625f, 0), 0.25f).Length > 0;
	}

	public virtual float GetX () {
		return transform.position.x;
	}
}
