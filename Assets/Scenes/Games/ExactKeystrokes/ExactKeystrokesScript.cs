using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.ExactKeystrokes {
	public class ExactKeystrokesScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			Task = transform.GetChild(1).GetComponent<Text>();
			Left = transform.GetChild(0).GetComponent<Text>();
			GetKeyList();
			DisplayKey(CurrentKeys[0].ToString());
			DisplayLeft(CurrentKeys.Count.ToString());
		}

		private static readonly List<KeyCode> AllKeys = new List<KeyCode> {
			KeyCode.A,
			KeyCode.B,
			KeyCode.C,
			KeyCode.D,
			KeyCode.E,
			KeyCode.F,
			KeyCode.G,
			KeyCode.H,
			KeyCode.I,
			KeyCode.J,
			KeyCode.K,
			KeyCode.M,
			KeyCode.L,
			KeyCode.N,
			KeyCode.O,
			KeyCode.P,
			KeyCode.Q,
			KeyCode.R,
			KeyCode.S,
			KeyCode.T,
			KeyCode.U,
			KeyCode.V,
			KeyCode.W,
			KeyCode.X,
			KeyCode.Y,
			KeyCode.Z
		};
		private static List<KeyCode> CurrentKeys;

		private static void GetKeyList() {
			CurrentKeys = new List<KeyCode>();
			for (int i = 0; i < 10; ++i) {
				int Index = Random.Range(0, AllKeys.Count);
				CurrentKeys.Add(AllKeys[Index]);
			}
		}

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			CheckKeystroke();
		}

		private static void CheckKeystroke() {
			if (Input.GetKeyDown(CurrentKeys[0])) {
				Enqueue();
			} else {
				CheckOtherKeys();
			}
		}

		private static void Enqueue() {
			CurrentKeys.RemoveAt(0);

			if (CurrentKeys.Count == 0) {
				Functions.Win();
				return;
			}

			DisplayKey(CurrentKeys[0].ToString());
			DisplayLeft(CurrentKeys.Count.ToString());
		}

		private static void CheckOtherKeys() {
			foreach (KeyCode Key in AllKeys) {
				if (Input.GetKeyDown(Key)) {
					TimeControl.TakeTime(5);
					break;
				}
			}
		}

		#endregion

		private static Text Task;

		private static void DisplayKey(string Key) {
			Task.text = $"Press {Key} key";
		}

		private static Text Left;

		private static void DisplayLeft(string Quantity) {
			Left.text = $"Left: {Quantity}";
		}

	}
}
