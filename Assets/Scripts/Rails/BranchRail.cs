using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BranchRail : Rail {

	public bool branchRight = false;

	protected Rail branchedRail;

	private LineRenderer curveRenderer;
	private SpriteRenderer straightRailRenderer;

	void Awake () {
		curveRenderer = transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
		straightRailRenderer = GetComponent<SpriteRenderer>();

		curveRenderer.material.DOFade(0.25f, 0.0f);
	}

	protected override Rail SpawnRail () {
		next = base.SpawnRail();

		int sign = -1;
		if (branchRight) {
			sign = 1;
		}

		branchedRail = SpawnRail(new Vector3(sign * 0.5f, 1f, 0));
		return next;
	}

	protected override Rail SpawnRail (Vector3 position) {
		GameObject railTypeToSpawn = railSpawner.rail;

		GameObject railInstance = Instantiate(railTypeToSpawn, transform.position + position, Quaternion.identity);

		Rail nextRail = railInstance.GetComponent<Rail>();
		railSpawner.AddRail(nextRail);

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
		if (railSpawner.player.WillTakeBranch()) {
			int sign = -1;
			if (branchRight) {
				sign = 1;
			}

			float percentage = transform.position.y + 0.625f;

			percentage = Mathf.Clamp(percentage, 0, 1);

			return base.GetX() + sign * 0.625f * Mathf.Cos(percentage * (Mathf.PI / 2));
		} else {
			return base.GetX();
		}
	}

	// Fades the straight branch and brings back the curve branch to make it more clear which branch will be taken
	public void SwapAlphas () {
		curveRenderer.material.DOFade(1.0f, 0.25f).SetEase(Ease.OutQuad);
		straightRailRenderer.DOFade(0.25f, 0.25f).SetEase(Ease.InQuad);
	}

}
