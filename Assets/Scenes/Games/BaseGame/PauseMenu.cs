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
			// Remove all existing buttons from the scene
			/*for (int i = 0; i < transform.childCount; ++i)
				Destroy(transform.GetChild(i).gameObject);//*/

			Buttons = new GameObject[ButtonNames.Length];
			// Set panel position and size
			float ScreenWidth = GetComponent<RectTransform>().rect.width;
			RectTransform PanelRectTransform = GetComponent<RectTransform>();
			float PanelWidth = PanelRectTransform.rect.height / ButtonNames.Length * 0.82f * 4.2f;
			PanelRectTransform.anchorMin = new Vector2((ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMin.y);
			PanelRectTransform.anchorMax = new Vector2(1 - (ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMax.y);

			// Button creating
			for (int i = 0; i < Buttons.Length; ++i) {
				GameObject ButtonWrapper = Instantiate(ButtonPrefab, transform);
				ButtonWrapper.name = ButtonNames[i] + "Wrapper";

				// Set position and size
				RectTransform ButtonRectTransform = ButtonWrapper.GetComponent<RectTransform>();
				ButtonRectTransform.anchorMin = new Vector2(0, (ButtonNames.Length - i - 1) / (float)ButtonNames.Length);
				ButtonRectTransform.anchorMax = new Vector2(1, (ButtonNames.Length - i) / (float)ButtonNames.Length);

				Buttons[i] = ButtonWrapper.transform.GetChild(0).gameObject;
				Buttons[i].name = ButtonNames[i];

				// Set text
				Buttons[i].transform.GetChild(0).GetComponent<Text>().text = ButtonNames[i].ToNormalCase();
			}
		}

		#endregion

		#region Clicks

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

			PreviousSelected = EventSystem.current.currentSelectedGameObject;
		}

		#endregion

	}
}