using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.MainMenu.Select {
	public class DisplayGames : MonoBehaviour {

		public GameObject PanelPrefab;
		public Sprite[] SceneImages;
		public GameObject ScrollContent;

		[UsedImplicitly]
		private void Start() {
			int VerticalQuantity = (int)Math.Ceiling(MainFunctions.Games.Length / 3f);
			float Height = VerticalQuantity * GameObject.Find("Canvas").GetComponent<RectTransform>().rect.height / 2;
			RectTransform RectTransform = GetComponent<RectTransform>();
			RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, Height);

			for (int i = 0; i < MainFunctions.Games.Length; i++) {
				GameObject Panel = Instantiate(PanelPrefab, transform);

				RectTransform PanelRectTransform = Panel.GetComponent<RectTransform>();
				int v = VerticalQuantity - i / 3;
				PanelRectTransform.anchorMin = new Vector2(i % 3 / 3f, (float)(v - 1) / VerticalQuantity);
				PanelRectTransform.anchorMax = new Vector2((i % 3 + 1) / 3f, (float)v / VerticalQuantity);

				if (i < SceneImages.Length) {
					GameObject SceneImage = Panel.transform.GetChild(0).gameObject;
					Image Image = SceneImage.GetComponent<Image>();
					Image.sprite = SceneImages[i];
				}

				GameObject NameScene = Panel.transform.GetChild(1).gameObject;
				Text Name = NameScene.GetComponent<Text>();
				Name.text = MainFunctions.Games[i];
			}
		}

	}
}
