using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.MainMenu.Select {
	public class DisplayGames : MonoBehaviour {

		public GameObject PanelPrefab;
		public Sprite[] SceneImages;
		public static int AmountInRow = 3;

		[UsedImplicitly]
		private void Start() {
			const float AspectRatio = 3 / 4f;

			int VerticalQuantity = (int)Math.Ceiling(MainFunctions.Games.Length / (float)AmountInRow);
			float Height = VerticalQuantity * GameObject.Find("Canvas").GetComponent<RectTransform>().rect.width / AmountInRow * AspectRatio;
			RectTransform RectTransform = GetComponent<RectTransform>();
			RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, Height);

			for (int i = 0; i < MainFunctions.Games.Length; i++) {
				GameObject Panel = Instantiate(PanelPrefab, transform);

				RectTransform PanelRectTransform = Panel.GetComponent<RectTransform>();
				int v = VerticalQuantity - i / AmountInRow;
				PanelRectTransform.anchorMin = new Vector2(i % AmountInRow / (float)AmountInRow, (float)(v - 1) / VerticalQuantity);
				PanelRectTransform.anchorMax = new Vector2((i % AmountInRow + 1) / (float)AmountInRow, (float)v / VerticalQuantity);

				if (i < SceneImages.Length) {
					GameObject SceneImage = Panel.transform.GetChild(0).gameObject;
					Image Image = SceneImage.GetComponent<Image>();
					Image.sprite = SceneImages[i];
				}

				GameObject NameScene = Panel.transform.GetChild(1).gameObject;
				Text Name = NameScene.GetComponent<Text>();
				Name.text = MainFunctions.Games[i].ToNormalCase();
			}
		}

	}
}
