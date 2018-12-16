using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.Games.PreciseCircles {
	public class PreciseCircleScript : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			Left = transform.parent.GetChild(0).GetComponent<Text>();
			SpawnCircles();
			//StartCoroutine(Log());
		}

		private IEnumerator Log() {
			Debug.Log(new string('\t', 5) + $"{Mathf.RoundToInt(0.008f * Mathf.Pow(PlayingInfo.Score, 0.8f)) + 8}\t{Mathf.RoundToInt(PlayingInfo.Score)}");
			yield return new WaitForSeconds(1f);
			Functions.Win();
		}

		public GameObject CirclePrefab;
		private static int CircleLeft;

		private void SpawnCircles() {
			CircleLeft = Mathf.RoundToInt(0.008f * Mathf.Pow(PlayingInfo.Score, 0.8f)) + 8;
			DisplayLeft(CircleLeft);

			RectTransform RectTransform = GetComponent<RectTransform>();
			float Width = RectTransform.rect.width;
			float Height = RectTransform.rect.height;

			for (int i = 0; i < CircleLeft; ++i) {
				GameObject Circle = Instantiate(CirclePrefab, transform);

				RectTransform CircleRectTransform = Circle.GetComponent<RectTransform>();
				int Diameter = Random.Range(
					300 / CircleLeft + 10,
					1250 / CircleLeft + 12
				);
				CircleRectTransform.sizeDelta = new Vector2(Diameter, Diameter);
				CircleRectTransform.localPosition = new Vector3(
					Random.Range(Diameter / 2f - Width / 2, Width / 2 - Diameter / 2f),
					Random.Range(Diameter / 2f - Height / 2, Height / 2 - Diameter / 2f),
					0
				);

				Circle.GetComponent<Image>().color = Color.HSVToRGB(Random.Range(0, 1000) / 1000f, 1f, 1f);

				AddClickHandler(Circle);
			}
		}

		private static void AddClickHandler(GameObject Circle) {
			EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
			entry.callback.AddListener(delegate { RemoveCircle(Circle); });
			Circle.AddComponent<EventTrigger>().triggers.Add(entry);
		}

		private static void RemoveCircle(GameObject Circle) {
			--CircleLeft;
			Destroy(Circle);
			CheckWin();
		}

		private static void CheckWin() {
			DisplayLeft(CircleLeft);
			if (CircleLeft == 0)
				Functions.Win();
		}

		private static Text Left;

		private static void DisplayLeft(int Number) {
			Left.text = $"Left: {Number}";
		}

	}
}