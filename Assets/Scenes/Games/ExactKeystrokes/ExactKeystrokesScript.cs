﻿using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System.Collections;
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
			DisplayLeft(CurrentKeys.Count);
			//StartCoroutine(Log());
		}

		private static IEnumerator Log() {
			Debug.Log(new string('\t', 5) + $"{Mathf.RoundToInt(0.008f * Mathf.Pow(PlayingInfo.Score, 0.8f)) + 8}\t{Mathf.RoundToInt(PlayingInfo.Score)}");
			yield return new WaitForSeconds(0.5f);
			Functions.Win();
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
			KeyCode.Z,
			KeyCode.Alpha1,
			KeyCode.Alpha2,
			KeyCode.Alpha3,
			KeyCode.Alpha4,
			KeyCode.Alpha5,
			KeyCode.Alpha6,
			KeyCode.Alpha7,
			KeyCode.Alpha8,
			KeyCode.Alpha9,
			KeyCode.Alpha0
		};

		private static List<KeyCode> CurrentKeys;

		private static void GetKeyList() {
			int Amount = Mathf.RoundToInt(0.008f * Mathf.Pow(PlayingInfo.Score, 0.8f)) + 8;

			System.Func<int, int, bool> CheckTheSame = (Index, i) => {
				if (i == 0) { return false; }
				return AllKeys[Index] == CurrentKeys[i - 1];
			};

			CurrentKeys = new List<KeyCode>();
			for (int i = 0; i < Amount; ++i) {
				int Index;
				do Index = Random.Range(0, AllKeys.Count);
				while (CheckTheSame(Index, i));
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
			DisplayLeft(CurrentKeys.Count);
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
			if (Key.Length != 1)
				Key = Key.Substring(Key.Length - 1, 1);

			Task.text = $"Press {Key} key";
		}

		private static Text Left;

		private static void DisplayLeft(int Number) {
			Left.text = $"Left: {Number}";
		}

	}
}
