using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.MainMenu {
	public class MainMenuScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			InitializeMenuButtons();
			AddEventsToButtons();
		}

		private static readonly string[][] ButtonNames = {
			// MainMenu
			new[] {
				"Play",
				"ChooseGame",
				"Settings",
				"Quit"
			},

			// ChooseGame
			null,

			// Settings
			new[] {
				"Screen",
				"Game",
				"Control",
				"Back"
			}
		};

		#region Initialization panels and buttons

		public GameObject ButtonPrefab;
		private static GameObject[] Panels;
		private static GameObject[][] Buttons;

		private void InitializeMenuButtons() {
			int PanelAmount = transform.childCount - 1;

			Panels = new GameObject[PanelAmount];
			Buttons = new GameObject[PanelAmount][];

			for (int i = 0; i < PanelAmount; ++i) {
				// Panel initializing
				Panels[i] = transform.GetChild(i + 1).gameObject; // + 1 'cause there's Background at 0 index

				// ChooseGame panel
				if (i == 1) {
					InitializeGameButtons();
					continue;
				}

				// Remove all existing buttons from the scene
				for (int j = 0; j < Panels[i].transform.childCount; ++j)
					Destroy(Panels[i].transform.GetChild(j).gameObject);

				// Set panel position and size
				float ScreenWidth = GetComponent<RectTransform>().rect.width;
				RectTransform PanelRectTransform = Panels[i].GetComponent<RectTransform>();
				float PanelWidth = PanelRectTransform.rect.height / ButtonNames[i].Length * 0.82f * 4.2f;
				PanelRectTransform.anchorMin = new Vector2((ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMin.y);
				PanelRectTransform.anchorMax = new Vector2(1 - (ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMax.y);

				// Button creating
				Buttons[i] = new GameObject[ButtonNames[i].Length];
				for (int j = 0; j < ButtonNames[i].Length; ++j) {
					GameObject ButtonWrapper = Instantiate(ButtonPrefab, Panels[i].transform);
					ButtonWrapper.name = ButtonNames[i][j] + "Wrapper";

					// Set position and size
					RectTransform ButtonRectTransform = ButtonWrapper.GetComponent<RectTransform>();
					ButtonRectTransform.anchorMin = new Vector2(0, (ButtonNames[i].Length - j - 1) / (float)ButtonNames[i].Length);
					ButtonRectTransform.anchorMax = new Vector2(1, (ButtonNames[i].Length - j) / (float)ButtonNames[i].Length);

					Buttons[i][j] = ButtonWrapper.transform.GetChild(0).gameObject;
					Buttons[i][j].name = ButtonNames[i][j];

					// Set text
					Buttons[i][j].transform.GetChild(0).GetComponent<Text>().text = ButtonNames[i][j].ToNormalCase();
				}
			}
		}

		public GameObject GameButtonPrefab;
		public Sprite[] SceneImages;
		public GameObject ChooseGameContent;
		public GameObject ChooseGameBack;
		private const int AmountInRow = 3;
		private const float AspectRatio = 3 / 4f;

		private void InitializeGameButtons() {
			// Remove all buttons from the scene
			for (int i = 0; i < ChooseGameContent.transform.childCount; ++i)
				Destroy(ChooseGameContent.transform.GetChild(i).gameObject);

			// Set size of the Content
			int VerticalQuantity = (int)Math.Ceiling(Functions.Games.Length / (float)AmountInRow);
			float Height = VerticalQuantity * GetComponent<RectTransform>().rect.width / AmountInRow * AspectRatio;
			RectTransform RectTransform = ChooseGameContent.GetComponent<RectTransform>();
			RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, Height);

			Buttons[1] = new GameObject[Functions.Games.Length + 1];
			for (int i = 0; i < Functions.Games.Length; ++i) {
				GameObject Game = Instantiate(GameButtonPrefab, ChooseGameContent.transform);
				Game.name = Functions.Games[i] + "Wrapper";

				// Set button position and size
				RectTransform PanelRectTransform = Game.GetComponent<RectTransform>();
				int v = VerticalQuantity - i / AmountInRow;
				PanelRectTransform.anchorMin = new Vector2(i % AmountInRow / (float)AmountInRow, (float)(v - 1) / VerticalQuantity);
				PanelRectTransform.anchorMax = new Vector2((i % AmountInRow + 1) / (float)AmountInRow, (float)v / VerticalQuantity);

				Buttons[1][i] = Game.transform.GetChild(0).gameObject;
				Buttons[1][i].name = Functions.Games[i];

				// Set game image
				if (i < SceneImages.Length) {
					GameObject SceneImage = Buttons[1][i].transform.GetChild(0).gameObject;
					Image Image = SceneImage.GetComponent<Image>();
					Image.sprite = SceneImages[i];
				}

				// Set game name
				GameObject NameScene = Buttons[1][i].transform.GetChild(1).gameObject;
				Text Name = NameScene.GetComponent<Text>();
				Name.text = Functions.Games[i].ToNormalCase();
			}

			// Set Back button
			Buttons[1][Buttons[1].Length - 1] = ChooseGameBack;
		}

		#endregion

		#region Change current panel

		private static int _CurrentPanel = 0;

		private static int CurrentPanel {
			get => _CurrentPanel;
			set {
				if (_CurrentPanel == value) { return; }

				Panels[_CurrentPanel].SetActive(false);
				Panels[value].SetActive(true);
				EventSystem.current.SetSelectedGameObject(Buttons[value][0]);
				_CurrentPanel = value;
			}
		}

		#endregion

		#region Clicks

		public static void AddEventsToButtons() {
			foreach (GameObject[] Row in Buttons) {
				foreach (GameObject Button in Row) {
					// Add mouse over event
					EventTrigger.Entry Entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
					Entry.callback.AddListener(delegate { EventSystem.current.SetSelectedGameObject(Button); });
					Button.AddComponent<EventTrigger>().triggers.Add(Entry);

					// Add click event
					Button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(Button.name); });
				}
			}
		}

		public struct ButtonClick {
			public readonly string Name;
			public readonly Action OnClick;

			public ButtonClick(string Name, Action OnClick) {
				this.Name = Name;
				this.OnClick = OnClick;
			}
		}

		private static readonly ButtonClick[] ButtonClicks = {
			// MainMenu
			new ButtonClick(ButtonNames[0][0], delegate {
				ToNextScene.Score = 0;
				ToNextScene.GameMode = "Mixed";
				Functions.LoadGame(ToNextScene.GameMode);
			}),
			new ButtonClick(ButtonNames[0][1], delegate { CurrentPanel = 1; }),
			new ButtonClick(ButtonNames[0][2], delegate { CurrentPanel = 2; }),
			new ButtonClick(ButtonNames[0][3], Application.Quit),
			
			// Settings
			new ButtonClick(ButtonNames[2][3], delegate { CurrentPanel = 0; }),
		};

		public static void OnButtonClick(string ButtonName) {
			foreach (ButtonClick ButtonEvent in ButtonClicks) {
				if (ButtonEvent.Name == ButtonName) {
					ButtonEvent.OnClick();
					return;
				}
			}

			foreach (string Game in Functions.Games) {
				if (Game == ButtonName) {
					Functions.LoadGame(Game);
					ToNextScene.Score = 0;
					ToNextScene.GameMode = Game;
					return;
				}
			}
		}

		#endregion

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			UnselectedCheck();
			KeystrokeCheck();
		}

		private static GameObject PreviousSelected;

		private static void UnselectedCheck() {
			if (EventSystem.current.currentSelectedGameObject == null) {
				EventSystem.current.SetSelectedGameObject(
					PreviousSelected == null ?
						Buttons[CurrentPanel][0] :
						PreviousSelected
				);
			}

			PreviousSelected = EventSystem.current.currentSelectedGameObject;
		}

		private static void KeystrokeCheck() {
			if (Input.GetKeyDown(KeyCode.Backspace)) {
				OnButtonClick("Back");
			}
		}

		#endregion

	}
}
