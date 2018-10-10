using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scenes.MainMenu {
	public class Buttons : MonoBehaviour {

		[UsedImplicitly]
		void Start () {
			addEvents();
		}

		private void addEvents () {
			EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
			entry.callback.AddListener(eventData =>
				EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(gameObject));
			gameObject.AddComponent<EventTrigger>().triggers.Add(entry);
		}

		[UsedImplicitly]
		void Update () {

		}
	}
}
