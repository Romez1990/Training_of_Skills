using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
			Material.SetFloat("size", BlurSizeLerp);
			isPause = false;
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
			Material.SetFloat("size", BlurSizeLerp);
		}

		#endregion

		private static GameObject GamePanel;

		private static bool isPause;
		public static bool IsPause {
			get => isPause;
			set {
				if (value == isPause) { return; }

				if (value) {
					BlurSize = 5;
					PausePanel.SetActive(true);
					GamePanel.SetActive(false);
					PauseMenu.PreviousSelected = null;
					EventSystem.current.SetSelectedGameObject(null);
				} else {
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					BlurSize = 0;
				}

				isPause = value;
			}
		}

		[UsedImplicitly]
		public void ClickPause() {
			IsPause = true;
		}

	}
}
