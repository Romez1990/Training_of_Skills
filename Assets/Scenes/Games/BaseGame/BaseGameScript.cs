using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.Games.BaseGame {
	public class BaseGameScript : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			PausePanel = _PausePanel;
			StartPause();
			if (ToNextScene.GameMode == null)
				ToNextScene.GameMode = SceneManager.GetActiveScene().name;
		}

		public GameObject _PausePanel;
		private static GameObject PausePanel;
		private static float Size;
		private static float _Size;
		public Material Material;

		private void StartPause() {
			Size = 0;
			_Size = 0;
			Material.SetFloat("_Size", Size);
		}

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			CheckPause();
		}

		private void CheckPause() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				TogglePause();
			}
		}

		[UsedImplicitly]
		private void FixedUpdate() {
			ChangeBlur();
		}

		private void ChangeBlur() {
			if (Math.Abs(Size - _Size) < 0.1f) { return; }

			Size = Mathf.Lerp(Size, _Size, 0.1f);
			Material.SetFloat("_Size", Size);
		}

		#endregion

		#region Pause

		private static bool _IsPause = false;
		public static bool IsPause {
			get => _IsPause;
			set {
				if (value != _IsPause) {
					TogglePause();
				}
			}
		}

		private static void TogglePause() {
			_IsPause = !_IsPause;
			_Size = _IsPause ? 5 : 0;
			PausePanel.SetActive(_IsPause);
			EventSystem.current.SetSelectedGameObject(null);
			//EventSystem.current.SetSelectedGameObject(PauseMenu.Buttons[0]);
		}

		#endregion

		#region Game over

		public static bool GameIsOver = false;

		public static void Win(int BaseScore, int TimeScore) {
			ToNextScene.Score = ToNextScene.Score + ScoreControl.CalculateAddScore(BaseScore, TimeScore, TimerControl.GivenTime, TimerControl.TimeLeft);

			ScoreControl.SetScore(ToNextScene.Score);

			if (ToNextScene.GameMode == "Mixed")
				Functions.LoadRandomGame();
			else
				SceneManager.LoadScene(ToNextScene.GameMode);
		}

		public static void GameOver() {
			SceneManager.LoadScene("Scoreboard");
		}

		#endregion

	}
}
