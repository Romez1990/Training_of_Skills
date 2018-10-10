using UnityEngine;
using UnityEngine.EventSystems;

public class Buttons : MonoBehaviour {

	void Start () {
		addEvents();
	}

	private void addEvents () {
		EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(data => { OnPointerDownDelegate((PointerEventData)data); });
		eventTrigger.triggers.Add(entry);
	}

	private void OnPointerDownDelegate (PointerEventData data) {
		EventSystem.current.currentSelectedGameObject = gameObject;
		Debug.Log("OnPointerEnterDelegate called");
	}

	void Update () {

	}
}
