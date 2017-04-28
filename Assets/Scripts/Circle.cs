using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {

	private RailSpawner railSpawner;

	void Start () {
		railSpawner = GameObject.FindWithTag("GameController").GetComponent<RailSpawner>();
	}

	void Update () {
		transform.Translate(Vector2.up * -railSpawner.speed * Time.deltaTime);

		if (transform.position.y <= 0) {
			Camera.main.backgroundColor = GetComponent<SpriteRenderer>().color;
			Destroy(gameObject);
		}
	}
}
