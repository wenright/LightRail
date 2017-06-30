using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RailSweepIn : MonoBehaviour {
	void Start () {
		Vector3 finishPosition = transform.position;
		float offsetDistance = 25.0f;
		transform.position += new Vector3(Random.value, Random.value, Random.value).normalized * offsetDistance;

		float comebackTime = 1.0f;
		transform.DOMove(finishPosition, comebackTime).SetEase(Ease.InOutQuad);
	}
}
