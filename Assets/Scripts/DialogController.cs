using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogController : MonoBehaviour {

	void Start () {
		if (PlayerPrefs.HasKey("UseGPG") || PlayerPrefs.HasKey("HighQuality")) {
			StartGame();
		} else {
			ShowGPGAlert();
		}
	}	

	private void ShowGPGAlert () {
		string title = "Enable Google Play Games services";
		string message = "This will enable you to view and submit scores to a global leaderboard";
		string[] buttonsArray = new string[] { "Ok", "Cancel"};

		NPBinding.UI.ShowAlertDialogWithMultipleButtons(title, message, buttonsArray, ApplyGPGDecision); 
	}

	private void ApplyGPGDecision (string _buttonPressed) {
		if (_buttonPressed == "Ok") {
			PlayerPrefs.SetInt("UseGPG", 1);
		}

		// Move on to next dialog
		ShowQualityAlert();
	}

	private void ShowQualityAlert () {
		string title = "Enable high quality graphics";
		string message = "Enabling HQ graphics may cause poor performance or battery life on older devices";
		string[] buttonsArray = new string[] { "Ok", "Cancel"};

		NPBinding.UI.ShowAlertDialogWithMultipleButtons(title, message, buttonsArray, ApplyQualityDecision); 
	}

	private void ApplyQualityDecision (string _buttonPressed) {
		if (_buttonPressed == "Ok") {
			PlayerPrefs.SetInt("HighQuality", 1);
		}
	}

	private void StartGame () {
		SceneManager.LoadScene("game");
	}
}
