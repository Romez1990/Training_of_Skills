using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseGame {
	public class TimeControl : MonoBehaviour {

		public static Text Time;
		public static float GivenTime;

		[UsedImplicitly]
		private void Start() {
			if (PlayingInfo.Time == 0)
				PlayingInfo.Time = 300;

			Time = GetComponent<Text>();
			GivenTime = PlayingInfo.Time;
		}

		[UsedImplicitly]
		private void Update() {
			TickTimer();
		}

		private static int IntTime;

		private void TickTimer() {
			if (BaseGameScript.IsPause) { return; }

			PlayingInfo.Time -= UnityEngine.Time.deltaTime;
			DisplayTime();
		}

		public static void DisplayTime() {
			if (IntTime == (int)PlayingInfo.Time) { return; }

			if (PlayingInfo.Time < 0.2f) {
				BaseGameScript.GameOver();
				return;
			}

			IntTime = (int)PlayingInfo.Time;
			int Second = (int)PlayingInfo.Time % 60;
			int Minute = (int)PlayingInfo.Time / 60;
			Time.text = (Minute < 10 ? "0" : string.Empty) + Minute +
							 ":" +
							 (Second < 10 ? "0" : string.Empty) + Second;
		}

	}
}
