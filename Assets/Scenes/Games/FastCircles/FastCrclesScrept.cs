using Assets.Scenes.Games.BaseScene;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.FastCircles {
	public class FastCrclesScrept : BaseSceneScript {

		#region

		private float GivenTime = 7;
		private float TimeLeft;
		public GameObject Score;
		public GameObject Timer;

		[UsedImplicitly]
		private void Start() {
			TimerText = Timer.GetComponent<Text>();
			TimeLeft = GivenTime;

			Score.GetComponent<Text>().text = PlayerPrefs.GetInt("Score").ToString();
		}

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			BaseUpdate();
			TickTimer();
		}

		private Text TimerText;
		private int LastTime;

		private void TickTimer() {
			TimeLeft -= Time.deltaTime;
			if (LastTime == (int)TimeLeft) { return; }

			if (TimeLeft < 0.2) {
				GameOver();
			}

			int Second = (int)TimeLeft % 60;
			int Minute = ((int)TimeLeft - Second) / 60;
			TimerText.text = (Minute < 10 ? '0' + Minute.ToString() : Minute.ToString()) +
									':' +
								  (Second < 10 ? '0' + Second.ToString() : Second.ToString());

			LastTime = (int)TimeLeft;
		}

		#endregion

	}
}
