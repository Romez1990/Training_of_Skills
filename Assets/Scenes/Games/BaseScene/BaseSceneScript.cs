using Assets.Scenes.Games.BaseScene.Pause;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseScene {
	public class BaseSceneScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			if (ToNextScene.GameMode == null)
				ToNextScene.GameMode = SceneManager.GetActiveScene().name;

			StartTimer();
		}

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			TickTimer();
		}

		#region Timer

		private void StartTimer() {
			TimeLeft = GivenTime;
		}

		public Text Timer;
		public int GivenTime = 15;
		public float TimeLeft;
		private int LastTime;
		protected bool GameIsOver = false;

		private void TickTimer() {
			if (PauseControl.IsPause) { return; }

			TimeLeft -= Time.deltaTime;
			if (LastTime == (int)TimeLeft) { return; }

			if (TimeLeft < 0.2) {
				MainFunctions.GameOver();
				GameIsOver = true;
			}

			int Second = (int)TimeLeft % 60;
			int Minute = ((int)TimeLeft - Second) / 60;
			Timer.text = (Minute < 10 ? "0" : string.Empty) + Minute +
							 ":" +
							 (Second < 10 ? "0" : string.Empty) + Second;

			LastTime = (int)TimeLeft;
		}

		#endregion

		#endregion

	}
}
