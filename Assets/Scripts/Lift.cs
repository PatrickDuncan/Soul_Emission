using UnityEngine;
using System.Collections;

public class Lift : MonoBehaviour {		

	public bool allowedToBeam = true;		// If the player can use the beam.

	private Rigidbody2D rigid;				// Reference to the player's rigid body.
	private PolygonCollider2D poly;			// Reference to the player's polygon collider.
	private Transform player;				// Reference to the player's transform.
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.
	private Gun gun;						// Reference to the Gun class

	private void Awake () {
		rigid = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
		poly = GameObject.FindGameObjectWithTag("Player").GetComponent<PolygonCollider2D>();
		playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
		gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		anim = GetComponent<Animator>();
	}

	public void OnMouseDown () {
		if (GetComponent<PolygonCollider2D>().IsTouching(poly) && allowedToBeam && !playerCtrl.isGhost) {
			if (transform.position.y < player.position.y) {
				anim.SetTrigger("BeamDown");
				rigid.gravityScale = 0.1f;
			}
			else {
				anim.SetTrigger("BeamUp");
				rigid.gravityScale = -0.1f;		//Negative gravity makes it up up.
			}
			poly.isTrigger = true;
			gun.allowedToShoot = false;
			rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
			StartCoroutine(WaitForBeam());
		}
	}

	private void OnTouchStart () {
		if (GetComponent<PolygonCollider2D>().IsTouching(poly) && allowedToBeam && !playerCtrl.isGhost) {
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
			rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
			StartCoroutine(WaitForBeam());
		}
	}	

	private void OnTriggerExit2D(Collider2D col) {
		rigid.gravityScale = 1.8f;
		poly.isTrigger = false;
		anim.SetTrigger("IdleBeam");
		gun.allowedToShoot = true;
	}	
	
	private IEnumerator WaitForBeam () {
		allowedToBeam = false;
		yield return new WaitForSeconds(4f);
		allowedToBeam = true;
		rigid.constraints = RigidbodyConstraints2D.None;
		rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

	}	
}
