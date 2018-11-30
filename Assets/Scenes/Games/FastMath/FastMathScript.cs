using Assets.Scenes.Games.BaseScene;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scenes.Games.FastMath {
	public class FastMathScript : BaseSceneScript {

		#region Start

		[UsedImplicitly]
		private void Start() {
			BaseStart();
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
			BaseUpdate();
			UnselectCheck();
		}

		private void UnselectCheck() {
			if (EventSystem.current.currentSelectedGameObject == null)
				EventSystem.current.SetSelectedGameObject(UserAnswer.gameObject);
		}

		#region Pause

		private string PreviousAnswer1;
		private string PreviousAnswer2;

		/*
		protected override void Pause() {
			UserAnswer.text = PreviousAnswer2;
			PreviousAnswer2 = PreviousAnswer1;

			bool pause = PausePanel.activeSelf;
			Blur.SetActive(!pause);
			PausePanel.SetActive(!pause);
		}
		//*/

		#endregion

		#region Check answer

		[UsedImplicitly]
		public void OnTextChanged() {
			// For saving value to pause
			PreviousAnswer2 = PreviousAnswer1;
			PreviousAnswer1 = UserAnswer.text;

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
				Win(20, 50);
			}

			color.a = 1;
			Indicator.color = color;
		}

		#endregion

		#endregion

	}
}
