using Assets.Scenes.Games.BaseScene;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.Scenes.MainMenu.MainMenuScript;

namespace Assets.Scenes.MainMenu.Select {
	public class DisplayGames : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			EventSystem = EventSystem.current.GetComponent<EventSystem>();
			DisplayAndInitializeGames();
			AddEventsToButtons();
		}

		public GameObject PanelPrefab;
		public Sprite[] SceneImages;
		public static int AmountInRow = 3;
		public GameObject BackButton;

		private void DisplayAndInitializeGames() {
			/*for (int i = 0; i < transform.childCount; i++)
				Destroy(transform.GetChild(i).gameObject);//*/

			const float AspectRatio = 3 / 4f;

			int VerticalQuantity = (int)Math.Ceiling(MainFunctions.Games.Length / (float)AmountInRow);
			float Height = VerticalQuantity * GameObject.Find("Canvas").GetComponent<RectTransform>().rect.width / AmountInRow * AspectRatio;
			RectTransform RectTransform = GetComponent<RectTransform>();
			RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, Height);

			if (Buttons == null)
				Buttons = new GameObject[GameObject.Find("Canvas").transform.childCount][];
			Buttons[1] = new GameObject[MainFunctions.Games.Length + 1];

			for (int i = 0; i < MainFunctions.Games.Length; i++) {
				GameObject Game = Instantiate(PanelPrefab, transform);

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

		private EventSystem EventSystem;

		private void AddEventsToButtons() {
			foreach (GameObject Game in Buttons[1])
				AddEvent(Game);

			void AddEvent(GameObject Game) {
				EventTrigger.Entry entry = new EventTrigger.Entry {
					eventID = EventTriggerType.PointerEnter
				};
				entry.callback.AddListener(PointerEventData => {
					EventSystem.SetSelectedGameObject(Game);
				});
				Game.AddComponent<EventTrigger>().triggers.Add(entry);
			}
		}

		#endregion

		#region Clicks

		[UsedImplicitly]
		public void Play() {
			ToNextScene.Save(new ToNextScene(0, GameMode.Mixed));
			MainFunctions.LoadRandomGame();
		}

		#endregion

	}
}
