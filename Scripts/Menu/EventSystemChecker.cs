using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemChecker : MonoBehaviour {

	void OnLevelWasLoaded () {
		// If there is no EventSystem (needed for UI interactivity) present
		if(!FindObjectOfType<EventSystem>()) {
			GameObject obj = new GameObject("EventSystem");
			// And adds the required components
			obj.AddComponent<EventSystem>();
			obj.AddComponent<StandaloneInputModule>().allowActivationOnMobileDevice = true;
			obj.AddComponent<TouchInputModule>();
		}
	}
}
