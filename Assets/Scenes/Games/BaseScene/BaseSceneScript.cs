using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseScene {
	public class BaseSceneScript : MonoBehaviour {

		#region Start

		protected void BaseStart() {
			ScoreStart();
			StartTimer();
		}

		#region From previous scene

		public Text Score;

		private void ScoreStart() {
			SetScore(ToNextScene.Score);
		}

		public void SetScore(int NewScore) {
			Score.text = NewScore.ToString();
		}

		#endregion

		#endregion

		#region Update

		protected void BaseUpdate() {
			TickTimer();
			CheckPause();
		}

		#region Timer

		private void StartTimer() {
			TimeLeft = GivenTime;
		}

		public Text Timer;
		protected float GivenTime = 5;
		protected float TimeLeft;
		private int LastTime;
		protected bool GameIsOver = false;

		private void TickTimer() {
			if (IsPause) { return; }

			TimeLeft -= Time.deltaTime;
			if (LastTime == (int)TimeLeft) { return; }

			if (TimeLeft < 0.2) {
				MainFunctions.GameOver();
				GameIsOver = true;
			}

			int Second = (int)TimeLeft % 60;
			int Minute = ((int)TimeLeft - Second) / 60;
			Timer.text = (Minute < 10 ? "0" : "") + Minute +
							 ":" +
							 (Second < 10 ? "0" : "") + Second;

			LastTime = (int)TimeLeft;
		}

		#endregion

		#region Pause

		protected void CheckPause() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Pause();
			}
		}

		public GameObject Blur;
		public GameObject PausePanel;
		protected bool IsPause = false;

		protected virtual void Pause() {
			Blur.SetActive(!IsPause);
			PausePanel.SetActive(!IsPause);
			IsPause = !IsPause;

			//BlurMaterial.GetFloat("Size");
			//BlurMaterial.SetFloat("Size", 2.8f);
		}

		#endregion

		#region Win

		public void Win(int BaseScore, int TimeScore) {
			ToNextScene.Score = ToNextScene.Score + MainFunctions.CalculateAddScore(BaseScore, TimeScore, GivenTime, TimeLeft);

			SetScore(ToNextScene.Score);

			if (ToNextScene.GameMode == "Mixed")
				MainFunctions.LoadRandomGame();
			else
				SceneManager.LoadScene(ToNextScene.GameMode);
		}

		#endregion

		#endregion

	}
}
