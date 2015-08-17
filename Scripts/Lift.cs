using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Lift : MonoBehaviour {		

	public bool allowedToBeam = true;		// If the player can use the beam.

	private Rigidbody2D rigid;				// Reference to the player's rigid body.
	private PolygonCollider2D poly;			// Reference to the player's polygon collider.
	private Transform player;				// Reference to the player's transform.
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private PlayerHealth playerH;			// Reference to the PlayerHealth script.
	private Animator anim;					// Reference to the Animator component.
	private Gun gun;						// Reference to the Gun class
	public AudioClip liftClip;				// Clip for when the lift is activated.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	private void Awake () {
		rigid = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
		poly = GameObject.FindWithTag("Player").GetComponent<PolygonCollider2D>();
		playerCtrl = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
		playerH = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
		gun = GameObject.Find("Gun").GetComponent<Gun>();
		player = GameObject.FindWithTag("Player").transform;
		anim = GetComponent<Animator>();
		custom = GameObject.Find("Background").GetComponent<CustomPlayClipAtPoint>();
	}

	public void OnMouseDown () {
		Beam();
	}

	private void OnTouchStart () {
		Beam();
	}

	private void Beam() {
		if (GetComponent<PolygonCollider2D>().IsTouching(poly) && allowedToBeam && !playerCtrl.isGhost && !playerH.isDead) {
			allowedToBeam = false;
			playerCtrl.isBeam = true;
			if (transform.position.y < player.position.y) {
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
			custom.PlayClipAt(liftClip, transform.position);
		}
	}

	private void OnTriggerExit2D(Collider2D col) {
		if (!allowedToBeam) {
			rigid.gravityScale = 1.8f;
			poly.isTrigger = false;
			gun.allowedToShoot = true;
			playerCtrl.isBeam = false;
			anim.SetTrigger("IdleBeam");
		}
	}	
	
	private IEnumerator WaitForBeam () {
		yield return new WaitForSeconds(4.5f);
		allowedToBeam = true;
	}	
}
