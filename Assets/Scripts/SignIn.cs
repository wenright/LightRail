using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignIn : MonoBehaviour {

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

				// Wait a second before starting the game so that the welcome back screen doesn't get in the way
				Invoke("StartGame", 1.0f);
			});
		}

		#endif
	}

	private void StartGame () {
		SceneManager.LoadScene("game");
	}
	
}
