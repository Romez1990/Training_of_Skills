using Assets.Scenes.Games.BaseScene;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.MainMenu {
	public class MainMenuScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			EventSystem = EventSystem.current.GetComponent<EventSystem>();
			InitializePanelsAndButtons();
			AddEventsToButtons();
		}

		public static GameObject[] Panels;
		public static GameObject[][] Buttons;

		private void InitializePanelsAndButtons() {
			Panels = new GameObject[transform.childCount];
			if (Buttons == null)
				Buttons = new GameObject[Panels.Length][];


			for (int i = 0; i < Panels.Length; i++) {
				Panels[i] = transform.GetChild(i).gameObject;

				if (i == 1) { continue; }

				Buttons[i] = new GameObject[Panels[i].transform.childCount];
				for (int j = 0; j < Panels[i].transform.childCount; j++) {
					Buttons[i][j] = Panels[i].transform.GetChild(j).gameObject;
				}
			}
		}

		private EventSystem EventSystem;

		private void AddEventsToButtons() {
			foreach (GameObject[] Row in Buttons)
				foreach (GameObject Button in Row)
					AddEvent(Button);

			void AddEvent(GameObject Button) {
				EventTrigger.Entry entry = new EventTrigger.Entry {
					eventID = EventTriggerType.PointerEnter
				};
				entry.callback.AddListener(PointerEventData => {
					EventSystem.SetSelectedGameObject(Button);
				});
				Button.AddComponent<EventTrigger>().triggers.Add(entry);
			}
		}

		#endregion

		private int _CurrentPanel = 0;

		public int CurrentPanel {
			get => _CurrentPanel;
			set {
				if (_CurrentPanel == value) { return; }

				Panels[_CurrentPanel].SetActive(false);
				Panels[value].SetActive(true);
				//EventSystem.SetSelectedGameObject(value == 1 ? DisplayGames.Games[0] : Buttons[value][0]);
				EventSystem.SetSelectedGameObject(Buttons[value][0]);
				_CurrentPanel = value;
			}
		}

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

		#region Update

		[UsedImplicitly]
		private void Update() {
			CheckBack();
		}

		private void CheckBack() {
			if (Input.GetKeyDown(KeyCode.Backspace)) {
				Back();
			}
		}

		#endregion

	}
}
