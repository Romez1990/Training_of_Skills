using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scenes.MainMenu {
	public class LabelIndent : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			RectTransform RectTransform = GetComponent<RectTransform>();
			RectTransform.offsetMin = new Vector2(1.25f * RectTransform.rect.height, 0);
		}

	}
}
