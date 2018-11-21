using System.Web.Script.Serialization;
using System.IO;
using Functions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseScene {
	public class BaseSceneScript : MonoBehaviour {

		protected void BaseStart() {
			Debug.Log("Start");

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
			Debug.Log("Pause");
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

		public float GivenTime = 7;
		public float TimeLeft;
		private Text TimerText;
		private int LastTime;
		public bool GameIsOver = false;

		private void TickTimer() {
			//Debug.Log(GameIsOver);
			//if (GameIsOver) { return; }

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

		#region Score

		public GameObject Score;
		private Text ScoreText;

		private void ScoreStart() {
			ScoreText = Score.GetComponent<Text>();
			ScoreText.text = PlayerPrefs.GetInt("Score").ToString();

		}

		public void SetScore(int Score) {
			ScoreText.text = Score.ToString();
		}

		#endregion

		#region Win

		public void Win(int BaseScore, int TimeScore) {
			Debug.Log("Win");

			int NewScore = PlayerPrefs.GetInt("Score") + MainFunctions.CalculateAddScore(BaseScore, TimeScore, GivenTime, TimeLeft);
			PlayerPrefs.SetInt("Score", NewScore);
			SetScore(NewScore);

			if (PlayerPrefs.GetString("Mode") == "Mixed") {
				MainFunctions.LoadRandomGame();
			} else if (PlayerPrefs.GetString("Mode") == "Single") {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}

		private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();


		private static void Write(ToNextScene ToNextScene) {
			string JSON = Serializer.Serialize(ToNextScene);
			string СipherText = Encryption.Encrypt(JSON);
			File.WriteAllText(@"ToNextScene.dat", СipherText);
		}

		private static ToNextScene Read() {
			try {
				string CipherText = File.ReadAllText(@"ToNextScene.dat");
				string JSON = Encryption.Decrypt(CipherText);
				return Serializer.Deserialize<ToNextScene>(JSON);
			} catch {
				return new ToNextScene();
			}
		}

		#endregion

	}
}
