using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.MainMenu {
	public class MainMenuScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			InitializeMenuButtons();
			InitializeGameButtons();
		}

		#region Initialization panels and buttons

		private static GameObject[] Panels;
		private static GameObject[][] Buttons;

		private void InitializeMenuButtons() {
			int PanelNumber = transform.childCount - 1; // - 1 because there is Background there
			Panels = new GameObject[PanelNumber];
			Buttons = new GameObject[PanelNumber][];

			for (int i = 0; i < PanelNumber; ++i) {
				Panels[i] = transform.GetChild(i + 1).gameObject; // + 1 because there is Background at 0 index

				// ChooseGame panel
				if (i == PanelNumber - 1)
					continue;

				// Set panel position and size
				/*float ScreenWidth = GetComponent<RectTransform>().rect.width;
				RectTransform PanelRectTransform = Panels[i].GetComponent<RectTransform>();
				float PanelWidth = PanelRectTransform.rect.height / Panels[i].transform.childCount * 0.82f * 4.2f;
				PanelRectTransform.anchorMin = new Vector2((ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMin.y);
				PanelRectTransform.anchorMax = new Vector2(1 - (ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMax.y);//*/

				// Button creating
				Buttons[i] = new GameObject[Panels[i].transform.childCount];
				for (int j = 0; j < Buttons[i].Length; ++j) {
					GameObject ButtonWrapper = Panels[i].transform.GetChild(j).gameObject;

					// Set position and size
					/*RectTransform ButtonRectTransform = ButtonWrapper.GetComponent<RectTransform>();
					ButtonRectTransform.anchorMin = new Vector2(0, (Buttons[i].Length - j - 1) / (float)Buttons[i].Length);
					ButtonRectTransform.anchorMax = new Vector2(1, (Buttons[i].Length - j) / (float)Buttons[i].Length);//*/

					Buttons[i][j] = ButtonWrapper.transform.GetChild(0).gameObject;

					AddMouseOver(Buttons[i][j]);
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
			int VerticalQuantity = Mathf.CeilToInt(Functions.Games.Length / (float)AmountInRow);
			float Height = VerticalQuantity * GetComponent<RectTransform>().rect.width / AmountInRow * AspectRatio;
			RectTransform RectTransform = ChooseGameContent.GetComponent<RectTransform>();
			RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, Height);

			ref GameObject[] GameButtons = ref Buttons[2];
			GameButtons = new GameObject[Functions.Games.Length + 1];
			for (int i = 0; i < Functions.Games.Length; ++i) {
				GameObject Game = Instantiate(GameButtonPrefab, ChooseGameContent.transform);
				Game.name = Functions.Games[i] + "Wrapper";

				// Set button position and size
				RectTransform PanelRectTransform = Game.GetComponent<RectTransform>();
				int v = VerticalQuantity - i / AmountInRow;
				PanelRectTransform.anchorMin = new Vector2(i % AmountInRow / (float)AmountInRow, (float)(v - 1) / VerticalQuantity);
				PanelRectTransform.anchorMax = new Vector2((i % AmountInRow + 1) / (float)AmountInRow, (float)v / VerticalQuantity);

				GameButtons[i] = Game.transform.GetChild(0).gameObject;
				GameButtons[i].name = Functions.Games[i];

				// Set game image
				if (i < SceneImages.Length) {
					GameObject SceneImage = GameButtons[i].transform.GetChild(0).gameObject;
					Image Image = SceneImage.GetComponent<Image>();
					Image.sprite = SceneImages[i];
				}

				// Set game name
				GameObject NameScene = GameButtons[i].transform.GetChild(1).gameObject;
				Text Name = NameScene.GetComponent<Text>();
				Name.text = Functions.Games[i].ToNormalCase();

				AddMouseOver(GameButtons[i]);
				string ButtonName = GameButtons[i].name;
				GameButtons[i].GetComponent<Button>().onClick.AddListener(delegate { OnButtonGameClick(ButtonName); });
			}

			// Set Back button
			GameButtons[GameButtons.Length - 1] = ChooseGameBack;
			AddMouseOver(ChooseGameBack);
			ChooseGameBack.GetComponent<Button>().onClick.AddListener(ClickBack);
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

		#region Events

		public static void AddMouseOver(GameObject Button) {
			EventTrigger.Entry Entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
			Entry.callback.AddListener(delegate { MouseOver(Button); });
			Button.AddComponent<EventTrigger>().triggers.Add(Entry);
		}

		private static void MouseOver(GameObject Button) {
			EventSystem.current.SetSelectedGameObject(Button);
		}

		public static void OnButtonGameClick(string ButtonName) {
			Debug.Log(ButtonName);
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

		// Main menu
		[UsedImplicitly]
		public void ClickPlay() {
			Functions.LoadGame("Mixed");
			PlayingInfo.Score = 0;
			PlayingInfo.Time = 30;
			PlayingInfo.GameMode = "Mixed";
		}
		[UsedImplicitly] public void ClickChooseGame() => CurrentPanel = 2;
		[UsedImplicitly] public void ClickSettings() => CurrentPanel = 1;
		[UsedImplicitly] public void ClickQuit() => Application.Quit();

		// Settings
		[UsedImplicitly] public void ClickBack() => CurrentPanel = 0;

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

		private void KeystrokeCheck() {
			if (Input.GetKeyDown(KeyCode.Backspace)) {
				ClickBack();
			}
		}

		#endregion

	}
}
