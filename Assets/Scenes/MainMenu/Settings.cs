using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scenes.MainMenu {
	[Serializable]
	public struct Settings {

		public static Settings CurrentSettings;

		#region Parameters

		public bool SoundIsOn;
		public bool TimeIsOn;

		private static void SetDefaultSettings() {
			CurrentSettings.SoundIsOn = true;
			CurrentSettings.TimeIsOn = true;
			CurrentSettings.Save();
		}

		#endregion

		#region Saving and loading

		private static readonly BinaryFormatter BinaryFormatter = new BinaryFormatter();
		private static readonly string PathToFile = Path.Combine(Functions.PathToData, "Settings.dat");

		public void Save() {
			Directory.CreateDirectory(Functions.PathToData);
			using (FileStream FileStream = new FileStream(PathToFile, FileMode.Create)) {
				BinaryFormatter.Serialize(FileStream, this);
				FileStream.Close();
			}
			CurrentSettings = this;
		}

		public static void Load() {
			if (!File.Exists(PathToFile)) {
				SetDefaultSettings();
				return;
			}

			using (FileStream FileStream = new FileStream(PathToFile, FileMode.Open)) {
				try {
					CurrentSettings = (Settings)BinaryFormatter.Deserialize(FileStream);
				} catch {
					SetDefaultSettings();
				} finally {
					FileStream.Close();
				}
			}
		}

		#endregion

	}
}
