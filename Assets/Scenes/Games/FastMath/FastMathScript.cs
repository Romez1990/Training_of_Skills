using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.Games.FastMath {
	public class FastMathScript : MonoBehaviour {

		#region Start

		private static readonly MathExp MathExp = new MathExp();

		[UsedImplicitly]
		private void Start() {
			Expression = transform.GetChild(0).GetComponent<Text>();
			UserAnswer = transform.GetChild(1).GetComponent<InputField>();
			UserAnswer.onValueChanged.AddListener(delegate { OnTextChanged(); });
			Indicator = transform.GetChild(2).GetComponent<Image>();
			SetExp();
			//StartCoroutine(Log());
		}

		private IEnumerator Log() {
			//Debug.Log(new string('\t', 5) + $"{SignProbabilities[0]}\t{SignProbabilities[1]}\t{SignProbabilities[2]}\t{SignProbabilities[3]}\t\t{PlayingInfo.Score}");
			yield return new WaitForSeconds(1);
			Functions.Win();
		}

		private Text Expression;

		private void SetExp() {
			MathExp.SignProbabilities = new[] {
				Mathf.RoundToInt(170 / Mathf.Pow(PlayingInfo.Score / 300 + 2, 0.7f)),
				PlayingInfo.Score < 2000 ?
					Mathf.RoundToInt(150 / Mathf.Pow(PlayingInfo.Score / 300 + 2, 0.8f)) + 8 :
					Mathf.RoundToInt(0.6f * Mathf.Pow(1.5f, Mathf.Pow(PlayingInfo.Score * 0.8f, 0.3f))),
				Mathf.RoundToInt(0.7f * Mathf.Pow(1.5f, Mathf.Pow(PlayingInfo.Score, 0.3f))),
				Mathf.RoundToInt(0.6f * Mathf.Pow(1.5f, Mathf.Pow(PlayingInfo.Score * 0.48f, 0.33f)))
			};
			//MathExp.SignProbabilities = new[] { 0, 1, 0, 0 };
			MathExp.SetSign();
			MathExp.SetNumbers();
			Expression.text = MathExp.ToString();
		}

		#endregion

		#region Update

		private InputField UserAnswer;

		[UsedImplicitly]
		private void Update() {
			if (!PauseControl.IsPause) {
				EventSystem.current.SetSelectedGameObject(UserAnswer.gameObject);
				UserAnswer.ActivateInputField();
			}
		}

		#endregion

		#region Check answer

		private Image Indicator;
		public Sprite TrueIndicator;
		public Sprite FalseIndicator;

		[UsedImplicitly]
		public void OnTextChanged() {
			Color color = Indicator.color;

			if (UserAnswer.text == string.Empty) {
				color.a = 0;
				Indicator.color = color;
				return;
			}

			if (int.Parse(UserAnswer.text) != MathExp.Answer) {
				Indicator.sprite = FalseIndicator;
				if (UserAnswer.text.Length == MathExp.Answer.ToString().Length)
					TimeControl.TakeTime(4);
				else if (UserAnswer.text.Length > MathExp.Answer.ToString().Length)
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
