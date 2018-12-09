using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseGame {
	public class ScoreControl : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			ScoreStart();
		}

		private static Text Score;

		private void ScoreStart() {
			Score = GetComponent<Text>();
			SetScore();
		}

		public static void SetScore() {
			Score.text = "Score: " + (PlayingInfo.Score > 0 ? PlayingInfo.Score.ToString("#,#").Replace(',', ' ') : "0");
		}

		private const int BaseScore = 50;
		private const int TimeScore = 100;

		public static void CalculateAddScore() {
			float PercentTime = PlayingInfo.Time / TimeControl.GivenTime;
			int CalcScore = Mathf.RoundToInt(TimeScore * PercentTime);
			PlayingInfo.Score += (BaseScore + CalcScore) * (1 + PlayingInfo.Score / 300);
			SetScore();
		}

	}
}
