using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scenes.MainMenu.UIElements;

namespace Assets.Scenes.MainMenu {
	public class MainMenuScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			InitializePanelsAndButtons();
			AddEventsToButtons();
		}

		public GameObject SelectGameContent;

		private void InitializePanelsAndButtons() {
			Panels = new GameObject[transform.childCount];
			Buttons = new GameObject[Panels.Length][];


			for (int i = 0; i < Panels.Length; i++) {
				Panels[i] = transform.GetChild(i).gameObject;

				if (i == 1) { continue; }

				Buttons[i] = new GameObject[Panels[i].transform.childCount];
				for (int j = 0; j < Panels[i].transform.childCount; j++) {
					Buttons[i][j] = Panels[i].transform.GetChild(j).gameObject;
				}
			}

			DisplayAndInitializeGames();
		}

		#endregion

		#region Select panel

		public GameObject PanelPrefab;
		public Sprite[] SceneImages;
		public GameObject BackButton;
		private const int AmountInRow = 3;
		private const float AspectRatio = 3 / 4f;

		private void DisplayAndInitializeGames() {
			int VerticalQuantity = (int)Math.Ceiling(MainFunctions.Games.Length / (float)AmountInRow);
			float Height = VerticalQuantity * GetComponent<RectTransform>().rect.width / AmountInRow * AspectRatio;
			RectTransform RectTransform = SelectGameContent.GetComponent<RectTransform>();
			RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, Height);

			Buttons[1] = new GameObject[MainFunctions.Games.Length + 1];

			for (int i = 0; i < MainFunctions.Games.Length; i++) {
				GameObject Game = Instantiate(PanelPrefab, SelectGameContent.transform);

				Game.name = MainFunctions.Games[i];

				RectTransform PanelRectTransform = Game.GetComponent<RectTransform>();
				int v = VerticalQuantity - i / AmountInRow;
				PanelRectTransform.anchorMin = new Vector2(i % AmountInRow / (float)AmountInRow, (float)(v - 1) / VerticalQuantity);
				PanelRectTransform.anchorMax = new Vector2((i % AmountInRow + 1) / (float)AmountInRow, (float)v / VerticalQuantity);

				if (i < SceneImages.Length) {
					GameObject SceneImage = Game.transform.GetChild(0).gameObject;
					Image Image = SceneImage.GetComponent<Image>();
					Image.sprite = SceneImages[i];
				}

				GameObject NameScene = Game.transform.GetChild(1).gameObject;
				Text Name = NameScene.GetComponent<Text>();
				Name.text = MainFunctions.Games[i].ToNormalCase();

				Buttons[1][i] = Game;
			}

			Buttons[1][Buttons[1].Length - 1] = BackButton;
		}


		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			CheckKeystroke();
		}

		private void CheckKeystroke() {
			if (Input.GetKeyDown(KeyCode.Backspace)) {
				OnButtonClick("Back");
			}
		}

		#endregion
	}
}
