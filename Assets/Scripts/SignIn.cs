using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SignIn : MonoBehaviour {

	public Text signInText;
	public Transform spinnerImage;
	public Sprite checkSprite;

	void Awake () {
		// Rotate spinner indefinitely
		spinnerImage.DORotate(new Vector3(0, 0, -350), 1.0f, RotateMode.WorldAxisAdd)
			.SetEase(Ease.Linear)
			.SetLoops(-1);

		#if UNITY_ANDROID

		// Sign user in
		if (!Social.localUser.authenticated) {
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();
			
			PlayGamesPlatform.Instance.Authenticate((bool success) => {
				if (success) {
					Debug.Log("Successfully signed in");
				} else {
					Debug.LogError("Failed signing in!");
				}

				// Wait a second or so to start the game so that the welcome message doesn't cover up incoming dead ends
				signInText.DOFade(0, 0.5f)
					.SetEase(Ease.OutQuad)
					.OnComplete(() => StartGame());

				DOTween.Pause(spinnerImage);
				spinnerImage.rotation = Quaternion.identity;

				Image newImage = spinnerImage.gameObject.GetComponent<Image>();

				newImage.overrideSprite = checkSprite;

				newImage.DOFade(0, 0.5f)
					.SetEase(Ease.OutQuad);
			});
		}

		#endif
	}

	private void StartGame () {
		SceneManager.LoadScene("game");
	}
	
}
