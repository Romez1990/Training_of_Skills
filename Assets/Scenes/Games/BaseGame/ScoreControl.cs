using JetBrains.Annotations;
using System;
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
			SetScore(PlayingInfo.Score);
		}

		public static void SetScore(int NewScore) {
			Score.text = "Score: " + (NewScore > 0 ? NewScore.ToString("#,#").Replace(',', ' ') : "0");
		}

		private const int BaseScore = 50;
		private const int TimeScore = 100;

		public static int CalculateAddScore(float GivenTime) {
			float PercentTime = PlayingInfo.Time / GivenTime;
			int CalcScore = (int)Math.Round(TimeScore * PercentTime);
			int TotalScore = BaseScore + CalcScore;
			TotalScore += TotalScore * PlayingInfo.Score / 1000;
			return TotalScore;
		}

	}
}
