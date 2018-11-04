using UnityEngine;

namespace Assets.Scenes.Games.BaseScene {
	public class BaseSceneScript : MonoBehaviour {

		protected void BaseUpdate() {
			CheckPause();
		}

		#region Pause

		protected void CheckPause() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Pause();
			}
		}

		[SerializeField]
		protected GameObject Blur;
		[SerializeField]
		protected GameObject PausePanel;
		private bool IsPause = false;

		protected virtual void Pause() {
			Debug.Log("Pause");
			Blur.SetActive(!IsPause);
			PausePanel.SetActive(!IsPause);
			IsPause = !IsPause;

			//BlurMaterial.GetFloat("Size");
			//BlurMaterial.SetFloat("Size", 2.8f);
		}

		#endregion

	}
}
