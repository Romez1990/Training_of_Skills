using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseScene {
	public class BaseSceneScript : MonoBehaviour {

		public GameObject Score;

		protected void BaseStart() {
			Score.GetComponent<Text>().text = PlayerPrefs.GetInt("Score").ToString();
			StartTimer();
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

		private float GivenTime = 7;
		private float TimeLeft;
		private Text TimerText;
		private int LastTime;

		private void TickTimer() {
			TimeLeft -= Time.deltaTime;
			if (LastTime == (int)TimeLeft) { return; }

			if (TimeLeft < 0.2) {
				MainFunctions.GameOver();
			}

			int Second = (int)TimeLeft % 60;
			int Minute = ((int)TimeLeft - Second) / 60;
			string sdfsadf = (Minute < 10 ? "0" : "") + Minute  +
								  ":" +
								  (Second < 10 ? "0" : "") + Second;
			TimerText.text = sdfsadf;

			LastTime = (int)TimeLeft;
		}

		#endregion

	}
}
