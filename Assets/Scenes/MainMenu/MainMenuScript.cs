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
			intializingRepeators();
			intializingPanels();
			context = SynchronizationContext.Current;
		}

		#endregion

		#region Repeators

		private SynchronizationContext context;
		private Repeater repeaterDown;
		private Repeater repeaterUp;

		private unsafe void intializingRepeators () {
			fixed (bool* b = &down) { repeaterDown = new Repeater(context, b, 450, 75); }
			repeaterDown.Act += moveSelectionDown;
			fixed (bool* b = &up) { repeaterUp = new Repeater(context, b, 450, 75); }
			repeaterUp.Act += moveSelectionUp;
		}

		#endregion

		#region Buttons and panels

		private GameObject[] Panels;
		private List<GameObject>[] Buttons;

		private void intializingPanels () {
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

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update () {
			checkKeyToMoveSelection();
		}

		#endregion

		#region Moving selection

		bool down;
		bool up;

		private void checkKeyToMoveSelection () {
			bool downDown = Input.GetKeyDown(KeyCode.DownArrow);
			down = Input.GetKey(KeyCode.DownArrow);
			if (downDown) {
				repeaterDown.startWork();
			}

			bool upDown = Input.GetKeyDown(KeyCode.UpArrow);
			up = Input.GetKey(KeyCode.UpArrow);
			if (upDown) {
				repeaterUp.startWork();
			}
		}

		private void moveSelectionDown () {
			Debug.Log("I found");

			for (int i = 0; i < Panels[_currentPanel].transform.childCount; i++) {
				if (Buttons[_currentPanel][i] == EventSystem.current.currentSelectedGameObject) {
					//Debug.Log("I found");
				}
			}
		}

		private void moveSelectionUp () {



		}

		#endregion

		#region Properties

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

		#region Button clicks

		private readonly System.Random random = new System.Random();

		public void onClickPlay () {
			int select = random.Next(1);

			switch (select) {
				case 0:
					SceneManager.LoadScene("FastMath");
					break;
				case 2:
					SceneManager.LoadScene("");
					break;
				case 3:
					SceneManager.LoadScene("");
					break;
				case 4:
					SceneManager.LoadScene("");
					break;
				case 5:
					SceneManager.LoadScene("");
					break;
			}
		}

		public void onClickSelectMode () {
			//CurrentPanel = 1;
		}

		public void onClickSettings () {
			CurrentPanel = 1;
		}

		public void onClickQuit () {
			Application.Quit();
		}

		public void onClickBack () {
			CurrentPanel = 0;
		}

		#endregion

	}
}
