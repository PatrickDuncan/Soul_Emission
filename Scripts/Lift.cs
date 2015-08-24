using UnityEngine;
using System.Collections;

public class Lift : MonoBehaviour {		

	private Transform theTransform;			// Reference to the Transform.
	private Rigidbody2D rigid;				// Reference to the player's rigid body.
	private PolygonCollider2D poly;			// Reference to the player's polygon collider.
	private Transform player;				// Reference to the player's transform.
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private PlayerHealth playerH;			// Reference to the PlayerHealth script.
	private Animator anim;					// Reference to the Animator component.
	private Gun gun;						// Reference to the Gun class.
	public AudioClip liftClip;				// Clip for when the lift is activated.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	private void Awake () {
		theTransform = transform;
		GameObject gO  = GameObject.FindWithTag("Player");
		rigid = gO.GetComponent<Rigidbody2D>();
		poly = gO.GetComponent<PolygonCollider2D>();
		playerCtrl = gO.GetComponent<PlayerControl>();
		playerH = gO.GetComponent<PlayerHealth>();
		gun = gO.GetComponentInChildren<Gun>();
		player = gO.transform;
		anim = GetComponent<Animator>();
		custom = GameObject.FindWithTag("Scripts").GetComponent<CustomPlayClipAtPoint>();
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
			gun.allowedToShoot = false;
			rigid.velocity = new Vector2(0, 0);	
			StartCoroutine(WaitForBeam());
			custom.PlayClipAt(liftClip, theTransform.position);
		}
	}

	private void OnTriggerExit2D (Collider2D col) {
		if (!playerCtrl.allowedToBeam) {
			rigid.gravityScale = 1.8f;
			poly.isTrigger = false;
			gun.allowedToShoot = true;
			playerCtrl.isBeam = false;
			anim.SetTrigger("IdleBeam");
		}
	}	
	
	private IEnumerator WaitForBeam () {
		yield return new WaitForSeconds(4.5f);
		playerCtrl.allowedToBeam = true;
	}	
}
