using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.MainMenu {
	public class MainMenuScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start () {
			IntializingPanels();
		}

		#endregion

		#region Buttons and panels

		private GameObject[] Panels;
		private List<GameObject>[] Buttons;

		private void IntializingPanels () {
			Panels = new GameObject[transform.childCount];
			Buttons = new List<GameObject>[Panels.Length];

			for (int i = 0; i < Panels.Length; i++) {
				Panels[i] = transform.GetChild(i).gameObject;
				Buttons[i] = new List<GameObject>();

				for (int j = 0; j < Panels[i].transform.childCount; j++) {
					Buttons[i].Add(Panels[i].transform.GetChild(j).gameObject);
					Buttons[i][j].AddComponent<Buttons>(); // Add script component
				}
			}
		}

		private int _currentPanel = 0;

		public int CurrentPanel {
			get => _currentPanel;
			set {
				if (_currentPanel >= Panels.Length) { return; }

				Panels[_currentPanel].SetActive(false);
				Panels[value].SetActive(true);
				_currentPanel = value;
			}
		}

		#endregion

	}
}
