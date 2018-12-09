using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scenes.Games.FastCircles {
	public class FastCirclesScript : MonoBehaviour {

		private static readonly Color DefaultTimeColor = Color.white;

		[UsedImplicitly]
		private void Start() {
			TimeColorLerp = DefaultTimeColor;

			EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
			entry.callback.AddListener(delegate { TakeTime(); });
			gameObject.AddComponent<EventTrigger>().triggers.Add(entry);
		}

		private void TakeTime() {
			PlayingInfo.Time -= 2;
			TimeControl.DisplayTime();
			StartCoroutine(ColorTime());
		}

		private static IEnumerator ColorTime() {
			TimeControl.Time.color = Color.red;
			TimeColorLerp = Color.red;

			yield return new WaitForSeconds(0.12f);

			TimeColorLerp = DefaultTimeColor;
		}

		private static Color TimeColorLerp;
		private const float Step = 0.08f;

		[UsedImplicitly]
		private void FixedUpdate() {
			EquateTimeColor();
		}

		private static void EquateTimeColor() {
			/*Debug.Log($"R:\t{TimeColorLerp.r}\t{TimeControl.Time.color.r}");
			Debug.Log($"R:\t{TimeColorLerp.g}\t{TimeControl.Time.color.g}");
			Debug.Log($"R:\t{TimeColorLerp.b}\t{TimeControl.Time.color.b}");//*/

			if (Math.Abs(TimeColorLerp.r - TimeControl.Time.color.r) < Step / 4 &&
				 Math.Abs(TimeColorLerp.g - TimeControl.Time.color.g) < Step / 4 &&
				 Math.Abs(TimeColorLerp.b - TimeControl.Time.color.b) < Step / 4)
				return;

			TimeControl.Time.color = new Color(
				Mathf.Lerp(TimeControl.Time.color.r, TimeColorLerp.r, Step),
				Mathf.Lerp(TimeControl.Time.color.g, TimeColorLerp.g, Step),
				Mathf.Lerp(TimeControl.Time.color.b, TimeColorLerp.b, Step)
			);
		}

	}
}
