using JetBrains.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

	[UsedImplicitly]
	private void Start () {
		Debug.Log(nameof(Start));
	}

	[UsedImplicitly]
	private void Update () {
		checkKeyToMoveOn();
	}

	#region Moving selection

	bool down;
	bool up;

	struct Move {
		public Move (ref bool _b, Action _method) {
			b = _b;
			method = _method;
		}

		public bool b;
		public Action method;
	}

	private void checkKeyToMoveOn () {

		bool downDown = Input.GetKeyDown(KeyCode.DownArrow);
		down = Input.GetKey(KeyCode.DownArrow);

		if (downDown) {
			moveSelectionDown();

			Thread check = new Thread(checkHold);
			Move m = new Move(ref down, moveSelectionDown);
			object obj = m;
			check.Start(obj);
		}

		bool upDown = Input.GetKeyDown(KeyCode.UpArrow);
		up = Input.GetKey(KeyCode.UpArrow);

		if (upDown) {
			moveSelectionUp();
		}
	}

	private void checkHold (object str) {
		Move move = (Move)str;

		Thread.Sleep(300);

		while (move.b) {
			move.method();
			Thread.Sleep(50);
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

	public void onClickSettings () {
		SceneManager.LoadScene("Settings");
	}

	public void onClickQuit () {
		Application.Quit();
	}

}
