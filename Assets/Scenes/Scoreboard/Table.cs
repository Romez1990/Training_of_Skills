using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scenes.Scoreboard {
	public class Table : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			ToNextScene.Username = "Unity";
			ToNextScene.Score = 16;
			StartCoroutine(SendRequest(ToNextScene.Username, ToNextScene.Score));
			SetRows();
			AddHighLightToRows();
		}

		private void AddHighLightToRows() {
			for (int i = 1; i < transform.childCount; ++i) {
				List<EventTrigger.Entry> Triggers = transform.GetChild(i).gameObject.AddComponent<EventTrigger>().triggers;

				EventTrigger.Entry Entry1 = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
				Entry1.callback.AddListener(delegate { transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.95f); });
				Triggers.Add(Entry1);

				EventTrigger.Entry Entry2 = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
				Entry2.callback.AddListener(delegate { transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0); });
				Triggers.Add(Entry2);
			}
		}

		public GameObject RowPrefab;
		private static Text[,] Cells;

		private void SetRows() {
			for (int i = 1; i < transform.childCount; ++i)
				Destroy(transform.GetChild(i).gameObject);

			Cells = new Text[11, 4];
			for (int i = 1; i <= 11; ++i) {
				GameObject Row = Instantiate(RowPrefab, transform);
				Row.name = "Row" + i;

				RectTransform RectTransform = Row.GetComponent<RectTransform>();
				RectTransform.anchorMin = new Vector2(0, 1 - (i + 1) / 12f);
				RectTransform.anchorMax = new Vector2(1, 1 - i / 12f);

				for (int j = 0; j < 4; j++) {
					Cells[i - 1, j] = Row.transform.GetChild(j).GetComponent<Text>();
				}
			}

			Color Gold = new Color(255 / 255f, 215 / 255f, 1 / 255f, 1);
			Color Silver = new Color(192 / 255f, 192 / 255f, 192 / 255f, 1);
			Color Bronze = new Color(236 / 255f, 106 / 255f, 22 / 255f, 1);
			for (int i = 0; i < 4; i++) {
				Cells[0, i].color = Gold;
				Cells[1, i].color = Silver;
				Cells[2, i].color = Bronze;
			}
		}

		private IEnumerator SendRequest(string Name, int Score) {
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
				FigureOutTable(Encoding.Default.GetString(Request.downloadHandler.data));
			}
		}

		[Serializable]
		public class Record {
			public string position;
			public string username;
			public string score;
			public string datetime;
		}

		private void FigureOutTable(string Json) {
			bool Found = false;
			int Current = 0;
			Record[] Records = JsonHelper.FromJson<Record>(Json);
			for (int i = 0; i < Records.Length - 1; ++i) {
				Cells[i, 0].text = Records[i].position;
				Cells[i, 1].text = Records[i].username;
				Cells[i, 2].text = Records[i].score;
				Cells[i, 3].text = Records[i].datetime;

				if (ToNextScene.Username == Records[i].username && ToNextScene.Score.ToString() == Records[i].score) {
					Found = true;
					Current = i;
				}
			}

			bool AddDots = Convert.ToInt32(Records[9].position) > 10;
			if (AddDots) {
				Cells[9, 0].text = "...";
				Cells[10, 0].text = Records[9].position;
				Cells[10, 1].text = Records[9].username;
				Cells[10, 2].text = Records[9].score;
				Cells[10, 3].text = Records[9].datetime;
			} else {
				Cells[9, 0].text = Records[9].position;
				Cells[9, 1].text = Records[9].username;
				Cells[9, 2].text = Records[9].score;
				Cells[9, 3].text = Records[9].datetime;
			}

			transform.GetChild(Found ? Current : AddDots ? 11 : 10).gameObject.AddComponent<Image>().color = new Color(255, 255, 255, 0.15f);
		}
	}

	#endregion

}
