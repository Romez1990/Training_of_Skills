using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseScene {
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

	}
}
