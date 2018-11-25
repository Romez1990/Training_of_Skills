using Assets.Scenes.Games.BaseScene;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.MainMenu {
	public class MainMenuScript : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			EventSystem = EventSystem.current.GetComponent<EventSystem>();
			InitializingPanels();
			CurrentPanel = 0;
			//EventSystem.SetSelectedGameObject(Buttons[0][0]);
		}

		#region Buttons and panels

		private GameObject[] Panels;
		private List<GameObject>[] Buttons;

		private void InitializingPanels() {
			Panels = new GameObject[transform.childCount];
			Buttons = new List<GameObject>[Panels.Length];

			for (int i = 0; i < Panels.Length; i++) {
				Panels[i] = transform.GetChild(i).gameObject;
				Buttons[i] = new List<GameObject>();

				for (int j = 0; j < Panels[i].transform.childCount; j++) {
					Buttons[i].Add(Panels[i].transform.GetChild(j).gameObject);
					AddEvents(Buttons[i][j]);
				}
			}
		}

		private EventSystem EventSystem;

		private void AddEvents(GameObject Button) {
			EventTrigger.Entry entry = new EventTrigger.Entry {
				eventID = EventTriggerType.PointerEnter
			};
			entry.callback.AddListener(eventData =>
				EventSystem.SetSelectedGameObject(Button));
			Button.AddComponent<EventTrigger>().triggers.Add(entry);
		}

		private int? _CurrentPanel = null;

		public int CurrentPanel {
			get => _CurrentPanel ?? -1;
			set {
				//Debug.Log("Set");

				if (_CurrentPanel == value) { return; }

				if (_CurrentPanel != null)
					Panels[(int)_CurrentPanel].SetActive(false);
				Panels[value].SetActive(true);
				_CurrentPanel = value;

				EventSystem.SetSelectedGameObject(Buttons[value][0]);
				//Debug.Log("Set EventSystem");
			}
		}

		#endregion

		#region Clicks

		[UsedImplicitly]
		public void Play() {
			ToNextScene.Save(new ToNextScene(0, GameMode.Mixed));
			MainFunctions.LoadRandomGame();
		}

		[UsedImplicitly]
		public void SelectGame() {
			CurrentPanel = 1;
		}

		[UsedImplicitly]
		public void PlaySelectedGame(string GameName) {
			SceneManager.LoadScene(GameName);
			ToNextScene.Save(new ToNextScene(0, GameMode.Single));
		}

		[UsedImplicitly]
		public void Settings() {
			CurrentPanel = 2;
		}

		[UsedImplicitly]
		public void Quit() {
			Application.Quit();
		}

		[UsedImplicitly]
		public void Back() {
			CurrentPanel = 0;
		}

		#endregion

		[UsedImplicitly]
		private void Update() {
			if (Input.GetKeyDown(KeyCode.Backspace)) {
				Back();
			}
		}

	}
}
