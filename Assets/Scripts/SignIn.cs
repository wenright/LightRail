using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SignIn : MonoBehaviour {

	public UnityEngine.UI.Text signInText;

	void Awake () {
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
			});
		}

		#endif
	}

	private void StartGame () {
		SceneManager.LoadScene("game");
	}
	
}
