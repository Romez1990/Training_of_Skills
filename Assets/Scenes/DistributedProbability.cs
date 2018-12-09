using UnityEngine;

namespace Assets.Scenes {
	static class DistributedProbability {

		public static int RandomByProbability(int[] Probability) {
			int[] SumsProbability = new int[Probability.Length];
			for (int i = 0; i < SumsProbability.Length; ++i) {
				int Sum = 0;
				for (int j = 0; j < i + 1; ++j)
					Sum += Probability[j];
				SumsProbability[i] = Sum;
			}

			int RandomNumber = Random.Range(0, SumsProbability[SumsProbability.Length - 1]);
			for (int i = 0; i < SumsProbability.Length; ++i)
				if (RandomNumber < SumsProbability[i])
					return i;

			return 0; // But this never will happen
		}

	}
}
