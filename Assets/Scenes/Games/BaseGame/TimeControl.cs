using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseGame {
	public class TimeControl : MonoBehaviour {

		private Text Timer;
		public static float GivenTime;

		[UsedImplicitly]
		private void Start() {
			if (PlayingInfo.Time == 0)
				PlayingInfo.Time = 30;

			Timer = GetComponent<Text>();
			GivenTime = PlayingInfo.Time;
		}

		[UsedImplicitly]
		private void Update() {
			TickTimer();
		}

		private int LastTime;

		private void TickTimer() {
			if (BaseGameScript.IsPause) { return; }

			PlayingInfo.Time -= Time.deltaTime;

			if (LastTime == (int)PlayingInfo.Time) { return; }

			if (PlayingInfo.Time < 0.2f)
				BaseGameScript.GameOver();

			int Second = (int)PlayingInfo.Time % 60;
			int Minute = (int)PlayingInfo.Time / 60;
			Timer.text = (Minute < 10 ? "0" : string.Empty) + Minute +
							 ":" +
							 (Second < 10 ? "0" : string.Empty) + Second;

			LastTime = (int)PlayingInfo.Time;
		}
	}
}
