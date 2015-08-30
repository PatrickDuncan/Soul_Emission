using UnityEngine;
using System.Collections;

public class Lift : MonoBehaviour {		

	private static bool found;						// If beam 1 has been found, for efficiency.

	private Transform theTransform;			// Reference to the Transform.
	private Rigidbody2D rigid;				// Reference to the player's rigid body.
	private PolygonCollider2D poly;			// Reference to the player's polygon collider.
	private Transform player;				// Reference to the player's transform.
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private PlayerHealth playerH;			// Reference to the PlayerHealth script.
	private Animator anim;					// Reference to the Animator component.
	public AudioClip liftClip;				// Clip for when the lift is activated.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	private void Awake () {
		theTransform = transform;
		GameObject gO  = GameObject.FindWithTag("Player");
		rigid = gO.GetComponent<Rigidbody2D>();
		poly = gO.GetComponent<PolygonCollider2D>();
		playerCtrl = gO.GetComponent<PlayerControl>();
		playerH = gO.GetComponent<PlayerHealth>();
		player = gO.transform;
		anim = GetComponent<Animator>();
		custom = GameObject.FindWithTag("Scripts").GetComponent<CustomPlayClipAtPoint>();
		if (!found && gameObject.name.Equals("beam 1")) {
			found = true;
			anim.SetTrigger("HelpBeam");
		}
	}

	public void OnMouseDown () {
		Beam();
	}

	private void OnTouchStart () {
		Beam();
	}

	private void Beam () {
		if (GetComponent<PolygonCollider2D>().IsTouching(poly) && playerCtrl.allowedToBeam && !playerCtrl.isGhost && !playerH.isDead) {
			playerCtrl.allowedToBeam = false;
			playerCtrl.isBeam = true;
			if (theTransform.position.y < player.position.y) {
				anim.SetTrigger("BeamDown");
				rigid.gravityScale = 0.1f;
			}
			else {
				anim.SetTrigger("BeamUp");
				rigid.gravityScale = -0.1f;
			}
			poly.isTrigger = true;
			playerCtrl.allowedToShoot = false;
			rigid.velocity = new Vector2(0, 0);	
			StartCoroutine(WaitForBeam());
			custom.PlayClipAt(liftClip, theTransform.position);
		}
	}

	private void OnTriggerExit2D (Collider2D col) {
		if (!playerCtrl.allowedToBeam) {
			rigid.gravityScale = 1.8f;
			poly.isTrigger = false;
			playerCtrl.allowedToShoot = true;
			playerCtrl.isBeam = false;
			anim.SetTrigger("IdleBeam");
		}
	}	
	
	private IEnumerator WaitForBeam () {
		yield return new WaitForSeconds(4.5f);
		playerCtrl.allowedToBeam = true;
	}	
}
