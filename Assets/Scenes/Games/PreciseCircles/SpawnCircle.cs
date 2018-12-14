using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.PreciseCircles {
	public class SpawnCircle : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			SetCircles();
			//StartCoroutine(Log());
		}

		private IEnumerator Log() {
			Debug.Log(new string('\t', 5) + $"{Mathf.RoundToInt(0.008f * Mathf.Pow(PlayingInfo.Score, 0.8f)) + 8}\t{Mathf.RoundToInt(PlayingInfo.Score)}");
			yield return new WaitForSeconds(1f);
			Functions.Win();
		}

		public GameObject CirclePrefab;

		private void SetCircles() {
			int Amount = Mathf.RoundToInt(0.008f * Mathf.Pow(PlayingInfo.Score, 0.8f)) + 8;

			RectTransform GamePanelRectTransform = GetComponent<RectTransform>();

			float Width = GamePanelRectTransform.rect.width;
			float Height = GamePanelRectTransform.rect.height;

			for (int i = 0; i < Amount; ++i) {
				GameObject Circle = Instantiate(CirclePrefab, transform);
				RectTransform CircleRectTransform = Circle.GetComponent<RectTransform>();

				int Diameter = Random.Range(
					300 / Amount + 10,
					1250 / Amount + 12
				);
				CircleRectTransform.sizeDelta = new Vector2(Diameter, Diameter);

				CircleRectTransform.localPosition = new Vector3(
					Random.Range(-(Width / 2 - Diameter / 2f), Width / 2 - Diameter / 2f),
					Random.Range(-(Height / 2 - Diameter / 2f), Height / 2 - Diameter / 2f),
					0
				);

				Image Image = Circle.GetComponent<Image>();
				Image.color = Color.HSVToRGB(Random.Range(0, 1000) / 1000f, 1f, 1f);
			}
		}

	}
}
