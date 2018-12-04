using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scenes.Scoreboard {
	public class Table : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			Debug.Log("You earned " + ToNextScene.Score + " points");
			StartCoroutine(SendRequest("Unity", ToNextScene.Score));
		}

		private static IEnumerator SendRequest(string Name, int Score) {
			Dictionary<string, string> Parameters = new Dictionary<string, string> {
				{ "username", Name },
				{ "score", Score.ToString() }
			};
			UnityWebRequest Request = UnityWebRequest.Post("http://u9895013.beget.tech/scoreboard.php", Parameters);
			Request.SetRequestHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");

			yield return Request.SendWebRequest();

			if (Request.isNetworkError || Request.isHttpError) {
				Debug.Log(Request.error);
			} else {
				SetTable(Encoding.Default.GetString(Request.downloadHandler.data));
			}
		}

		[Serializable]
		public class Record {
			public int position;
			public string username;
			public int score;
			public string datetime;
		}

		private static void SetTable(string Json) {
			Record[] Records = JsonHelper.FromJson<Record>(Json);
			for (int i = 0; i < Records.Length; i++) {
				Debug.Log(Records[i].position);
				Debug.Log(Records[i].username);
				Debug.Log(Records[i].score);
				Debug.Log(Records[i].datetime);
			}
		}

		#endregion

	}
}
