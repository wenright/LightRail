using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchRightRail : Rail {

	protected override void SpawnRail () {
		base.SpawnRail();
		base.SpawnRail(new Vector3(0.625f, 1.25f, 0));
	}

}
