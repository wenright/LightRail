using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEndRail : Rail {

	protected override Rail SpawnRail () {
		return null;
	}

	public override bool HasPath () {
		return false;
	}

	public override int GetPathCount () {
		return 0;
	}

}
