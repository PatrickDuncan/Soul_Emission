using UnityEngine;
using System.Collections;

public class Activate : MonoBehaviour {

	private Light accessLight;
	private Light doorLight;
	private Transform player;

	public AudioClip activateClip;				// Clip for when the player shoots.

	private void Awake () {
		accessLight = GetComponent<Light>();
		char i = transform.tag[transform.tag.Length-1];
		doorLight = GameObject.FindGameObjectWithTag("Door"+i).GetComponent<Light>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
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
			AudioSource.PlayClipAtPoint(activateClip, transform.position);
		}
	}		
}
