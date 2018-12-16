using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.Games.CircleByCircle {
	public class CircleByCircleScript : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			CircleNumberWas = Mathf.RoundToInt(0.008f * Mathf.Pow(PlayingInfo.Score, 0.8f)) + 6;
			CircleLeft = CircleNumberWas;

			RectTransform RectTransform = GetComponent<RectTransform>();
			Width = RectTransform.rect.width;
			Height = RectTransform.rect.height;

			Left = transform.parent.GetChild(0).GetComponent<Text>();

			SpawnCircle();

			//StartCoroutine(Log());
		}

		private IEnumerator Log() {
			Debug.Log(new string('\t', 5) + $"{Mathf.RoundToInt(0.008f * Mathf.Pow(PlayingInfo.Score, 0.8f)) + 6}\t{Mathf.RoundToInt(PlayingInfo.Score)}");
			yield return new WaitForSeconds(1f);
			Functions.Win();
		}

		public GameObject CirclePrefab;
		private static int CircleNumberWas;
		private static float Width;
		private static float Height;

		private void SpawnCircle() {
			--CircleLeft;
			DisplayLeft(CircleLeft);
			GameObject Circle = Instantiate(CirclePrefab, transform);

			RectTransform CircleRectTransform = Circle.GetComponent<RectTransform>();
			int Diameter = Random.Range(
				300 / CircleNumberWas + 10,
				1250 / CircleNumberWas + 12
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

		private void AddClickHandler(GameObject Circle) {
			EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
			entry.callback.AddListener(delegate { RemoveCircle(); });
			Circle.AddComponent<EventTrigger>().triggers.Add(entry);
		}

		private void RemoveCircle() {
			Destroy(transform.GetChild(0).gameObject);
			CheckWin();
		}

		private static int CircleLeft;

		private void CheckWin() {
			if (CircleLeft == 0)
				Functions.Win();
			else
				SpawnCircle();
		}

		private static Text Left;

		private static void DisplayLeft(int Number) {
			Left.text = $"Left: {Number + 1}";
		}

	}
}
