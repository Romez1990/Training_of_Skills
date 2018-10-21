using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.FastMath {
	public class FastMathScript : MonoBehaviour {

		[UsedImplicitly]
		void Start() {
			SetExpression();
		}

		public Text Expression;
		public (int FirstNumber, char Operation, int SecondNumber, int Answer) ExpressionElements;
		public InputField UserAnswer;
		public Image Indicator;
		public Sprite TrueIndicator;
		public Sprite FalseIndicator;

		private void SetExpression() {
			string expression = string.Empty;

			ExpressionElements.Operation = GetSign();

			ExpressionElements = SetNumbers(ExpressionElements.Operation);

			expression += ExpressionElements.FirstNumber;
			expression += " ";
			expression += ExpressionElements.Operation;
			expression += " ";
			expression += ExpressionElements.SecondNumber;
			expression += " = ";

			Expression.text = expression;
		}

		private readonly char[] signs = { '+', '-', '×', '÷' };

		private char GetSign() {
			int[] signsProbabitily = { 25, 25, 25, 25 };
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

		[UsedImplicitly]
		void Update() {
			CheckPause();
		}

		private void CheckPause() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				UserAnswer.text = PreviousAnswer2;
				Pause();
			}
		}

		public GameObject Blur;
		public GameObject PausePanel;

		public void Pause() {
			Debug.Log("Pressed");
			Image BlurMaterial = Blur.GetComponent<Image>();
			//Shader a = BlurMaterial.material.shader;
			
			BlurMaterial.material.SetFloat("Size", 5);
		}

		private string PreviousAnswer1;
		private string PreviousAnswer2;

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
			}

			color.a = 1;
			Indicator.color = color;
		}

	}
}
