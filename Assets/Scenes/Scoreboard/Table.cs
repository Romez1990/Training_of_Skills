using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scenes.Scoreboard {
	public class Table : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			ToNextScene.Name = "Unity";
			ToNextScene.Score = 10000;
			StartCoroutine(SendRequest(ToNextScene.Name, ToNextScene.Score));
		}

		[Serializable]
		public class Record {
			public string position;
			public string name;
			public string score;
			public string datetime;
		}

		private IEnumerator SendRequest(string Name, int Score) {
			Dictionary<string, string> Parameters = new Dictionary<string, string> {
				{ "name", Name },
				{ "score", Score.ToString() }
			};
			UnityWebRequest Request = UnityWebRequest.Post("http://u9895013.beget.tech/scoreboard.php", Parameters);
			Request.SetRequestHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");

			yield return Request.SendWebRequest();

			if (Request.isNetworkError || Request.isHttpError) {
				Debug.Log(Request.error);
			} else {
				FigureOutTable(Encoding.Default.GetString(Request.downloadHandler.data));
			}
		}

		private List<Record> Records;
		private int Current;

		private void FigureOutTable(string Json) {
			Records = JsonHelper.FromJson<Record>(Json).ToList();

			if (Records.Count == 10)
				if (Convert.ToInt32(Records[9].position) > 10)
					Records.Insert(9, new Record { position = "..." });

			for (int i = 0; i <= Records.Count - 1; ++i) {
				if (ToNextScene.Name == Records[i].name && ToNextScene.Score.ToString() == Records[i].score) {
					Current = i;
					break;
				}
			}

			SetTable();
		}

		public GameObject RowPrefab;

		private void SetTable() {
			for (int i = 1; i < transform.childCount; ++i)
				Destroy(transform.GetChild(i).gameObject);

			for (int i = 1; i <= Records.Count; ++i) {
				GameObject Row = Instantiate(RowPrefab, transform);
				Row.name = "Row" + i;

				RectTransform RectTransform = Row.GetComponent<RectTransform>();
				RectTransform.anchorMin = new Vector2(0, 1 - (i + 1) / 12f);
				RectTransform.anchorMax = new Vector2(1, 1 - i / 12f);

				Row.transform.GetChild(0).GetComponent<Text>().text = Records[i - 1].position;
				Row.transform.GetChild(1).GetComponent<Text>().text = Records[i - 1].name;
				Row.transform.GetChild(2).GetComponent<Text>().text = Records[i - 1].score;
				Row.transform.GetChild(3).GetComponent<Text>().text = Records[i - 1].datetime;
			}

			Color[] Top = new Color[3];
			Top[0] = new Color(255 / 255f, 215 / 255f, 1 / 255f, 1); // Gold
			Top[1] = new Color(192 / 255f, 192 / 255f, 192 / 255f, 1); // Silver
			Top[2] = new Color(236 / 255f, 106 / 255f, 22 / 255f, 1); // Bronze
			for (int i = 0; i < 3; ++i)
				for (int j = 0; j < 4; ++j)
					transform.GetChild(i).GetChild(j).GetComponent<Text>().color = Top[i];

			transform.GetChild(Current + 2).gameObject.AddComponent<Image>().color = new Color(255, 255, 255, 0.12f);
			// Yeah, + 2 This is a fucking bug
			//Debug.Log(transform.GetChild(1));
			//transform.GetChild(1).gameObject.AddComponent<Image>().color = Color.red;
		}

	}

	#endregion

}
