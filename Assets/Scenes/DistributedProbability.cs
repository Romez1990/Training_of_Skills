using System;

namespace Assets.Scenes.Games.FastMath {
	static class DistributedProbability {
		private static readonly Random random = new Random();

		public static int RandomByProbabitity(int[] probability) {
			int[] sumsProbability = new int[probability.Length];

			for (int i = 0; i < sumsProbability.Length; ++i) {
				int sum = 0;

				for (int j = 0; j < i + 1; j++) {
					sum += probability[j];
				}

				sumsProbability[i] = sum;
			}

			int randomNumber = random.Next(sumsProbability[sumsProbability.Length - 1]);

			int index = 0;

			for (int i = 0; i < sumsProbability.Length; ++i) {
				if (randomNumber >= sumsProbability[i]) { continue; }
				index = i;
				break;
			}

			return index;
		}
	}
}
