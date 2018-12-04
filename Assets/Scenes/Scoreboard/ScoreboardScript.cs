using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.Scoreboard {
	public class ScoreboardScript : MonoBehaviour {

		#region Update

		/*
		[UsedImplicitly]
		private void Update() {

		}
		*/

		#endregion

		#region Clicks

		[UsedImplicitly]
		public void MainMenuClick() {
			SceneManager.LoadScene("MainMenu");
		}

		[UsedImplicitly]
		public void StartOverClick() {
			ToNextScene.Score = 0;

			if (ToNextScene.GameMode == "Mixed" || ToNextScene.GameMode == null)
				Functions.LoadRandomGame();
			else
				SceneManager.LoadScene(ToNextScene.GameMode);
		}

		#endregion

	}
}
