using UnityEngine;
using System.Collections;

public class ToCamera : MonoBehaviour {
	private Transform theTransform; 		// Reference to the Transform.
	private new Transform camera;			// Reference the Main Camera's transform

	private void Awake () {
		theTransform = transform;
		camera = GameObject.FindWithTag("MainCamera").transform;
	}

	private void Update () {
		theTransform.position = new Vector3(camera.position.x, camera.position.y, 0f);
	}
	
	private void OnLevelWasLoaded(int level) {
        camera = GameObject.FindWithTag("MainCamera").transform;	// Brings object to the camera when level is loaded.
    }
}
