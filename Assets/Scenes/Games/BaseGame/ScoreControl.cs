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
			Score.text = ToNextScene.Score.ToString();
		}

		public static void SetScore(int NewScore) {
			Score.text = NewScore.ToString();
		}

		public static int CalculateAddScore(int BaseScore, int TimeScore, float GivenTime, float TimeLeft) {
			if (BaseScore < 0) { throw new ArgumentOutOfRangeException(nameof(BaseScore)); }
			if (TimeScore < 0) { throw new ArgumentOutOfRangeException(nameof(TimeScore)); }

			float PercentTime = TimeLeft / GivenTime;
			TimeScore = (int)Math.Round(TimeScore * PercentTime);
			return BaseScore + TimeScore;
		}

	}
}
