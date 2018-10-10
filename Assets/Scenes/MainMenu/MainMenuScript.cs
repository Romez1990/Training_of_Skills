using System.Threading;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.MainMenu {
	public class MainMenuScript : MonoBehaviour {

		[UsedImplicitly]
		private void Start () {
			intializingRepeators();
			intializingPanels();
			context = SynchronizationContext.Current;
		}

		private SynchronizationContext context;
		private Repeater repeaterDown;
		private Repeater repeaterUp;

		private unsafe void intializingRepeators () {
			fixed (bool* b = &down) { repeaterDown = new Repeater(context, b, 450, 75); }
			repeaterDown.Act += moveSelectionDown;
			fixed (bool* b = &up) { repeaterUp = new Repeater(context, b, 450, 75); }
			repeaterUp.Act += moveSelectionUp;
		}

		private GameObject[] Panels;
		private GameObject[,] Buttons;

		private void intializingPanels () {
			Panels = new GameObject[transform.childCount];
			int max = 0;

			for (int i = 0; i < transform.childCount; i++) {
				Panels[i] = transform.GetChild(i).gameObject;

				if (max < Panels[i].transform.childCount) {
					max = Panels[i].transform.childCount;
				}
			}

			Buttons = new GameObject[Panels.Length, max];

			for (int i = 0; i < Panels.Length; i++) {
				for (int j = 0; j < max; j++) {
					if (j >= Panels[i].transform.childCount) { break; }
					Buttons[i, j] = Panels[i].transform.GetChild(j).gameObject;
					Buttons[i, j].AddComponent<Buttons>(); // Add script component
				}
			}
		}

		[UsedImplicitly]
		private void Update () {
			checkKeyToMoveSelection();
		}

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
			for (int i = 0; i < Panels[_currentPanel].transform.childCount; i++) {
				if (Buttons[_currentPanel, i] == EventSystem.current.currentSelectedGameObject) {
					Debug.Log("I found");
				}
			}
		}

		private void moveSelectionUp () {



		}

		#endregion

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

		public void onClickPlay () {
			SceneManager.LoadScene("MixedMode");
		}

		public void onClickQuit () {
			Application.Quit();
		}

	}
}
