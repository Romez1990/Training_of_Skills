using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scenes.Games.BaseGame {
	public class PauseButtonResizer : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			RectTransform RectTransform = GetComponent<RectTransform>();
			RectTransform.offsetMax = new Vector2(RectTransform.rect.height, 0);
		}

	}
}
