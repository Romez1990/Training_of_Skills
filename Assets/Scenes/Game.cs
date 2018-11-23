using UnityEngine.UI;

namespace Assets.Scenes {
	public struct Game {

		public string Name;
		public Image Image;

		public Game(string Name, Image Image) {
			this.Name = Name;
			this.Image = Image;
		}

	}
}
