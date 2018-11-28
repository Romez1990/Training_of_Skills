using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseScene {
	public class BaseSceneScript : MonoBehaviour {

		protected void BaseStart() {
			StartTimer();
			ScoreStart();
		}

		protected void BaseUpdate() {
			CheckPause();
			TickTimer();
		}

		#region Pause

		protected void CheckPause() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Pause();
			}
		}

		[SerializeField]
		protected GameObject Blur;
		[SerializeField]
		protected GameObject PausePanel;
		private bool IsPause = false;

		protected virtual void Pause() {
			Blur.SetActive(!IsPause);
			PausePanel.SetActive(!IsPause);
			IsPause = !IsPause;

			//BlurMaterial.GetFloat("Size");
			//BlurMaterial.SetFloat("Size", 2.8f);
		}

		#endregion

		#region Timer

		public GameObject Timer;

		private void StartTimer() {
			TimerText = Timer.GetComponent<Text>();
			TimeLeft = GivenTime;
		}

		public float GivenTime = 5;
		public float TimeLeft;
		private Text TimerText;
		private int LastTime;
		public bool GameIsOver = false;

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
			TimerText.text = (Minute < 10 ? "0" : "") + Minute +
								  ":" +
								  (Second < 10 ? "0" : "") + Second;

			LastTime = (int)TimeLeft;
		}

		#endregion

		#region Start from previous scene

		public GameObject Score;
		private Text ScoreText;

		private void ScoreStart() {
			ScoreText = Score.GetComponent<Text>();

			SetScore(ToNextScene.Score);
		}

		public void SetScore(int Score) {
			ScoreText.text = Score.ToString();
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
	}
}
