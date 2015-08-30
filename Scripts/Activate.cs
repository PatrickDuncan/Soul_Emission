using UnityEngine;

public class Activate : MonoBehaviour {

	private Light accessLight;				// Reference to the access' light.
	private Light doorLight;				// Reference to the door's light.
	private Transform player;				// Reference to the player's transform.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	private Transform theTransform;			// Reference to the Transform.
	public AudioClip activateClip;			// Clip for when the player shoots.

	private void Start () {
		theTransform = transform;
		accessLight = GetComponentInChildren<Light>();
		//char i = transform.tag[transform.tag.Length-1];
		doorLight = GameObject.FindWithTag("Door").GetComponentInChildren<Light>();
		player = GameObject.FindWithTag("Player").transform;
		custom = GameObject.FindWithTag("Scripts").GetComponent<CustomPlayClipAtPoint>();
	}
	
	private void OnMouseDown () {
		Enable();
	}

	private void OnTouchStart () {
		Enable();
	}

	private void Enable () {
		if (Functions.DeltaMax(player.position.x, theTransform.position.x, 4f) && Functions.DeltaMax(player.position.y, theTransform.position.y, 2f)) { 
			accessLight.enabled = !accessLight.enabled;
		 	doorLight.enabled = !doorLight.enabled;
			custom.PlayClipAt(activateClip, theTransform.position);
		}
	}		
}
