using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour {

	private float minWaitTime = 10.0f;
	private float maxWaitTime = 35.0f;
	private GameObject circle;

	private Color[] colors = new Color[] {
		new Color(142 / 255.0f, 68 / 255.0f, 173 / 255.0f),
		new Color(44 / 255.0f, 62 / 255.0f, 80 / 255.0f),
		new Color(41 / 255.0f, 128 / 255.0f, 185 / 255.0f),
		new Color(41 / 255.0f, 128 / 255.0f, 185 / 255.0f),
		new Color(192 / 255.0f, 57 / 255.0f, 43 / 255.0f)
	};

	void Start () {
		circle = Resources.Load("Circle") as GameObject;

		Invoke("SpawnCircle", Random.Range(minWaitTime, maxWaitTime));
	}

	private void SpawnCircle () {
		GameObject circleInstance = Instantiate(circle, new Vector3(transform.position.x, 13, 0), Quaternion.identity) as GameObject;
		circleInstance.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Length	)];
		Invoke("SpawnCircle", Random.Range(minWaitTime, maxWaitTime));
	}

}
