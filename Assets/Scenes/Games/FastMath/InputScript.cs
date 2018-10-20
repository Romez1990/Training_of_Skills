using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputScript : MonoBehaviour {

	[UsedImplicitly]
	void Start() {
		EventSystem.current.SetSelectedGameObject(gameObject);
	}

}
