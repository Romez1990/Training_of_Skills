using Assets.Scenes.Games.BaseScene;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scenes.Games.FastCircles {
	public class CircleScript : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			AddClickHandler();
		}

		private void AddClickHandler() {
			EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
			entry.callback.AddListener(delegate { RemoveCircle(); });
			gameObject.AddComponent<EventTrigger>().triggers.Add(entry);
		}

		private void RemoveCircle() {
			CheckWin();
			Destroy(gameObject);
		}

		private void CheckWin() {
			if (transform.parent.childCount == 1)
				Win();
		}

		private void Win() {
			GameObject.Find("BaseScene").GetComponent<BaseSceneScript>().Win(20, 50);
		}

	}
}