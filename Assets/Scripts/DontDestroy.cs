using UnityEngine;

public class DontDestroy : MonoBehaviour {

	private void Awake () {
		// Object will not be destroyed when loading a new level/scene
		DontDestroyOnLoad(transform.gameObject);
	}	
}
