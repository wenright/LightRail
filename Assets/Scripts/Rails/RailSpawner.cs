using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSpawner : MonoBehaviour {

	public static Vector3 spawnPoint = new Vector3(0, 5, 0);
	public static Vector3 destroyPoint = new Vector3(0, -5, 0);
    public static float speed = 0.5f;
    private static float acceloration = 0.0f;

    void Update () {
        speed += acceloration * Time.deltaTime;
    }
}
