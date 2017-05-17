using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeImage : MonoBehaviour {

	private Image image;

	void Start () {
		image = GetComponent<Image>();

		FadeOut();
	}

	public void FadeOut () {
		image.DOFade(0, 0.75f).SetEase(Ease.OutQuad);
	}

	public void FadeIn () {
		image.DOFade(1, 0.75f).SetEase(Ease.OutQuad);
	}

}
