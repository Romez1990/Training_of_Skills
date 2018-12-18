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

		[UsedImplicitly]
		private void Start() {
			StartCoroutine(SendRequest(PlayingInfo.Name, Mathf.RoundToInt(PlayingInfo.Score)));
		}

		private IEnumerator SendRequest(string Name, int Score) {
			transform.GetChild(2).GetComponent<Text>().text = "Please wait";

			Dictionary<string, string> Parameters = new Dictionary<string, string> {
				{ "name", Name },
				{ "score", Score.ToString() }
			};
			UnityWebRequest Request = UnityWebRequest.Post("http://s90012jj.beget.tech/Training_of_Skills/scoreboard.php", Parameters);
			Request.SetRequestHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");

			yield return Request.SendWebRequest();

			if (Request.isNetworkError)
				DisplayError("There is no internet connection to display scoreboard", Request.error);
			else if (Request.isHttpError)
				DisplayError("Something went wrong. We can't display scoreboard", Request.error);
			else
				Callback(Encoding.Default.GetString(Request.downloadHandler.data));
		}

		private void DisplayError(string ErrorText, string RequestError) {
			Debug.Log(RequestError);
			Transform CurrentRow = transform.GetChild(1);
			CurrentRow.gameObject.SetActive(true);
			CurrentRow.gameObject.AddComponent<Image>().color = new Color(255, 255, 255, 0.12f);
			CurrentRow.GetChild(0).GetComponent<Text>().text = "1";
			CurrentRow.GetChild(1).GetComponent<Text>().text = PlayingInfo.Name;
			CurrentRow.GetChild(2).GetComponent<Text>().text = Mathf.RoundToInt(PlayingInfo.Score).ToString();
			CurrentRow.GetChild(3).GetComponent<Text>().text = DateTime.Now.ToString("MM/dd/yyyy HH:mm").Replace('.', '/');
			transform.GetChild(2).GetComponent<Text>().text = ErrorText;
		}

		private void Callback(string Response) {
			FigureOutTable(Response);
			SetTable();
		}

		[Serializable]
		public class Record {
			public string position;
			public string name;
			public string score;
			public string datetime;
		}

		private List<Record> Records;
		private int? CurrentPlayer = null; // For highlight

		private void FigureOutTable(string Json) {
			Records = JsonArrayHelper.FromJson<Record>(Json).ToList();

			if (Records.Count == 10)
				if (int.Parse(Records[9].position) > 10)
					Records.Insert(9, new Record { position = "..." });

			for (int i = 0; i <= Records.Count - 1; ++i) {
				if (PlayingInfo.Name == Records[i].name && Mathf.RoundToInt(PlayingInfo.Score).ToString() == Records[i].score) {
					CurrentPlayer = i;
					break;
				}
			}
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
			for (int i = 0; i < (Records.Count >= 3 ? 3 : Records.Count); ++i)
				for (int j = 0; j < 4; ++j)
					transform.GetChild(i + 3).GetChild(j).GetComponent<Text>().color = Top[i];

			if (CurrentPlayer != null)
				transform.GetChild((int)CurrentPlayer + 3).gameObject.AddComponent<Image>().color = new Color(255, 255, 255, 0.12f);
			// Yeah, + 3 because there already was 3 elements
		}

	}

}
