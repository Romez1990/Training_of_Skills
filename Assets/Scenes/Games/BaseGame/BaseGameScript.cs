using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.Games.BaseGame {
	public class BaseGameScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			if (PlayingInfo.GameMode == null)
				PlayingInfo.GameMode = SceneManager.GetActiveScene().name;
		}

		#endregion

	}
}
