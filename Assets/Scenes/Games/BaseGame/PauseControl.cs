using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.Games.BaseGame {
	public class PauseControl : MonoBehaviour {

		#region Start

		private static GameObject PausePanel;
		private static Material Material;
		private static float BlurSize;
		private static float BlurSizeLerp;

		[UsedImplicitly]
		private void Start() {
			GamePanel = transform.parent.parent.GetChild(1).gameObject;
			PausePanel = transform.GetChild(0).gameObject;
			Material = PausePanel.GetComponent<Image>().material;
			BlurSizeLerp = 0;
			BlurSize = 0;
			Material.SetFloat("_Size", BlurSizeLerp);
		}

		#endregion

		#region Update

		[UsedImplicitly]
		private void Update() {
			CheckPause();
		}

		private static void CheckPause() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				IsPause = !IsPause;
			}
		}

		#endregion

		#region Fixed update

		[UsedImplicitly]
		private void FixedUpdate() {
			EquateBlur();
		}

		private static void EquateBlur() {
			if (Mathf.Abs(BlurSizeLerp - BlurSize) < 0.1f / 4) { return; }

			BlurSizeLerp = Mathf.Lerp(BlurSizeLerp, BlurSize, 0.1f);
			Material.SetFloat("_Size", BlurSizeLerp);
		}

		#endregion

		private static GameObject GamePanel;

		private static bool _IsPause;
		public static bool IsPause {
			get => _IsPause;
			set {
				if (value == _IsPause) { return; }

				BlurSize = value ? 5 : 0;
				EventSystem.current.SetSelectedGameObject(null);
				PauseMenu.PreviousSelected = null;
				PausePanel.SetActive(value);
				GamePanel.SetActive(!value);
				_IsPause = value;
			}
		}

		[UsedImplicitly]
		public void ClickPause() {
			IsPause = true;
		}

	}
}
