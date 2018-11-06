using Assets.Scenes.Games.BaseScene;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.FastCircles {
	public class FastCrclesScrept : BaseSceneScript {

		#region

		[UsedImplicitly]
		private void Start() {
			if (PlayerPrefs.GetString("Mode") == "Mixed") {
				Score.GetComponent<Text>().text = PlayerPrefs.GetInt("Score").ToString();
			}
		}

		public GameObject Score;

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			BaseUpdate();
		}

		#endregion

	}
}
