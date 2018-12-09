﻿using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scenes.Games.FastCircles {
	public class FastCirclesScript : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
			entry.callback.AddListener(delegate { TimeControl.TakeTime(2); });
			gameObject.AddComponent<EventTrigger>().triggers.Add(entry);
		}

	}
}
