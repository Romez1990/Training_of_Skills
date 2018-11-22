using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scenes.Games.BaseScene {
	public enum GameMode { Single, Mixed }

	[Serializable]
	public struct ToNextScene {

		public int Score;
		public GameMode GameMode;

		public ToNextScene(int Score, GameMode GameMode) {
			this.Score = Score;
			this.GameMode = GameMode;
		}

		private static readonly BinaryFormatter BinaryFormatter = new BinaryFormatter();
		private static readonly string PathToFle = Path.Combine(MainFunctions.PathToData, "ToNextScene.dat");

		public static void Save(ToNextScene ToNextScene) {
			Directory.CreateDirectory(MainFunctions.PathToData);
			FileStream FileStream = new FileStream(PathToFle, FileMode.Create);
			BinaryFormatter.Serialize(FileStream, ToNextScene);
			FileStream.Close();
		}

		public static ToNextScene Load() {
			if (!File.Exists(PathToFle)) {
				return new ToNextScene();
			}

			FileStream FileStream = new FileStream(PathToFle, FileMode.Open);
			ToNextScene ToNextScene;
			try {
				ToNextScene = (ToNextScene)BinaryFormatter.Deserialize(FileStream);
			} catch {
				ToNextScene = new ToNextScene();
			}
			FileStream.Close();

			return ToNextScene;
		}

		public static void Delete() {
			File.Delete(PathToFle);
		}
	}
}
