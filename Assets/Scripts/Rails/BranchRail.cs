using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchRail : Rail {

	public bool branchRight = false;

	protected Rail branchedRail;

	protected override Rail SpawnRail () {
		next = base.SpawnRail();

		int sign = -1;
		if (branchRight) {
			sign = 1;
		}

		branchedRail = SpawnRail(new Vector3(0.625f * sign, 1.5f, 0));
		return next;
	}

	protected override Rail SpawnRail (Vector3 position) {
		GameObject railTypeToSpawn = RailSpawner.rail;

		GameObject railInstance = Instantiate(railTypeToSpawn, transform.position + position, Quaternion.identity);

		Rail nextRail = railInstance.GetComponent<Rail>();
		RailSpawner.AddRail(nextRail);

		return nextRail;
	}

	public override bool HasPath () {
		if (next == null || branchedRail == null) {
			return true;
		}

		return next.HasPath() || branchedRail.HasPath();
	}

	public override int GetPathCount () {
		int count = 0;

		// Next and branchedRail need to be treated separately because on may have spawned a dead end since the last call
		if (next == null) {
			count++;
		} else {
			count += next.GetPathCount();
		}

		if (branchedRail == null) {
			count++;
		} else {
			count += branchedRail.GetPathCount();
		}

		return count;
	}

	public override bool CanRailBeReached (Rail rail) {
		bool isDownBranchedRail = false;

		if (branchedRail != null) {
			isDownBranchedRail = branchedRail.CanRailBeReached(rail);
		}

		return base.CanRailBeReached(rail) || isDownBranchedRail;
	}

	public Rail GetBranchedRail () {
		return branchedRail;
	}

	public override float GetX () {
		if (RailSpawner.player.willTakeRightBranch || RailSpawner.player.willTakeLeftBranch) {
			int sign = -1;
			if (branchRight) {
				sign = 1;
			}

			// return base.GetX() - sign * 0.625f * ((transform.position.y - 1f) / 1.5f);

			float percentage = (transform.position.y + 0.75f) / 1.5f;

			percentage = Mathf.Clamp(percentage, 0, 1);

			return base.GetX() + sign * 0.625f * Mathf.Cos(percentage * (Mathf.PI / 2));
		} else {
			return base.GetX();
		}
	}

}
