﻿using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.Games.FastMath {
	public class FastMathScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			SignsProbability = new[] { 25, 25, 25, 25 };
			ExpressionElements.Operation = GetSign();
			ExpressionElements = SetNumbers();
			Expression.text = GetExpression();
		}

		public Text Expression;
		private static (int FirstNumber, char Operation, int SecondNumber, int Answer) ExpressionElements;
		public InputField UserAnswer;
		public Image Indicator;
		public Sprite TrueIndicator;
		public Sprite FalseIndicator;

		#region Get expression

		private static string GetExpression() {
			return $"{ExpressionElements.FirstNumber} {ExpressionElements.Operation} {ExpressionElements.SecondNumber} = ";
		}

		private static readonly char[] Signs = { '+', '-', '×', '÷' };
		private int[] SignsProbability;

		private char GetSign() {
			return Signs[DistributedProbability.RandomByProbability(SignsProbability)];
		}

		private static (int, char, int, int) SetNumbers() {
			int a, b, answer;
			switch (ExpressionElements.Operation) {
				case '+':
					a = Random.Range(20, 51);
					b = Random.Range(20, 51);
					answer = a + b;
					break;

				case '-':
					a = Random.Range(20, 100);
					b = Random.Range(20, a + 1);
					answer = a - b;
					break;

				case '×':
					a = Random.Range(5, 21);
					b = Random.Range(5, 21);
					answer = a * b;
					break;

				case '÷':
					int c = Random.Range(5, 13);
					b = Random.Range(5, 13);
					a = b * c;
					answer = a / b;
					break;

				default:
					a = b = answer = 0; // But this never will happen
					break;
			}
			return (a, ExpressionElements.Operation, b, answer);
		}

		#endregion

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			if (!PauseControl.IsPause) {
				EventSystem.current.SetSelectedGameObject(UserAnswer.gameObject);
				UserAnswer.ActivateInputField();
			}
		}

		#endregion

		#region Check answer

		[UsedImplicitly]
		public void OnTextChanged() {
			Color color = Indicator.color;

			if (UserAnswer.text == string.Empty) {
				color.a = 0;
				Indicator.color = color;
				return;
			}

			if (int.Parse(UserAnswer.text) != ExpressionElements.Answer) {
				Indicator.sprite = FalseIndicator;
			} else {
				Indicator.sprite = TrueIndicator;
				Functions.Win();
			}

			color.a = 1;
			Indicator.color = color;
		}

		#endregion

	}
}
