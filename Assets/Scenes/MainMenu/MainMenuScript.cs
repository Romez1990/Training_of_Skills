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
			IntializingPanels();
		}

		#region Buttons and panels

		private GameObject[] Panels;
		private List<GameObject>[] Buttons;

		private void IntializingPanels() {
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

		private void AddEvents(GameObject Button) {
			EventTrigger.Entry entry = new EventTrigger.Entry {
				eventID = EventTriggerType.PointerEnter
			};
			entry.callback.AddListener(eventData =>
				EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(Button));
			Button.AddComponent<EventTrigger>().triggers.Add(entry);
		}

		private int currentPanel = 0;

		public int CurrentPanel {
			get => currentPanel;
			set {
				if (currentPanel >= Panels.Length) { return; }

				Panels[currentPanel].SetActive(false);
				Panels[value].SetActive(true);
				currentPanel = value;
			}
		}

		#endregion

		#region Clicks

		public void Play() {
			ToNextScene.Save(new ToNextScene(0, GameMode.Mixed));
			MainFunctions.LoadRandomGame();
		}

		public void SelectGame() {
			CurrentPanel = 1;
		}

		public void PlaySelectedGame(string GameName) {
			SceneManager.LoadScene(GameName);
			ToNextScene.Save(new ToNextScene(0, GameMode.Single));
		}

		public void Settings() {
			CurrentPanel = 2;
		}

		public void Quit() {
			Application.Quit();
		}

		public void Back() {
			CurrentPanel = 0;
		}

		#endregion

	}
}
