using Assets.Scenes.Games.BaseGame.Sounds;
using Assets.Scenes.MainMenu;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseGame {
	public class TimeControl : MonoBehaviour {

		private static MonoBehaviour This;
		public static Text Time;
		public static float GivenTime;

		[UsedImplicitly]
		private void Start() {
			This = this;
			if (PlayingInfo.Time == 0)
				PlayingInfo.Time = 30;
			if (!Settings.CurrentSettings.TimeIsOn)
				PlayingInfo.Time = 100000000;

			Time = GetComponent<Text>();
			TimeColorLerp = DefaultTimeColor;
			GivenTime = PlayingInfo.Time;
			DisplayTime();
		}

		[UsedImplicitly]
		private void Update() {
			TickTimer();
		}

		private static int IntTime;

		private static void TickTimer() {
			if (PauseControl.IsPause) { return; }

			PlayingInfo.Time -= UnityEngine.Time.deltaTime;

			if (IntTime == (int)PlayingInfo.Time) { return; }

			DisplayTime();
		}

		public static void DisplayTime() {
			if (PlayingInfo.Time < 0.2f) {
				Time.text = "00:00";
				Functions.GameOver();
				return;
			}

			if (!Settings.CurrentSettings.TimeIsOn) {
				Time.text = "∞";
			} else {
				IntTime = (int)PlayingInfo.Time;
				int Second = (int)PlayingInfo.Time % 60;
				int Minute = (int)PlayingInfo.Time / 60;
				Time.text = (Minute < 10 ? "0" : string.Empty) + Minute +
								 ":" +
								(Second < 10 ? "0" : string.Empty) + Second;
			}
		}

		#region Take time

		private static readonly Color DefaultTimeColor = Color.white;
		private static Color TimeColorLerp;
		private const float Step = 0.08f;

		public static void TakeTime(float TimeToTake) {
			Sound.Mistake();
			PlayingInfo.Time = Mathf.Round(PlayingInfo.Time - TimeToTake);
			DisplayTime();
			Time.color = Color.red;
			TimeColorLerp = Color.red;
			This.Invoke("ResetTimeColor", 0.12f);
		}

		[UsedImplicitly]
		public void TakeTime_non_static(float TimeToTake) {
			TakeTime(TimeToTake);
		}

		[UsedImplicitly]
		private void ResetTimeColor() {
			TimeColorLerp = DefaultTimeColor;
		}

		[UsedImplicitly]
		private void FixedUpdate() {
			EquateTimeColor();
		}

		public static void EquateTimeColor() {
			/*Debug.Log($"R:\t{TimeColorLerp.r}\t{Time.color.r}");
			Debug.Log($"R:\t{TimeColorLerp.g}\t{Time.color.g}");
			Debug.Log($"R:\t{TimeColorLerp.b}\t{Time.color.b}");//*/

			if (Mathf.Abs(TimeColorLerp.r - Time.color.r) < Step / 4 &&
				 Mathf.Abs(TimeColorLerp.g - Time.color.g) < Step / 4 &&
				 Mathf.Abs(TimeColorLerp.b - Time.color.b) < Step / 4)
				return;

			Time.color = new Color(
				Mathf.Lerp(Time.color.r, TimeColorLerp.r, Step),
				Mathf.Lerp(Time.color.g, TimeColorLerp.g, Step),
				Mathf.Lerp(Time.color.b, TimeColorLerp.b, Step)
			);
		}

		#endregion

	}
}
