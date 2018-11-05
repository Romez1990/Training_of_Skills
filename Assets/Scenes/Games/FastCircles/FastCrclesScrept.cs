using Assets.Scenes.Games.BaseScene;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scenes.Games.FastCircles {
	public class FastCrclesScrept : BaseSceneScript {

		[UsedImplicitly]
		void Start() {
			SetCircles(20);
		}

		public GameObject Circle;
		public GameObject GamePanel;

		private void SetCircles(int Amount) {
			Debug.Log(GamePanel.transform.position);
			
			for (int i = 0; i < Amount; i++) {
				float Radius = Random.Range(10, 20);

				Instantiate(Circle, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10)), Quaternion.identity, GamePanel.transform);
			}
		}

		[UsedImplicitly]
		void Update() {
			BaseUpdate();
		}

	}
}
