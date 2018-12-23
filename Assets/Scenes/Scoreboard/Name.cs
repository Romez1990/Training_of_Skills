using System.IO;

namespace Assets.Scenes.Scoreboard {
	public static class Name {

		private static readonly string PathToFile = Path.Combine(Functions.PathToData, "Name.dat");

		public static void Save(string NameString) {
			Directory.CreateDirectory(Functions.PathToData);
			File.WriteAllText(PathToFile, NameString);
		}

		public static string Load() {
			return File.Exists(PathToFile) ? File.ReadAllText(PathToFile) : string.Empty;
		}

	}
}
