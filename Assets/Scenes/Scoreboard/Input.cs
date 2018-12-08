using Assets.Scenes.Games.BaseGame;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scenes.Scoreboard {
	public class Input : MonoBehaviour {

		[UsedImplicitly]
		private void Start() {
			InputField = transform.GetChild(0).GetComponent<InputField>();
			Warning = transform.GetChild(1).GetComponent<Text>();
		}

		[UsedImplicitly]
		public void Input_OnValueChanged() {
			Warning.text = string.Empty;
		}

		[UsedImplicitly]
		public void Input_OnEndEdit() {
			CheckName();
		}

		private InputField InputField;
		private Text Warning;

		[UsedImplicitly]
		public void Button_OnClick() {
			if (CheckName())
				ShowResults();
		}

		private bool CheckName() {
			if (InputField.text.Length == 0) {
				Warning.text = "Enter your name";
				InputField.ActivateInputField();
				return false;
			}

			if (InputField.text.Length < 4) {
				Warning.text = "The name is too short";
				InputField.ActivateInputField();
				return false;
			}

			return true;
		}

		private void ShowResults() {
			PlayingInfo.Name = InputField.text;
			transform.parent.GetChild(2).gameObject.SetActive(true);
			gameObject.SetActive(false);
		}

		private static GameObject PreviousSelected;

		[UsedImplicitly]
		private void Update() {
			EnterCheck();
			UnselectedCheck();
		}

		private void EnterCheck() {
			if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
				if (CheckName())
					ShowResults();
		}

		private void UnselectedCheck() {
			if (EventSystem.current.currentSelectedGameObject == null) {
				EventSystem.current.SetSelectedGameObject(
					PreviousSelected == null ?
						transform.GetChild(0).gameObject :
						PreviousSelected
				);
			}

			PreviousSelected = EventSystem.current.currentSelectedGameObject;
		}
	}
}
