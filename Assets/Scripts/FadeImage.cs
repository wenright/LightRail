using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeImage : MonoBehaviour {

	private Image image;

	void Start () {
		image = GetComponent<Image>();

		// Reset fill transparency so that it can be invisible to start
		Color color = image.color;
		color.a = 1.0f;
		image.color = color;

		FadeOut();
	}

	public void FadeOut () {
		image.DOFade(0, 0.75f).SetEase(Ease.OutQuad);
	}

	public void FadeIn () {
		image.DOFade(1, 0.75f).SetEase(Ease.OutQuad);
	}

}
