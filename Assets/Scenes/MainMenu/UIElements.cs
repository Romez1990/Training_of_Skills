using Assets.Scenes.Games.BaseScene;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.MainMenu {
	public static class UIElements {

		#region Buttons and panels

		public static GameObject[] Panels;
		public static GameObject[][] Buttons;
		private static bool AlreadyDid = false;

		public static void AddEventsToButtons() {
			if (Buttons.Any(Row => Row == null || AlreadyDid))
				return;

			foreach (GameObject[] Row in Buttons) {
				foreach (GameObject Button in Row) {
					EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
					entry.callback.AddListener(delegate { EventSystem.current.SetSelectedGameObject(Button); });
					Button.AddComponent<EventTrigger>().triggers.Add(entry);

					Button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(Button.name); });
				}
			}

			AlreadyDid = true;
		}

		private static int _CurrentPanel = 0;

		public static int CurrentPanel {
			get => _CurrentPanel;
			set {
				if (_CurrentPanel == value) { return; }

				Panels[_CurrentPanel].SetActive(false);
				Panels[value].SetActive(true);
				EventSystem.current.SetSelectedGameObject(Buttons[value][0]);
				_CurrentPanel = value;
			}
		}

		#endregion

		#region Clicks

		private struct ButtonClick {
			public readonly string Name;
			public readonly Action OnClick;

			public ButtonClick(string Name, Action OnClick) {
				this.Name = Name;
				this.OnClick = OnClick;
			}
		}

		private static readonly ButtonClick[] ButtonClicks = {
			new ButtonClick("Play",       delegate { MainFunctions.LoadRandomGame(); ToNextScene.Score = 0; ToNextScene.GameMode = "Mixed"; }),
			new ButtonClick("SelectGame", delegate { CurrentPanel = 1; }),
			new ButtonClick("Settings",   delegate { CurrentPanel = 2; }),
			new ButtonClick("Quit",       Application.Quit),
			new ButtonClick("Back",       delegate { CurrentPanel = 0; })
		};

		public static void OnButtonClick(string ButtonName) {
			foreach (ButtonClick ButtonEvent in ButtonClicks) {
				if (ButtonEvent.Name == ButtonName) {
					ButtonEvent.OnClick();
					return;
				}
			}

			foreach (string Game in MainFunctions.Games) {
				if (Game == ButtonName) {
					MainFunctions.LoadSelectedGame(Game);
					ToNextScene.Score = 0;
					ToNextScene.GameMode = Game;
					return;
				}
			}
		}

		#endregion

	}
}
