using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchRightRail : Rail {

	protected Rail branchedRail;

	protected override Rail SpawnRail () {
		next = base.SpawnRail();
		branchedRail = base.SpawnRail(new Vector3(0.625f, 1.25f, 0));
		return next;
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
		if (RailSpawner.player.willTakeBranch) {
			return base.GetX() - 0.625f * ((transform.position.y - 1.5f) / 1.5f);
		} else {
			return base.GetX();
		}
	}

}
