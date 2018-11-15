using UnityEngine;
using UnityEngine.SceneManagement;

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

		public static void Win() {
			Debug.Log("Win");

			if (PlayerPrefs.GetString("Mode") == "Mixed") {
				LoadRandomGame();
			} else if (PlayerPrefs.GetString("Mode") == "Single") {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}

		public static void GameOver() {
			Debug.Log("GameOver");
		}

	}
}
