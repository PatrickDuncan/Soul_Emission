using UnityEngine;
using System.Collections;

public class Activate : MonoBehaviour {

	private new Light light;

	private void Awake () {
		light = GetComponent<Light>();
	}
	
	private void OnMouseDown () {
		light.enabled = !light.enabled;
	}

	private void OnTouchStart () {
		light.enabled = !light.enabled;
	}		
}
