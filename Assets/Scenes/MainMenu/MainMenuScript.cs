using Assets.Scenes.Games.BaseScene;
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
			InitializePanelsAndButtons();
			DisplayAndInitializeGames();
			AddEventsToButtons();
		}

		#region Initialization panels and buttons

		public static GameObject[] Panels;
		public static GameObject[][] Buttons;

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
		}

		public GameObject SelectGameContent;
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

		#region Change current panel

		private static int _CurrentPanel = 0;

		public static int CurrentPanel {
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
					EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
					entry.callback.AddListener(delegate { EventSystem.current.SetSelectedGameObject(Button); });
					Button.AddComponent<EventTrigger>().triggers.Add(entry);

					Button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(Button.name); });
				}
			}
		}

		private struct ButtonClick {
			public readonly string Name;
			public readonly Action OnClick;

			public ButtonClick(string Name, Action OnClick) {
				this.Name = Name;
				this.OnClick = OnClick;
			}
		}

		private static readonly ButtonClick[] ButtonClicks = {
			new ButtonClick("Play",       delegate { MainFunctions.LoadRandomGame(); ToNextScene.Score = 0; ToNextScene.GameMode = "Mixed"; }),
			new ButtonClick("SelectGame", delegate { CurrentPanel = 1; }),
			new ButtonClick("Settings",   delegate { CurrentPanel = 2; }),
			new ButtonClick("Quit",       Application.Quit),
			new ButtonClick("Back",       delegate { CurrentPanel = 0; })
		};

		public static void OnButtonClick(string ButtonName) {
			foreach (ButtonClick ButtonEvent in ButtonClicks) {
				if (ButtonEvent.Name == ButtonName) {
					ButtonEvent.OnClick();
					return;
				}
			}

			foreach (string Game in MainFunctions.Games) {
				if (Game == ButtonName) {
					MainFunctions.LoadSelectedGame(Game);
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

		private GameObject PreviousSelected;

		private void UnselectedCheck() {
			if (EventSystem.current.currentSelectedGameObject == null) {
				EventSystem.current.SetSelectedGameObject(
					PreviousSelected != null ?
						PreviousSelected :
						Buttons[CurrentPanel][0]
				);
			}

			PreviousSelected = EventSystem.current.currentSelectedGameObject;
		}

		private void KeystrokeCheck() {
			if (Input.GetKeyDown(KeyCode.Backspace)) {
				OnButtonClick("Back");
			}
		}

		#endregion

	}
}
