using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CurveGenerator : MonoBehaviour {

	private float height = 1.5f;
	private float width = 0.32f;
	private LineRenderer lineRenderer;

	void Start () {
		lineRenderer = GetComponent<LineRenderer>();

		int numPoints = 15;

		Vector3[] points = new Vector3[numPoints];

		for (int i = 0; i < points.Length; i++) {
			float percentage = ((float) i / (points.Length - 1));
			float y =  percentage * height;
			points[i] = new Vector3(Mathf.Cos(percentage * (Mathf.PI)) * width, y, 0);
		}

		lineRenderer.SetPositions(points);
	}

}
