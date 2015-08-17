using UnityEngine;
using System.Collections;

public class ToCamera : MonoBehaviour {
	
	private new Transform camera;			//Reference the Main Camera's transform

	private void Awake () {
		camera = GameObject.FindWithTag("MainCamera").transform;
	}

	private void OnLevelWasLoaded(int level) {
        camera = GameObject.FindWithTag("MainCamera").transform;
    }

	private void Update () {
		transform.position = new Vector3(camera.position.x, camera.position.y, 0f);
	}
}
