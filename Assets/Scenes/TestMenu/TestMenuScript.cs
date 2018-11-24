using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scenes.Test {
	public class TestMenuScript : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			IntializingPanels();
		}

		#region Buttons and panels

		private GameObject[] Panels;
		private List<GameObject>[] Buttons;

		private void IntializingPanels() {
			Panels = new GameObject[transform.childCount];
			Buttons = new List<GameObject>[Panels.Length];

			for (int i = 0; i < Panels.Length; i++) {
				Panels[i] = transform.GetChild(i).gameObject;
				Buttons[i] = new List<GameObject>();

				for (int j = 0; j < Panels[i].transform.childCount; j++) {
					Buttons[i].Add(Panels[i].transform.GetChild(j).gameObject);
					AddEvents(Buttons[i][j]);
				}
			}
		}

		private void AddEvents(GameObject Button) {
			EventTrigger.Entry entry = new EventTrigger.Entry {
				eventID = EventTriggerType.PointerEnter
			};
			entry.callback.AddListener(eventData =>
				EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(Button));
			Button.AddComponent<EventTrigger>().triggers.Add(entry);
		}

		#endregion

	}
}
