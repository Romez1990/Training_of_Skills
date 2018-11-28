using Assets.Scenes.Games.BaseScene;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scenes.Scoreboard {
	public class ScoreboardScript : MonoBehaviour {

		private ToNextScene ToNextScene;

		[UsedImplicitly]
		private void Start() {
			Debug.Log("Score" + ToNextScene.Score);
			ToNextScene.Reset();
		}

		[UsedImplicitly]
		private void Update() {
			
		}

	}
}
