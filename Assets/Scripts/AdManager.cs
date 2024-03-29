﻿using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {

	private static int numTimesPlayed = 0;
	private const int numGamesBetweenAds = 7;
	private RailSpawner railSpawner;

	void Awake () {
		railSpawner = GameObject.FindWithTag("GameController").GetComponent<RailSpawner>();

		numTimesPlayed++;
	}

	public void PlayAd () {
		if (Advertisement.IsReady()) {
			ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show(options);
		}
	}

	#if UNITY_ADS
	private void HandleShowResult(ShowResult result) {
		// Could do something with the result later, but really it doesn't matter
		switch (result) {
			case ShowResult.Finished:
				break;
			case ShowResult.Skipped:
				break;
			case ShowResult.Failed:
				break;
		}

		railSpawner.ResetScene();
	}
	#endif

	public bool ShouldPlayAd () {
		return numTimesPlayed % numGamesBetweenAds == 0;
	}
	
}
