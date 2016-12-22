using UnityEngine;

public class AmmoPickUp : MonoBehaviour {

	private Transform player;				// Reference to the player's transform.
	public Sprite SMGSprite;				// Sprite for the SMG bullet.
	public Sprite pistolSprite;				// Sprite for the pistol bullet.
	public Sprite sniperSprite;				// Sprite for when the health is used.
	public AudioClip activateClip;			// Clip for when the player shoots.

	private Transform theTransform;			// Reference to the Transform.
	private Reset reset;					// Reference to the Reset class.
	private Gun gun;						// Reference to the Gun class in Player.	
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	private void Start () {
		theTransform = transform;
		GameObject gO = GameObject.FindWithTag("Player");
		player = gO.transform;
		gun = gO.GetComponentInChildren<Gun>();
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
			// Parent object has a sprite, so GetComponetInChildren doesn't work
			// Get all the components in the children and used the 1 element, which is the actual child.
			SpriteRenderer[] gO = gameObject.GetComponentsInChildren<SpriteRenderer>();
			if (gO[1].sprite == SMGSprite) { 
				gO[1].sprite = pistolSprite;
				gun.bulletType = (int)Gun.bullets.SMG;
				custom.PlayClipAt(activateClip, theTransform.position);
			}	
			else if (gO[1].sprite == pistolSprite) { 
				gO[1].sprite = sniperSprite;
				gun.bulletType = (int)Gun.bullets.Pistol;
				custom.PlayClipAt(activateClip, theTransform.position);
			}	
			else if (gO[1].sprite == sniperSprite) { 
				gO[1].sprite = SMGSprite;
				gun.bulletType = (int)Gun.bullets.Sniper;
				custom.PlayClipAt(activateClip, theTransform.position);
			}
		}
	}		
}
