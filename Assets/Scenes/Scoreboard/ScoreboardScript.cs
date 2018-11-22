using Assets.Scenes.Games.BaseScene;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scenes.Scoreboard {
	public class ScoreboardScript : MonoBehaviour {

		private ToNextScene ToNextScene;

		[UsedImplicitly]
		private void Start() {
			ToNextScene = ToNextScene.Load();
			Debug.Log("Score" + ToNextScene.Score);
		}

		[UsedImplicitly]
		private void Update() {
			
		}

	}
}
