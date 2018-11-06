using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.MainMenu {
	public class Buttons : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			AddEvents();
			Canvas = GameObject.Find("Canvas");
		}

		private GameObject Canvas;

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

		private readonly string[] Games = { "FastMath", "FastCircles" };

		private void PlayMode() {
			SceneManager.LoadScene(Games[Random.Range(0, Games.Length)]);
			PlayerPrefs.SetString("Mode", "Mixed");
			PlayerPrefs.SetInt("Score", 0);
		}

		#endregion

	}
}
