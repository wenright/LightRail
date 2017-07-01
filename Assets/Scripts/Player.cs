using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour {

	public Rail currentRail;
	public Rail nextRail;
	public bool willTakeRightBranch = false;
	public bool willTakeLeftBranch = false;
	public GooglePlay googlePlayController;
	public GameObject uiPanel;
	public Text gameOverScoreText;
	public GameObject explosion;
	public ParticleSystem stars;

	// TODO there should be a game controller that deals with gameOver states
	public bool gameOver = false;

	private RailSpawner railSpawner;

	// Use this for initialization
	void Start () {
		railSpawner = GameObject.FindWithTag("GameController").GetComponent<RailSpawner>();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentRail == null) {
			// This shouldn't ever happen, but just in case it should be dealt with
			return;
		}

		// Transfer player to next rail
		if (currentRail.transform.position.y <= -0.2f) {
			nextRail = currentRail.GetNext();

			if (nextRail is BranchRail && WillTakeBranch(nextRail)) {
				TweenRotation();
			}

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

		#if UNITY_EDITOR
			if (Input.GetKeyDown("right")) {
				Swipe(SwipeDetection.Directions.Right);
			} else if (Input.GetKeyDown("left")) {
				Swipe(SwipeDetection.Directions.Left);
			}
		#endif
	}

	// TODO Once 3 directional branches are added, swipe direction will become important
	public void Swipe (SwipeDetection.Directions direction) {
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
			if (direction == SwipeDetection.Directions.Left && !branchRail.branchRight) {
				willTakeLeftBranch = true;
				branchRail.SwapAlphas();				
			} else if (direction == SwipeDetection.Directions.Right && branchRail.branchRight) {
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

	public bool WillTakeBranch (Rail railToTest) {
		return ((railToTest as BranchRail).branchRight && willTakeRightBranch) || (!(railToTest as BranchRail).branchRight && willTakeLeftBranch);
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

	private void TweenRotation () {
		float rotateTime = (1 / railSpawner.speed) / 1.5f;
		int dir = 1;

		if ((nextRail as BranchRail).branchRight) {
			dir = -1;
		}

		transform.DORotate(new Vector3(0, 0, 30 * dir), rotateTime).SetEase(Ease.OutCirc)
			.OnComplete(() => transform.DORotate(Vector3.zero, rotateTime).SetEase(Ease.OutCirc));
	}
}
