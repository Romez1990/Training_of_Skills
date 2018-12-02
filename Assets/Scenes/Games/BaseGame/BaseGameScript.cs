using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.Games.BaseGame {
	public class BaseGameScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			GameIsOver = false;
			if (ToNextScene.GameMode == null)
				ToNextScene.GameMode = SceneManager.GetActiveScene().name;
		}

		#endregion

		public static bool GameIsOver;

		public static void Win(int BaseScore, int TimeScore) {
			ToNextScene.Score = ToNextScene.Score + ScoreControl.CalculateAddScore(BaseScore, TimeScore, TimerControl.GivenTime, TimerControl.TimeLeft);

			ScoreControl.SetScore(ToNextScene.Score);

			if (ToNextScene.GameMode == "Mixed")
				Functions.LoadRandomGame();
			else
				SceneManager.LoadScene(ToNextScene.GameMode);
		}

		public static void GameOver() {
			SceneManager.LoadScene("Scoreboard");
		}

	}
}
