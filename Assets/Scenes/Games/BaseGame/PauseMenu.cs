using Assets.Scenes.Games.BaseGame.Sounds;
using Assets.Scenes.MainMenu;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseGame {
	public class PauseMenu : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			InitializeMenuButtons();
			AddEventsToButtons();
			PreviousSelected = null;
		}

		private static readonly string[] ButtonNames = {
			"Resume",
			"Restart",
			"MainMenu",
			"Quit"
		};

		#region Initialization panels and buttons

		public GameObject ButtonPrefab;
		public static GameObject[] Buttons;

		private void InitializeMenuButtons() {
			Buttons = new GameObject[ButtonNames.Length];

			float CanvasWidth = transform.root.GetComponent<RectTransform>().rect.width;
			RectTransform PauseMenuRectTransform = transform.parent.GetComponent<RectTransform>();
			RectTransform ButtonsRectTransform = GetComponent<RectTransform>();

			float ButtonsWidth = ButtonsRectTransform.rect.height / ButtonNames.Length * 0.78f * 4;
			float PauseMenuWidth = ButtonsWidth + PauseMenuRectTransform.rect.height * 0.28f;

			float PauseMenuPercent = (1 - PauseMenuWidth / CanvasWidth) / 2;
			PauseMenuRectTransform.anchorMin = new Vector2(PauseMenuPercent, PauseMenuRectTransform.anchorMin.y);
			PauseMenuRectTransform.anchorMax = new Vector2(1 - PauseMenuPercent, PauseMenuRectTransform.anchorMax.y);

			float ButtonsPercent = (1 - ButtonsWidth / PauseMenuWidth) / 2;
			ButtonsRectTransform.anchorMin = new Vector2(ButtonsPercent, ButtonsRectTransform.anchorMin.y);
			ButtonsRectTransform.anchorMax = new Vector2(1 - ButtonsPercent, ButtonsRectTransform.anchorMax.y);

			for (int i = 0; i < Buttons.Length; ++i) {
				GameObject ButtonWrapper = Instantiate(ButtonPrefab, transform);
				ButtonWrapper.name = ButtonNames[i] + "Wrapper";

				RectTransform ButtonRectTransform = ButtonWrapper.GetComponent<RectTransform>();
				ButtonRectTransform.anchorMin = new Vector2(0, (ButtonNames.Length - i - 1) / (float)ButtonNames.Length);
				ButtonRectTransform.anchorMax = new Vector2(1, (ButtonNames.Length - i) / (float)ButtonNames.Length);

				Buttons[i] = ButtonWrapper.transform.GetChild(0).gameObject;
				Buttons[i].name = ButtonNames[i];

				Buttons[i].transform.GetChild(0).GetComponent<Text>().text = ButtonNames[i].ToNormalCase();
			}
		}

		#endregion

		#region Button events

		public static void AddEventsToButtons() {
			foreach (GameObject Button in Buttons) {
				// Add mouse over event
				EventTrigger.Entry Entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
				Entry.callback.AddListener(delegate { EventSystem.current.SetSelectedGameObject(Button); });
				Button.AddComponent<EventTrigger>().triggers.Add(Entry);

				// Add click event
				Button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(Button.name); });
			}
		}

		private static readonly MainMenuScript.ButtonClick[] ButtonClicks = {
			new MainMenuScript.ButtonClick(ButtonNames[0], delegate { PauseControl.IsPause = false; }),
			new MainMenuScript.ButtonClick(ButtonNames[1], Functions.ReloadGame),
			new MainMenuScript.ButtonClick(ButtonNames[2], delegate { Functions.LoadGame("MainMenu"); }),
			new MainMenuScript.ButtonClick(ButtonNames[3], Application.Quit)
		};

		public static void OnButtonClick(string ButtonName) {
			Sound.Click();

			foreach (MainMenuScript.ButtonClick ButtonEvent in ButtonClicks) {
				if (ButtonEvent.Name == ButtonName) {
					ButtonEvent.OnClick();
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
		}

		public static GameObject PreviousSelected;

		private static void UnselectedCheck() {
			if (PauseControl.IsPause && EventSystem.current.currentSelectedGameObject == null) {
				EventSystem.current.SetSelectedGameObject(
					PreviousSelected == null ?
						Buttons[0] :
						PreviousSelected
				);
			}

			if (PreviousSelected != EventSystem.current.currentSelectedGameObject) {
				Sound.MouseOver();
				PreviousSelected = EventSystem.current.currentSelectedGameObject;
			}
		}

		#endregion

	}
}