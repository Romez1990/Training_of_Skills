using Assets.Scenes.MainMenu;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scenes.Games.BaseGame.Sounds {
	public class Sound : MonoBehaviour {

		#region Start

		[UsedImplicitly]
		private void Start() {
			DestroyOrNot();
		}

		private static bool SoundExists = false;

		private void DestroyOrNot() {
			if (SoundExists) {
				Destroy(gameObject);
			} else {
				SoundExists = true;
				transform.SetParent(null);
				DontDestroyOnLoad(gameObject);
				SetSounds();
			}
		}

		private static AudioSource ClickGameObject;
		private static AudioSource MouseOverGameObject;
		private static AudioSource CorrectAnswerGameObject;
		private static AudioSource MistakeGameObject;
		private static AudioSource LoseGameObject;

		private void SetSounds() {
			ClickGameObject = transform.GetChild(0).GetComponent<AudioSource>();
			MouseOverGameObject = transform.GetChild(1).GetComponent<AudioSource>();
			CorrectAnswerGameObject = transform.GetChild(2).GetComponent<AudioSource>();
			MistakeGameObject = transform.GetChild(3).GetComponent<AudioSource>();
			LoseGameObject = transform.GetChild(4).GetComponent<AudioSource>();
		}

		private static bool CheckSoundIsOn() {
			return Settings.CurrentSettings.SoundIsOn;
		}

		public static void Click() {
			if (CheckSoundIsOn())
				ClickGameObject.Play();
		}

		public static void MouseOver() {
			if (CheckSoundIsOn())
				MouseOverGameObject.Play();
		}

		public static void CorrectAnswer() {
			if (CheckSoundIsOn())
				CorrectAnswerGameObject.Play();
		}

		public static void Mistake() {
			if (CheckSoundIsOn())
				MistakeGameObject.Play();
		}

		public static void Lose() {
			if (CheckSoundIsOn())
				LoseGameObject.Play();
		}

		#endregion

	}
}
