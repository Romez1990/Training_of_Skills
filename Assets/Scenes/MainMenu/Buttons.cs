using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.MainMenu {
	public class Buttons : MonoBehaviour {

		[UsedImplicitly]
		void Start() {
			AddEvents();
			Canvas = GameObject.Find("Canvas");
		}

		private GameObject Canvas;

		private readonly System.Random random = new System.Random();

		private void AddEvents() {
			EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
			entry.callback.AddListener(eventData =>
				EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(gameObject));
			gameObject.AddComponent<EventTrigger>().triggers.Add(entry);

			EventTrigger.Entry entry2 = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
			entry2.callback.AddListener(eventData => Click());
			gameObject.AddComponent<EventTrigger>().triggers.Add(entry2);
		}

		private void Click() {
			switch (gameObject.name) {
				case "Play":
					PlayMode();
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

		public int CurrentPanel {
			get => Canvas.GetComponent<MainMenuScript>().CurrentPanel;
			set => Canvas.GetComponent<MainMenuScript>().CurrentPanel = value;
		}

		private readonly string[] Games = { "FastMath" };

		private void PlayMode() {
			SceneManager.LoadScene(Games[random.Next(Games.Length)]);
		}
	}
}
