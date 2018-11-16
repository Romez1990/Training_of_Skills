using JetBrains.Annotations;
using System;
using Assets.Scenes.Games.BaseScene;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.FastMath {
	public class FastMathScript : BaseSceneScript {

		[UsedImplicitly]
		private void Start() {
			BaseStart();
			Expression.text = GetExpression();
		}

		public Text Expression;
		public (int FirstNumber, char Operation, int SecondNumber, int Answer) ExpressionElements;
		public InputField UserAnswer;
		public GameObject UserAnswerInput;
		public Image Indicator;
		public Sprite TrueIndicator;
		public Sprite FalseIndicator;

		#region Get expression

		private string GetExpression() {
			ExpressionElements = SetNumbers(ExpressionElements.Operation = GetSign());

			string expression = string.Empty;
			expression += ExpressionElements.FirstNumber;
			expression += " ";
			expression += ExpressionElements.Operation;
			expression += " ";
			expression += ExpressionElements.SecondNumber;
			expression += " = ";
			return expression;
		}

		private readonly char[] signs = { '+', '-', '×', '÷' };
		private readonly int[] signsProbabitily = { 25, 25, 25, 25 };

		private char GetSign() {
			return signs[DistributedProbability.RandomByProbabitity(signsProbabitily)];
		}

		private readonly System.Random random = new System.Random();

		private (int, char, int, int) SetNumbers(char operation) {
			int a, b, answer;

			switch (operation) {
				case '+':
					a = random.Next(20, 51);
					b = random.Next(20, 51);
					answer = a + b;
					break;

				case '-':
					a = random.Next(20, 100);
					b = random.Next(20, a + 1);
					answer = a - b;
					break;

				case '×':
					a = random.Next(5, 21);
					b = random.Next(5, 21);
					answer = a * b;
					break;

				case '÷':
					int c = random.Next(5, 13);
					b = random.Next(5, 13);
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

		[UsedImplicitly]
		private void Update() {
			BaseUpdate();
		}

		#region Pause

		protected override void Pause() {
			UserAnswer.text = PreviousAnswer2;
			PreviousAnswer2 = PreviousAnswer1;
			
			bool pause = PausePanel.activeSelf;
			Blur.SetActive(!pause);
			PausePanel.SetActive(!pause);
		}

		private string PreviousAnswer1;
		private string PreviousAnswer2;

		#endregion

		#region Check answer

		public void onTextChanged() {
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
				GameObject.Find("CorrectSound").GetComponent<AudioSource>().Play();

				Win(20, 50);
			}

			color.a = 1;
			Indicator.color = color;
		}

		#endregion

	}
}
