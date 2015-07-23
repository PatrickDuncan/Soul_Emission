using UnityEngine;
using System.Collections;

public class Activate : MonoBehaviour {

	private new Light light;
	private Transform player;

	private void Awake () {
		light = GetComponent<Light>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	private void OnMouseDown () {
		if (Mathf.Abs(player.position.x - transform.position.x) < 5f)
			light.enabled = !light.enabled;
	}

	private void OnTouchStart () {
		if (Mathf.Abs(player.position.x - transform.position.x) < 5f)
			light.enabled = !light.enabled;
	}		
}
