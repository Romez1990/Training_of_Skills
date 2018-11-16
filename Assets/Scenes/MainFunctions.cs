using System;
using Assets.Scenes.Games.FastCircles;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Assets.Scenes {
	public class MainFunctions {

		private static readonly string[] Games = {
			"FastMath",
			"FastCircles"
		};

		public static void LoadRandomGame() {
			SceneManager.LoadScene(Games[Random.Range(0, Games.Length)]);
			PlayerPrefs.SetString("Mode", "Mixed");
			PlayerPrefs.SetInt("Score", 0);
		}

		public static void GameOver() {
			Debug.Log("GameOver");

			SceneManager.LoadScene("Scoreboard");
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
