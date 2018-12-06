using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scenes.Games.FastMath {
	public class FastMathScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			ExpressionElements = SetNumbers(ExpressionElements.Operation = GetSign());
			Expression.text = GetExpression();
		}

		public Text Expression;
		private (int FirstNumber, char Operation, int SecondNumber, int Answer) ExpressionElements;
		public InputField UserAnswer;
		public Image Indicator;
		public Sprite TrueIndicator;
		public Sprite FalseIndicator;

		#region Get expression

		private string GetExpression() {
			string NewExpression = string.Empty;
			NewExpression += ExpressionElements.FirstNumber;
			NewExpression += " ";
			NewExpression += ExpressionElements.Operation;
			NewExpression += " ";
			NewExpression += ExpressionElements.SecondNumber;
			NewExpression += " = ";
			return NewExpression;
		}

		private readonly char[] Signs = { '+', '-', '×', '÷' };
		private readonly int[] SignsProbability = { 25, 25, 25, 25 };

		private char GetSign() {
			return Signs[DistributedProbability.RandomByProbabitity(SignsProbability)];
		}

		private static (int, char, int, int) SetNumbers(char operation) {
			int a, b, answer;

			switch (operation) {
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
					a = b = answer = 0;
					break;
			}
			return (a, operation, b, answer);
		}

		#endregion

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			if (!BaseGameScript.IsPause) {
				EventSystem.current.SetSelectedGameObject(UserAnswer.gameObject);
				UserAnswer.ActivateInputField();
			}
		}

		#region Check answer

		[UsedImplicitly]
		public void OnTextChanged() {
			Color color = Indicator.color;

			if (UserAnswer.text == string.Empty) {
				color.a = 0;
				Indicator.color = color;
				return;
			}

			if (Convert.ToInt32(UserAnswer.text) != ExpressionElements.Answer) {
				Indicator.sprite = FalseIndicator;
			} else {
				Indicator.sprite = TrueIndicator;
				BaseGameScript.Win(20, 50);
			}

			color.a = 1;
			Indicator.color = color;
		}

		#endregion

		#endregion

	}
}
