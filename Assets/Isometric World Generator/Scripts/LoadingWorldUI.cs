using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadingWorldUI : MonoBehaviour {
	public GameObject loadingScreen;
	[HideInInspector] public GameObject instance = null;
	private float width = 0;

	void Start() {
	}

	public void SetUp() {
		if (instance == null) {
			instance = Instantiate (loadingScreen, new Vector3 (0, 0, 0), Quaternion.identity);
		}
		if (!instance.activeSelf) {
			instance.SetActive (true);
		}
	}

	public void Discard() {
		instance.SetActive (false);
	}

	public void UpdateProgress(int progress, string progressText) {
		GameObject loading = GameObject.Find ("LoadingBar");

		if (loading == null) {
			SetUp ();
		} else {
			GameObject loadingText = GameObject.Find ("DetailLoadingLabel");
			loadingText.GetComponent<Text> ().text = progressText;

			RectTransform loadingRect = loading.GetComponent<RectTransform> (); 
			if (width == 0) {
				width = -loadingRect.rect.width;
			}
			loadingRect.offsetMax = new Vector2 (width + ((-width / 100) * progress), loadingRect.offsetMax.y);
		}
	}
}
