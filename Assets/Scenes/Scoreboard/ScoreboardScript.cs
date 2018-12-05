using System.Collections.Generic;
using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
					PreviousSelected != null ?
						PreviousSelected :
						Buttons[0]
				);
			}

			PreviousSelected = EventSystem.current.currentSelectedGameObject;
		}

		[UsedImplicitly]
		public void MainMenuClick() {
			SceneManager.LoadScene("MainMenu");
		}

		[UsedImplicitly]
		public void StartOverClick() {
			ToNextScene.Score = 0;

			if (ToNextScene.GameMode == "Mixed" || ToNextScene.GameMode == null)
				Functions.LoadRandomGame();
			else
				SceneManager.LoadScene(ToNextScene.GameMode);
		}

		#endregion

	}
}
