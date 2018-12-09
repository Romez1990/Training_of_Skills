using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseGame {
	public class PauseControl : MonoBehaviour {

		#region Start

		private static GameObject PausePanel;
		private static Material Material;
		private static float Size;
		private static float SizeLerp;

		[UsedImplicitly]
		private void Start() {
			PausePanel = transform.GetChild(0).gameObject;
			Material = PausePanel.GetComponent<Image>().material;
			SizeLerp = 0;
			Size = 0;
			Material.SetFloat("_Size", SizeLerp);
		}

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			CheckPause();
		}

		#endregion

		#region Fixed update

		private static void CheckPause() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				IsPause = !IsPause;
			}
		}

		[UsedImplicitly]
		private void FixedUpdate() {
			EquateBlur();
		}

		private static void EquateBlur() {
			if (Math.Abs(SizeLerp - Size) < 0.1f) { return; }

			SizeLerp = Mathf.Lerp(SizeLerp, Size, 0.1f);
			Material.SetFloat("_Size", SizeLerp);
		}

		#endregion

		private static bool _IsPause = false;
		public static bool IsPause {
			get => _IsPause;
			set {
				if (value == _IsPause) { return; }

				Size = value ? 5 : 0;
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

	}
}
