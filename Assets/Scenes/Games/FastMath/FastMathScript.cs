using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.Games.FastMath {
	public class FastMathScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			SignsProbability = new[] {
				Mathf.RoundToInt(170 / Mathf.Pow(PlayingInfo.Score / 300 + 2, 0.7f)),
				PlayingInfo.Score < 2000 ?
					Mathf.RoundToInt(150 / Mathf.Pow(PlayingInfo.Score / 300 + 2, 0.8f)) + 8 :
					Mathf.RoundToInt(0.6f * Mathf.Pow(1.5f, Mathf.Pow(PlayingInfo.Score * 0.8f, 0.3f))),
				Mathf.RoundToInt(0.7f * Mathf.Pow(1.5f, Mathf.Pow(PlayingInfo.Score, 0.3f))),
				Mathf.RoundToInt(0.6f * Mathf.Pow(1.5f, Mathf.Pow(PlayingInfo.Score * 0.48f, 0.33f)))
			};

			//SignsProbability = new[] { 0, 0, 0, 1 };

			ExpressionElements.Operation = GetSign();
			ExpressionElements = SetNumbers();
			Expression.text = GetExpression();
			//StartCoroutine(Log());
		}

		private IEnumerator Log() {
			//Debug.Log(new string('\t', 5) + $"{SignsProbability[0]}\t{SignsProbability[1]}\t{SignsProbability[2]}\t{SignsProbability[3]}\t\t{PlayingInfo.Score}");
			yield return new WaitForSeconds(2);
			Functions.Win();
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
			int a, b, answer, min, max, min_min, max_min;
			max = Mathf.RoundToInt(Mathf.Pow(1.5f, Mathf.Pow(PlayingInfo.Score, 0.33f)));
			min = Mathf.RoundToInt(Mathf.Pow(1.1f, Mathf.Pow(PlayingInfo.Score, 0.485f)));
			switch (ExpressionElements.Operation) {
				case '+':
					min = min < 2 ? 2 : min;
					min = min > 499 ? 499 : min;
					max = max < 10 ? 10 : max;
					max = max > 499 ? 499 : max;

					a = Random.Range(min, max);
					b = Random.Range(min, max);
					answer = a + b;
					break;

				case '-':
					max *= 2;

					min = min < 2 ? 2 : min;
					min = min > 799 ? 799 : min;
					max = max < 10 ? 10 : max;
					max = max > 999 ? 999 : max;

					a = Random.Range(min, max);
					b = Random.Range(min, a - 1);
					answer = a - b;
					break;

				case '×':
					min /= 100;
					max /= 100;

					min_min = 2;
					max_min = 5;
					min += min_min;
					max += max_min;

					min = min < min_min ? min_min : min;
					min = min > 20 ? 20 : min;
					max = max < max_min ? max_min : max;
					max = max > 31 ? 31 : max;

					a = Random.Range(min, max);
					b = Random.Range(min, max);
					answer = a * b;
					break;

				default: // '÷'
					min /= 300;
					max /= 300;

					min_min = 2;
					max_min = 5;
					min += min_min;
					max += max_min;

					min = min < min_min ? min_min : min;
					max = max < max_min ? max_min : max;
					if (PlayingInfo.Score < 20_000) {
						min = min > 10 ? 10 : min;
						max = max > 20 ? 20 : max;
					} else {
						min = min > 20 ? 20 : min;
						max = max > 31 ? 31 : max;
					}

					int c = Random.Range(min, max);
					b = Random.Range(min, max);
					a = b * c;
					answer = a / b;
					break;
			}
			//Debug.Log(new string('\t', 5) + $"{min}\t{max}");
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
				if (UserAnswer.text.Length == ExpressionElements.Answer.ToString().Length)
					TimeControl.TakeTime(4);
				else if (UserAnswer.text.Length > ExpressionElements.Answer.ToString().Length)
					TimeControl.TakeTime(7);
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
