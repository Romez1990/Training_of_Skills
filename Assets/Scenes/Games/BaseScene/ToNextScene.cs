namespace Assets.Scenes.Games.BaseScene {
	public enum GameMode { Mixed, Single }

	public struct ToNextScene {

		public int Score;
		public GameMode GameMode;

		public ToNextScene(int Score, GameMode GameMode) {
			this.Score = Score;
			this.GameMode = GameMode;
		}

	}
}
