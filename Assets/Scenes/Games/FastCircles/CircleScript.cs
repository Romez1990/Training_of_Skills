using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scenes.Games.FastCircles {
	public class CircleScript : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			AddClickHendler();
		}

		private void AddClickHendler() {
			EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
			entry.callback.AddListener(RemoveCircle);
			gameObject.AddComponent<EventTrigger>().triggers.Add(entry);
		}

		private void RemoveCircle(BaseEventData e) {
			CheckWin();

			Destroy(gameObject);
		}

		private void CheckWin() {
			if (GameObject.Find("GamePanel").transform.childCount != 1) { return; }

			Win();
		}

		private void Win() {
			PlayerPrefs.GetInt("Score");

			MainFunctions.Win();
		}
	}
}