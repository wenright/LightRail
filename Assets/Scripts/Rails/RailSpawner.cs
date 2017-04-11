using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSpawner : MonoBehaviour {

	public static Vector3 spawnPoint = new Vector3(0, 5, 0);
	public static Vector3 destroyPoint = new Vector3(0, -7, 0);
    public static float speed = 3f;
    public static GameObject rail;
    public static GameObject branchRight;
    public static GameObject branchLeft;
    public static GameObject deadEnd;

    public UnityEngine.UI.Text scoreText;
    public float score = 0.0f;

    public static Player player;

    private static float acceleration = 0.1f;

    public static List<Rail> rails;

    void Start () {
    	// TODO remove this for actual game. Useful for debugging though
        // Random.InitState(321123);

    	rails = new List<Rail>();

    	GameObject playerObject = GameObject.FindWithTag("Player");
    	player = playerObject.GetComponent<Player>();

		rail = Resources.Load("rail") as GameObject;
		branchRight = Resources.Load("branchRight") as GameObject;
        branchLeft = Resources.Load("branchLeft") as GameObject;
		deadEnd = Resources.Load("deadEnd") as GameObject;
    }

    void Update () {
    	score += speed * Time.deltaTime;
    	scoreText.text = ((int) score).ToString();

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
