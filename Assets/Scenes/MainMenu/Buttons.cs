using Assets.Scenes.Games.BaseScene;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scenes.MainMenu {
	public class Buttons : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			AddEvents();
			Canvas = GameObject.Find("Canvas");
			MainMenuScript = Canvas.GetComponent<MainMenuScript>();
		}

		private void AddEvents() {
			var t = gameObject.AddComponent<EventTrigger>().triggers;

			EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
			entry.callback.AddListener(eventData =>
				EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(gameObject));
			t.Add(entry);

			EventTrigger.Entry entry2 = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
			entry2.callback.AddListener(eventData => Click());
			t.Add(entry2);
		}

		#endregion

		#region Click

		private void Click() {
			switch (gameObject.name) {
				// Main menu
				case "Play":
					ToNextScene.Save(new ToNextScene(0, GameMode.Mixed));
					MainFunctions.LoadRandomGame();
					break;

				case "SelectGame":
					//CurrentPanel = 1;
					break;

				case "Settings":
					CurrentPanel = 1;
					break;

				case "Quit":
					Application.Quit();
					break;

				// Settings
				case "Screen":

					break;

				case "Game":

					break;

				case "Control":

					break;

				case "Back":
					CurrentPanel = 0;
					break;
			}
		}

		private GameObject Canvas;
		private MainMenuScript MainMenuScript;

		public int CurrentPanel {
			get => MainMenuScript.CurrentPanel;
			set => MainMenuScript.CurrentPanel = value;
		}

		#endregion

	}
}
