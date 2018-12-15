using Assets.Scenes.Games.BaseGame;
using UnityEngine;

namespace Assets.Scenes.Games.FastMath {
	public class MathExp {

		public int[] SignProbabilities { get; set; }
		private static readonly char[] Signs = { '+', '-', '×', '÷' };

		public int Number1 { get; set; }
		public char Sign { get; set; }
		public int Number2 { get; set; }
		public int Answer { get; set; }

		public char SetSign() {
			return Sign = Signs[DistributedProbability.RandomByProbability(SignProbabilities)];

		}

		public void SetNumbers() {
			int min = Mathf.RoundToInt(Mathf.Pow(1.5f, Mathf.Pow(PlayingInfo.Score, 0.33f))),
				 max = Mathf.RoundToInt(Mathf.Pow(1.1f, Mathf.Pow(PlayingInfo.Score, 0.485f))),
				 min_min,
				 max_min;

			switch (Sign) {
				case '+':
					min = min < 2 ? 2 : min;
					min = min > 499 ? 499 : min;
					max = max < 10 ? 10 : max;
					max = max > 499 ? 499 : max;

					Number1 = Random.Range(min, max);
					Number2 = Random.Range(min, max);
					Answer = Number1 + Number2;
					break;

				case '-':
					max *= 2;

					min = min < 2 ? 2 : min;
					min = min > 799 ? 799 : min;
					max = max < 10 ? 10 : max;
					max = max > 999 ? 999 : max;

					Number1 = Random.Range(min, max);
					Number2 = Random.Range(min, Number1 - 3);
					Answer = Number1 - Number2;
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

					Number1 = Random.Range(min, max);
					Number2 = Random.Range(min, max);
					Answer = Number1 * Number2;
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
					Number2 = Random.Range(min, max);
					Number1 = Number2 * c;
					Answer = Number1 / Number2;
					break;
			}
		}

		public override string ToString() {
			return $"{Number1} {Sign} {Number2} = ";
		}

	}
}
