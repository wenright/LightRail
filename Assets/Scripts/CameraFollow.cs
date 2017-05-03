using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float smoothTime = 0.5f;

	private Vector3 velocity = Vector3.zero;
	private Vector3 offset;

	void Start () {
		offset = transform.position - target.position;
	}

	void Update () {
		if (target != null) {
			transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, smoothTime);			
		}
	}
}
