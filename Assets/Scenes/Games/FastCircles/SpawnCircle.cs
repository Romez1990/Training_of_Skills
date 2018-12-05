using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.FastCircles {
	public class SpawnCircle : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			SetCircles(15);
		}

		public GameObject CirclePrefab;

		private void SetCircles(int Amount) {
			RectTransform GamePanelRectTransform = GetComponent<RectTransform>();

			var Width = GamePanelRectTransform.rect.width;
			var Height = GamePanelRectTransform.rect.height;

			for (int i = 0; i < Amount; ++i) {
				GameObject Circle = Instantiate(CirclePrefab, transform);
				RectTransform CircleRectTransform = Circle.GetComponent<RectTransform>();

				int Diameter = Random.Range(30, 125);
				CircleRectTransform.sizeDelta = new Vector2(Diameter, Diameter);

				CircleRectTransform.localPosition = new Vector3(
					Random.Range(-(Width / 2 - Diameter / 2f), Width / 2 - Diameter / 2f),
					Random.Range(-(Height / 2 - Diameter / 2f), Height / 2 - Diameter / 2f),
					0
				);

				var Image = Circle.GetComponent<Image>();
				Image.color = Color.HSVToRGB(Random.Range(0, 1000) / 1000f, 1f, 1f);
			}
		}

	}
}
