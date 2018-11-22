using Assets.Scenes.Games.BaseScene;
using Functions;
using JetBrains.Annotations;
using System.IO;
using UnityEngine;

namespace Assets.Scenes.Test {
	public class TestWrite : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			Debug.Log("Write");
			//Write(new ToNextScene(555, GameMode.Single));
			Debug.Log("Write");
		}

		/*
		private static void Write(ToNextScene ToNextScene) {
			string JSON = Serializer.Serialize(ToNextScene);
			string СipherText = Encryption.Encrypt(JSON);
			File.WriteAllText(@"ToNextScene.dat", СipherText);
		}

		private static ToNextScene Read() {
			try {
				string CipherText = File.ReadAllText(@"ToNextScene.dat");
				string JSON = Encryption.Decrypt(CipherText);
				return Serializer.Deserialize<ToNextScene>(JSON);
			} catch {
				return new ToNextScene();
			}
		}
		*/

	}
}
