using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Toast : MonoBehaviour {

	public Text titleText;
	public SpriteRenderer background;

	private float displayTime = 1.0f;

	public void ShowToast (string title) {
		titleText.text = title;

		FadeIn ();
	}

	private void FadeIn () {
		background.DOFade(1, 0.75f).SetEase(Ease.OutQuad);
		titleText.DOFade(1, 0.75f).SetEase(Ease.OutQuad);

		Invoke("FadeOut", displayTime);
	}

	private void FadeOut () {
		background.DOFade(0, 0.75f).SetEase(Ease.OutQuad);
		titleText.DOFade(0, 0.75f).SetEase(Ease.OutQuad);
	}
}
