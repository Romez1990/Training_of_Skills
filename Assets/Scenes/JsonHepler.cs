using System;
using UnityEngine;

namespace Assets.Scenes {
	public static class JsonHelper {

		[Serializable]
		private class Wrapper<T> {
			public T[] Items;
		}

		public static T[] FromJson<T>(string Json) {
			Wrapper<T> Wrapper = JsonUtility.FromJson<Wrapper<T>>("{\"Items\":" + Json + "}");
			return Wrapper.Items;
		}

		public static string ToJson<T>(T[] Array) {
			Wrapper<T> Wrapper = new Wrapper<T> { Items = Array };
			string Json = JsonUtility.ToJson(Wrapper);
			return Json.Substring(9, Json.Length - 10);
		}

	}
}
