using Assets.Scenes.Games.BaseGame.Sounds;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scenes.Scoreboard {
	public class ScoreboardScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			AddEventsToButtons();
		}

		public GameObject[] Buttons;

		public void AddEventsToButtons() {
			foreach (GameObject Button in Buttons) {
				EventTrigger.Entry Entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
				Entry.callback.AddListener(delegate { EventSystem.current.SetSelectedGameObject(Button); });
				Button.AddComponent<EventTrigger>().triggers.Add(Entry);
			}
		}

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			UnselectedCheck();
		}

		private GameObject PreviousSelected;

		private void UnselectedCheck() {
			if (EventSystem.current.currentSelectedGameObject == null) {
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

		[UsedImplicitly]
		public void MainMenuClick() {
			Functions.LoadGame("MainMenu");
		}

		[UsedImplicitly]
		public void StartOverClick() {
			Functions.ReloadGame();
		}

		#endregion

	}
}
