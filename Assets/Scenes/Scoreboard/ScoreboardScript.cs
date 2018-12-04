using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.Scoreboard {
	public class ScoreboardScript : MonoBehaviour {

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
				Debug.Log(Encoding.Default.GetString(Request.downloadHandler.data));
			}
		}

		#endregion

		#region Update

		/*
		[UsedImplicitly]
		private void Update() {

		}
		*/

		#endregion

		#region Clicks

		[UsedImplicitly]
		public void MainMenuClick() {
			SceneManager.LoadScene("MainMenu");
		}

		[UsedImplicitly]
		public void StartOverClick() {
			ToNextScene.Score = 0;

			if (ToNextScene.GameMode == "Mixed" || ToNextScene.GameMode == null)
				Functions.LoadRandomGame();
			else
				SceneManager.LoadScene(ToNextScene.GameMode);
		}

		#endregion

	}
}
