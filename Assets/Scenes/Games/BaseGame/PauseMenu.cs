using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scenes.Games.BaseGame {
	public class PauseMenu : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			InitializeMenuButtons();
			AddEventsToButtons();
		}

		#region Initialization panels and buttons

		public static GameObject[] Buttons;

		private void InitializeMenuButtons() {
			Buttons = new GameObject[transform.childCount];

			// Set panel position and size
			/*float ScreenWidth = GetComponent<RectTransform>().rect.width;
			RectTransform PanelRectTransform = GetComponent<RectTransform>();
			float PanelWidth = PanelRectTransform.rect.height / ButtonNames.Length * 0.82f * 4.2f;
			PanelRectTransform.anchorMin = new Vector2((ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMin.y);
			PanelRectTransform.anchorMax = new Vector2(1 - (ScreenWidth - PanelWidth) / 2 / ScreenWidth, PanelRectTransform.anchorMax.y);//*/

			// Button creating
			for (int i = 0; i < Buttons.Length; ++i) {
				GameObject ButtonWrapper = transform.GetChild(i).gameObject;

				// Set position and size
				/*RectTransform ButtonRectTransform = ButtonWrapper.GetComponent<RectTransform>();
				ButtonRectTransform.anchorMin = new Vector2(0, (ButtonNames.Length - i - 1) / (float)ButtonNames.Length);
				ButtonRectTransform.anchorMax = new Vector2(1, (ButtonNames.Length - i) / (float)ButtonNames.Length);//*/

				Buttons[i] = ButtonWrapper.transform.GetChild(0).gameObject;
			}
		}

		#endregion

		#region Events

		public static void AddEventsToButtons() {
			foreach (GameObject Button in Buttons) {
				EventTrigger.Entry Entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
				Entry.callback.AddListener(delegate { EventSystem.current.SetSelectedGameObject(Button); });
				Button.AddComponent<EventTrigger>().triggers.Add(Entry);
			}
		}

		[UsedImplicitly] public void ClickResume() => PauseControl.IsPause = false;
		[UsedImplicitly] public void ClickREstart() => Functions.ReloadGame();
		[UsedImplicitly] public void ClickMainMenu() => Functions.LoadGame("MainMenu");
		[UsedImplicitly] public void ClickQuit() => Application.Quit();

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
