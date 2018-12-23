using Assets.Scenes.Games.BaseGame;
using Assets.Scenes.Games.BaseGame.Sounds;
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
			currentPanel = 0;
			InitPanels();
			InitMainMenu(0);
			InitChoosePanel(1);
			InitSettingsPanel(2);
			TimeToggle = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Toggle>();
			SoundToggle = transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Toggle>();
			Settings.Load();
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
				"Cancel",
				"Apply"
			}
		};

		#region Initialization panels and buttons

		private static GameObject[] Panels;
		private static GameObject[][] Buttons;

		private void InitPanels() {
			int PanelAmount = transform.childCount;

			Panels = new GameObject[PanelAmount];
			Buttons = new GameObject[PanelAmount][];

			for (int i = 0; i < PanelAmount; ++i) {
				Panels[i] = transform.GetChild(i).gameObject;
			}
		}

		public GameObject ButtonPrefab;

		private void InitMainMenu(int i) {
			Panels[i] = transform.GetChild(i).gameObject;

			for (int j = 0; j < Panels[i].transform.childCount; ++j)
				Destroy(Panels[i].transform.GetChild(j).gameObject);

			float ScreenWidth = GetComponent<RectTransform>().rect.width;
			RectTransform PanelRectTransform = Panels[i].GetComponent<RectTransform>();
			float PanelWidth = PanelRectTransform.rect.height / ButtonNames[i].Length * 0.82f * 4;
			PanelRectTransform.anchorMin = new Vector2((ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMin.y);
			PanelRectTransform.anchorMax = new Vector2(1 - (ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMax.y);

			Buttons[i] = new GameObject[ButtonNames[i].Length];
			for (int j = 0; j < ButtonNames[i].Length; ++j) {
				GameObject ButtonWrapper = Instantiate(ButtonPrefab, Panels[i].transform);
				ButtonWrapper.name = ButtonNames[i][j] + "Wrapper";

				RectTransform ButtonRectTransform = ButtonWrapper.GetComponent<RectTransform>();
				ButtonRectTransform.anchorMin = new Vector2(0, (ButtonNames[i].Length - j - 1) / (float)ButtonNames[i].Length);
				ButtonRectTransform.anchorMax = new Vector2(1, (ButtonNames[i].Length - j) / (float)ButtonNames[i].Length);

				Buttons[i][j] = ButtonWrapper.transform.GetChild(0).gameObject;
				Buttons[i][j].name = ButtonNames[i][j];

				AddEventsToButtons(Buttons[i][j]);

				Buttons[i][j].transform.GetChild(0).GetComponent<Text>().text = ButtonNames[i][j].ToNormalCase();
			}
		}

		public GameObject GameButtonPrefab;
		public Sprite[] SceneImages;
		public GameObject ChooseGameContent;
		public GameObject ChooseGameBack;
		private const int AmountInRow = 3;
		private const float AspectRatio = 3 / 4f;

		private void InitChoosePanel(int i) {
			// Remove all buttons from the scene
			/*for (int i = 0; i < ChooseGameContent.transform.childCount; ++i)
				Destroy(ChooseGameContent.transform.GetChild(i).gameObject);//*/

			// Set size of the Content
			int VerticalQuantity = Mathf.CeilToInt(Functions.Games.Length / (float)AmountInRow);
			float Height = VerticalQuantity * GetComponent<RectTransform>().rect.width / AmountInRow * AspectRatio;
			RectTransform RectTransform = ChooseGameContent.GetComponent<RectTransform>();
			RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, Height);

			Buttons[i] = new GameObject[Functions.Games.Length + 1];
			for (int j = 0; j < Functions.Games.Length; ++j) {
				GameObject Game = Instantiate(GameButtonPrefab, ChooseGameContent.transform);
				Game.name = Functions.Games[j];

				// Set button position and size
				RectTransform PanelRectTransform = Game.GetComponent<RectTransform>();
				int v = VerticalQuantity - j / AmountInRow;
				PanelRectTransform.anchorMin = new Vector2(j % AmountInRow / (float)AmountInRow, (float)(v - 1) / VerticalQuantity);
				PanelRectTransform.anchorMax = new Vector2((j % AmountInRow + 1) / (float)AmountInRow, (float)v / VerticalQuantity);

				Buttons[i][j] = Game.transform.GetChild(0).gameObject;
				Buttons[i][j].name = "Button";

				AddEventsToButtons(Buttons[i][j], Functions.Games[j]);

				// Set game image
				if (j < SceneImages.Length) {
					GameObject SceneImage = Buttons[i][j].transform.GetChild(0).gameObject;
					Image Image = SceneImage.GetComponent<Image>();
					Image.sprite = SceneImages[j];
				}

				// Set game name
				GameObject NameScene = Buttons[i][j].transform.GetChild(1).gameObject;
				Text Name = NameScene.GetComponent<Text>();
				Name.text = Functions.Games[j].ToNormalCase();
			}

			// Set Back button
			Buttons[i][Buttons[i].Length - 1] = ChooseGameBack;
			AddEventsToButtons(Buttons[i][Buttons[i].Length - 1]);
		}

		private void InitSettingsPanel(int i) {
			float ScreenWidth = GetComponent<RectTransform>().rect.width;
			RectTransform PanelRectTransform = Panels[i].GetComponent<RectTransform>();
			float PanelWidth = PanelRectTransform.rect.height / ButtonNames[i].Length * 0.82f * 4.2f;
			PanelRectTransform.anchorMin = new Vector2((ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMin.y);
			PanelRectTransform.anchorMax = new Vector2(1 - (ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMax.y);

			Buttons[i] = new GameObject[2];
			Buttons[i][0] = Panels[i].transform.GetChild(2).GetChild(0).gameObject;
			Buttons[i][1] = Panels[i].transform.GetChild(3).GetChild(0).gameObject;

			foreach (GameObject Button in Buttons[i])
				AddEventsToButtons(Button);
		}

		#endregion

		#region Change current panel

		private static int currentPanel;

		private static int CurrentPanel {
			get => currentPanel;
			set {
				if (currentPanel == value) { return; }

				Panels[currentPanel].SetActive(false);
				Panels[value].SetActive(true);
				EventSystem.current.SetSelectedGameObject(Buttons[value][0]);
				currentPanel = value;

				switch (value) {
					case 2:
						SetSettings();
						break;
				}
			}
		}

		#endregion

		#region Button events

		public static void AddEventsToButtons(GameObject Button) {
			AddEventsToButtons(Button, Button.name);
		}

		public static void AddEventsToButtons(GameObject Button, string Parameter) {
			// Add mouse over event
			EventTrigger.Entry Entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
			Entry.callback.AddListener(delegate { EventSystem.current.SetSelectedGameObject(Button); });
			Button.AddComponent<EventTrigger>().triggers.Add(Entry);

			// Add click event
			Button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(Parameter); });
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
				Functions.LoadGame("Mixed");
				PlayingInfo.Score = 0;
				PlayingInfo.Time = Settings.CurrentSettings.TimeIsOn ? 30 : 100000000;
				PlayingInfo.GameMode = "Mixed";
			}),
			new ButtonClick(ButtonNames[0][1], delegate { CurrentPanel = 1; }),
			new ButtonClick(ButtonNames[0][2], delegate { CurrentPanel = 2; }),
			new ButtonClick(ButtonNames[0][3], Application.Quit),
			
			// Choose panel
			new ButtonClick("Back", delegate { CurrentPanel = 0; }), 

			// Settings
			new ButtonClick(ButtonNames[2][0], CancelSettings),
			new ButtonClick(ButtonNames[2][1], ApplySettings)
		};

		public static void OnButtonClick(string ButtonName) {
			Sound.Click();

			foreach (ButtonClick ButtonEvent in ButtonClicks) {
				if (ButtonEvent.Name == ButtonName) {
					ButtonEvent.OnClick();
					return;
				}
			}

			foreach (string Game in Functions.Games) {
				if (Game == ButtonName) {
					Functions.LoadGame(Game);
					PlayingInfo.Score = 0;
					PlayingInfo.Time = 30;
					PlayingInfo.GameMode = Game;
					return;
				}
			}
		}

		#endregion

		#endregion

		#region Settings

		private static Toggle SoundToggle;
		private static Toggle TimeToggle;

		private static void SetSettings() {
			Settings.Load();
			SoundToggle.isOn = Settings.CurrentSettings.SoundIsOn;
			TimeToggle.isOn = Settings.CurrentSettings.TimeIsOn;
		}

		private static void CancelSettings() {
			CurrentPanel = 0;
		}

		private static void ApplySettings() {
			CurrentPanel = 0;

			new Settings {
				SoundIsOn = SoundToggle.isOn,
				TimeIsOn = TimeToggle.isOn
			}.Save();
		}

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

			if (PreviousSelected != EventSystem.current.currentSelectedGameObject) {
				Sound.MouseOver();
				PreviousSelected = EventSystem.current.currentSelectedGameObject;
			}
		}

		private static void KeystrokeCheck() {
			if (Input.GetKeyDown(KeyCode.Backspace)) {
				OnButtonClick("Back");
			}
		}

		#endregion

	}
}
