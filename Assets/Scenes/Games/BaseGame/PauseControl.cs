using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseGame {
	public class PauseControl : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			Size = 0;
			_Size = 0;
			Material = GetComponent<Image>().material;
			Material.SetFloat("_Size", Size);
		}

		private float Size;
		private float _Size;
		private Material Material;

		[UsedImplicitly]
		private void Update() {
			CheckPause();
		}

		protected void CheckPause() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				TogglePause();
			}
		}

		public static bool IsPause = false;

		protected virtual void TogglePause() {
			IsPause = !IsPause;

			_Size = IsPause ? 5 : 0;

			transform.GetChild(0).gameObject.SetActive(IsPause);
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

	}
}
