using JetBrains.Annotations;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

	[UsedImplicitly]
	private void Start () {

	}

	[UsedImplicitly]
	private void Update () {
		checkKeyToMoveOn();
	}

	#region Moving selection

	private void checkKeyToMoveOn () {

		bool downDown = Input.GetKeyDown(KeyCode.DownArrow);
		bool down = Input.GetKey(KeyCode.DownArrow);
		bool downUp = Input.GetKeyUp(KeyCode.DownArrow);

		if (downDown) {
			moveSelectionDown();

			//Thread check = new Thread(checkHold);
			//check.Start(ref down, moveSelectionDown);
		}

		bool upDown = Input.GetKeyDown(KeyCode.UpArrow);
		bool up = Input.GetKey(KeyCode.UpArrow);
		bool upUp = Input.GetKeyUp(KeyCode.UpArrow);

		if (upDown) {
			moveSelectionUp();
		}
	}

	private void checkHold (ref bool isPressing, Action methodToMove) {
		Thread.Sleep(300);

		while (isPressing) {
			methodToMove();
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
