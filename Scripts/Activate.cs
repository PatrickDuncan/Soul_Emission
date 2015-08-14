using UnityEngine;
using System.Collections;

public class Activate : MonoBehaviour {

	private Light accessLight;				// Reference to the access' light.
	private Light doorLight;				// Reference to the door's light.
	private Transform player;				// Reference to the player's transform.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	public AudioClip activateClip;			// Clip for when the player shoots.

	private void Awake () {
		accessLight = GetComponent<Light>();
		//char i = transform.tag[transform.tag.Length-1];
		doorLight = GameObject.FindWithTag("Door").GetComponent<Light>();
		player = GameObject.FindWithTag("Player").transform;
		custom = GameObject.FindWithTag("Background").GetComponent<CustomPlayClipAtPoint>();
	}
	
	private void OnMouseDown () {
		Enable();
	}

	private void OnTouchStart () {
		Enable();
	}

	private void Enable () {
		if (Mathf.Abs(player.position.x - transform.position.x) < 5f && Mathf.Abs(player.position.y - transform.position.y) < 2f) { 
			accessLight.enabled = !accessLight.enabled;
		 	doorLight.enabled = !doorLight.enabled;
			custom.PlayClipAt(activateClip, transform.position);
		}
	}		
}
