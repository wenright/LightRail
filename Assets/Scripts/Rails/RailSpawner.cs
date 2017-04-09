using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSpawner : MonoBehaviour {

	public static Vector3 spawnPoint = new Vector3(0, 5, 0);
	public static Vector3 destroyPoint = new Vector3(0, -7, 0);
    public static float speed = 2f;
    public static GameObject rail;
    public static GameObject branchRight;

    private static float acceleration = 0.0f;

    public static List<Rail> rails;

    void Start () {
    	rails = new List<Rail>();

		rail = Resources.Load("rail") as GameObject;
		branchRight = Resources.Load("branchRight") as GameObject;
    }

    void Update () {
        speed += acceleration * Time.deltaTime;
    }

    // Add rail to rails list
    public static void AddRail (Rail obj) {
    	rails.Add(obj);
    }

    public static void RemoveRail (Rail obj) {
    	rails.Remove(obj);
    }
}
