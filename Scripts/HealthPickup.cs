using UnityEngine;

public class HealthPickup : MonoBehaviour {

	private Transform player;				// Reference to the player's transform.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.
	public Sprite usedSprite;				// Sprite for when the health is used.
	public AudioClip activateClip;			// Clip for when the player shoots.
	public bool used;						// If the health pick-up has been used.

	private Transform theTransform;			// Reference to the Transform.
	private PlayerHealth playerH;			// Reference to the PlayerHealth script.
	private Reset reset;					// Reference tot he Reset class.

	private void Start () {
		theTransform = transform;
		//char i = transform.tag[transform.tag.Length-1];
		GameObject gO = GameObject.FindWithTag("Player");
		player = gO.transform;
		playerH = gO.GetComponent<PlayerHealth>();
		custom = GameObject.FindWithTag("Scripts").GetComponent<CustomPlayClipAtPoint>();
	}
	
	private void OnMouseDown () {
		Enable();
	}

	private void OnTouchStart () {
		Enable();
	}

	public void Enable () {
		if (Functions.DeltaMax(player.position.x, theTransform.position.x, 4f) && Functions.DeltaMax(player.position.y, theTransform.position.y, 2f)) {
			if (!used) { 
				used = true;
				SpriteChange();
				custom.PlayClipAt(activateClip, theTransform.position);
				playerH.AddHealth();
			}	
		}
	}		

	public void SpriteChange () {
		GetComponent<SpriteRenderer>().sprite = usedSprite;
	}
}
