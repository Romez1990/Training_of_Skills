using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseGame {
	public class TimerControl : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			StartTimer();
		}

		public Text Timer;
		public static int GivenTime;
		public static float TimeLeft;
		private int LastTime;

		private void StartTimer() {
			GivenTime = 15;
			TimeLeft = GivenTime;
		}

		[UsedImplicitly]
		private void Update() {
			TickTimer();
		}

		private void TickTimer() {
			if (PauseControl.IsPause) { return; }

			TimeLeft -= Time.deltaTime;
			if (LastTime == (int)TimeLeft) { return; }

			if (TimeLeft < 0.2) {
				BaseGameScript.GameOver();
				BaseGameScript.GameIsOver = true;
			}

			int Second = (int)TimeLeft % 60;
			int Minute = ((int)TimeLeft - Second) / 60;
			Timer.text = (Minute < 10 ? "0" : string.Empty) + Minute +
							 ":" +
							 (Second < 10 ? "0" : string.Empty) + Second;

			LastTime = (int)TimeLeft;
		}


	}
}
