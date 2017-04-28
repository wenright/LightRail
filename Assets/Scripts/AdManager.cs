using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {

	public static int numTimesPlayed = 0;

	void Awake () {
		// TODO should this be in a playerprefs dict?
		numTimesPlayed++;

		if (numTimesPlayed % 3 == 0) {
			PlayAd();
		}
	}

	private void PlayAd () {
		if (Advertisement.IsReady()) {
			Advertisement.Show();
		}
	}
	
}
