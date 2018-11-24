using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.MainMenu.Select {
	public class SelectScript : MonoBehaviour {

		public GameObject PanelPrefab;
		public Sprite[] SceneImages;

		[UsedImplicitly]
		private void Start() {
			for (int i = 0; i < transform.childCount; i++)
				Destroy(transform.GetChild(i).gameObject);

			for (int i = 0; i < MainFunctions.Games.Length; i++) {
				GameObject Panel = Instantiate(PanelPrefab, transform);

				RectTransform PanelRectTransform = Panel.GetComponent<RectTransform>();
				PanelRectTransform.anchorMin = new Vector2(i % 3 / 3f, 0.5f);
				PanelRectTransform.anchorMax = new Vector2((i + 1) % 3 / 3f, 1);

				GameObject SecondPanel = Panel.transform.GetChild(0).gameObject;

				if (i < SceneImages.Length) {
					GameObject SceneImage = SecondPanel.transform.GetChild(0).gameObject;
					Image Image = SceneImage.GetComponent<Image>();
					Image.sprite = SceneImages[i];
				}

				GameObject NameScene = SecondPanel.transform.GetChild(1).gameObject;
				Text Name = NameScene.GetComponent<Text>();
				Name.text = MainFunctions.Games[i];
			}
		}

	}
}
