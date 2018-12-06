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
			if (PlayingInfo.GameMode == null)
				PlayingInfo.GameMode = SceneManager.GetActiveScene().name;
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
				IsPause = !IsPause;
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
				if (value == _IsPause) { return; }

				_Size = value ? 5 : 0;
				EventSystem.current.SetSelectedGameObject(null);
				PauseMenu.PreviousSelected = null;
				PausePanel.SetActive(value);
				_IsPause = value;
			}
		}

		[UsedImplicitly]
		public void PauseClick() {
			IsPause = true;
		}

		#endregion

		#region Game over

		public static void Win() {
			PlayingInfo.Time += 10;
			PlayingInfo.Score += ScoreControl.CalculateAddScore(TimeControl.GivenTime);

			Functions.LoadGame(PlayingInfo.GameMode);
		}

		public static void GameOver() {
			Functions.LoadGame("Scoreboard");
		}

		#endregion

	}
}
