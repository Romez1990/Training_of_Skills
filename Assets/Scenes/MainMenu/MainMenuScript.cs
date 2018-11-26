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
			entry.callback.AddListener(PointerEventData => {

				EventSystem.SetSelectedGameObject(Button);
			});
			Button.AddComponent<EventTrigger>().triggers.Add(entry);
		}

		private int _CurrentPanel = 0;

		public int CurrentPanel {
			get => _CurrentPanel;
			set {
				if (_CurrentPanel == value) { return; }

				Panels[_CurrentPanel].SetActive(false);
				Panels[value].SetActive(true);
				EventSystem.SetSelectedGameObject(Buttons[value][0]);
				_CurrentPanel = value;
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
			EventSystem.SetSelectedGameObject(null);
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
