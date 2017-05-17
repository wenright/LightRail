using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour {

	public Rail currentRail;
	public bool willTakeRightBranch = false;
	public bool willTakeLeftBranch = false;
	public GooglePlay googlePlayController;
	public GameObject uiPanel;
	public Text gameOverScoreText;
	public ParticleSystem explosion;
	public ParticleSystem stars;

	// TODO move this into a different script that controls rotation
	private float lastPosX;

	// TODO there should be a game controller that deals with gameOver states
	public bool gameOver = false;

	private RailSpawner railSpawner;

	// TODO remove these if using tapping instead of swiping
	private enum Directions {Left, Right};
	private Directions direction;

	// Use this for initialization
	void Start () {
		railSpawner = GameObject.FindWithTag("GameController").GetComponent<RailSpawner>();

		lastPosX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentRail == null) {
			// This shouldn't ever happen, but just in case it should be dealt with
			return;
		}

		// Transfer player to next rail
		if (currentRail.transform.position.y <= -0.5f) {
			Rail nextRail = currentRail.GetNext();

			if (nextRail is DeadEndRail) {
				if (!gameOver) {
					GameOver();
				}
			} else {
				Rail branchToTake = nextRail;

				if (currentRail is BranchRail) {
					if (WillTakeBranch()) {
						branchToTake = (currentRail as BranchRail).GetBranchedRail();

						willTakeLeftBranch = false;
						willTakeRightBranch = false;
					}
				}

				currentRail = branchToTake;
			}
		}

		transform.position = new Vector3(currentRail.GetX(), 0, 0);

		// Rotate player so that they follow the path of the rail
		// TODO branch rails still have some jerky parts at the beginning and the end
		float vx = lastPosX - transform.position.x;
		if (Mathf.Abs(vx) >= 0.000001f) {
			float rotZ = -Mathf.Atan2(railSpawner.speed * Time.deltaTime, vx);
			float rotationOffset = 90.0f;
			transform.rotation = Quaternion.Euler(0, 0, rotZ * Mathf.Rad2Deg - rotationOffset);
			lastPosX = transform.position.x;
		} else {
			transform.rotation = Quaternion.identity;
		}

		if (Input.GetMouseButtonUp(0)) {
			Swipe(Directions.Left);
			Swipe(Directions.Right);
		}
	}

	// TODO Once 3 directional branches are added, swipe direction will become important
	private void Swipe (Directions direction) {
		BranchRail branchRail = null;

		// TODO should we allow branching if the player is already on the branch rail? Or maybe only partway down the rail
		if (currentRail is BranchRail && currentRail.transform.position.y >= -0.1f) {
			// TODO maybe add a spark particle system here since the train needs to jump from the straight rail to the branched one
			branchRail = currentRail as BranchRail;

			// TODO the rotation animation is jumpy when switching mid branch
		} else if (currentRail.next is BranchRail) {
			branchRail = currentRail.next as BranchRail;
		}

		// TODO Allow undoing of swipes (Swipe right, then swipe left before reaching branch to go straight)
		// TODO how far along should changing be allowed? up to two rails away? and can you change on the current rail?
		if (branchRail) {
			if (direction == Directions.Left && !branchRail.branchRight) {
				willTakeLeftBranch = true;
				branchRail.SwapAlphas();				
			} else if (direction == Directions.Right && branchRail.branchRight) {
				willTakeRightBranch = true;
				branchRail.SwapAlphas();				
			}
		}
	}

	public Rail GetClosestBranch () {
		return currentRail.GetClosestBranch();
	}

	public bool WillTakeBranch () {
		return ((currentRail as BranchRail).branchRight && willTakeRightBranch) || (!(currentRail as BranchRail).branchRight && willTakeLeftBranch);
	}

	private void GameOver () {
		gameOver = true;

		railSpawner.speed = 0.0f;
		// Tween the railspawner speed so that it smoothly goes to 0 after 0.5 seconds
		// DOTween.To(() => railSpawner.speed, x => railSpawner.speed = x, 0, 0.5f).SetEase(Ease.OutQuad);

		if (railSpawner.score > railSpawner.highScore) {
			PlayerPrefs.SetFloat("highscore", railSpawner.score);

			// Upload high score
			googlePlayController.UploadScore((int) railSpawner.score);
		}

		Invoke("TweenInUI", 1.0f);

		Instantiate(explosion, transform.position, Quaternion.identity);

		// Hide player sprite. Alternatively, destroy this object. But then tween in won't be called
		GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);

		// Disable the star particle system and slow down each individual particle
		var emission = stars.emission;
		emission.enabled = false;

		var limitVel = stars.limitVelocityOverLifetime;
		limitVel.enabled = true;
		limitVel.limit = 0.0f;
		limitVel.dampen = 0.1f;
	}

	private void TweenInUI () {
		// Tween in the game over ui panel
		uiPanel.SetActive(true);

		RectTransform rectTransform = uiPanel.GetComponent<RectTransform>();
		rectTransform.DOAnchorPos(Vector2.zero, 0.5f, false).SetEase(Ease.OutQuad);

		gameOverScoreText.text = ((int) railSpawner.score).ToString();
	}
}
