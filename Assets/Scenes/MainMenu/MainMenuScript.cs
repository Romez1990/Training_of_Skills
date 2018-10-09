using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.MainMenu {
	public class MainMenuScript : MonoBehaviour {

		[UsedImplicitly]
		private void Start () {
			intializingRepeators();
			intializingPanels();
		}

		Repeater repeaterDown;
		Repeater repeaterUp;

		private unsafe void intializingRepeators () {
			fixed (bool* b = &down) { repeaterDown = new Repeater(b, 450, 75); }
			repeaterDown.Act += moveSelectionDown;
			fixed (bool* b = &up) { repeaterUp = new Repeater(b, 450, 75); }
			repeaterUp.Act += moveSelectionUp;
		}

		private void intializingPanels () {

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
			Debug.Log("Moved down");
		}

		private void moveSelectionUp () {
			Debug.Log("Moved up");
		}

		#endregion

		public void onClickPlay () {
			SceneManager.LoadScene("MixedMode");
		}

		public void onClickSelect () {
			SceneManager.LoadScene("SelectMode");
		}

		public void onClickSettings () {
			SceneManager.LoadScene("Settings");
		}

		public void onClickQuit () {
			Application.Quit();
		}

	}
}
