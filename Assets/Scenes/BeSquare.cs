using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scenes {
	public class BeSquare : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			RectTransform RectTransform = GetComponent<RectTransform>();
			RectTransform.offsetMax = new Vector2(RectTransform.rect.height, 0);
		}

	}
}
