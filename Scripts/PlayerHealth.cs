using UnityEngine;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour {

	public readonly float HEALTH = 50f;		// The player's health maximum.
	[HideInInspector]			
	private float currentH;					// The player's current health.
	[HideInInspector]
	public bool isDead;						// If the player is dead.

	private Animator ripAnim;				// Reference to Rip's Animator
	private SpriteRenderer ripSprite;		// Reference to Rip's Sprite Renderer.
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator on the player.
	private Gun gun;						// Reference to the Gun class.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.
	private Reset reset;					// Reference to the Reset script.

	public AudioClip injuryClip;			// Clip for when the player gets injured.
	public AudioClip deathClip;				// Clip for when the player dies.

	private void Awake () {
		ripAnim = GameObject.Find("Rip").GetComponent<Animator>();
		ripSprite = GameObject.Find("Rip").GetComponent<SpriteRenderer>();
		playerCtrl = GetComponent<PlayerControl>();
		anim = GetComponent<Animator>();
		gun = GameObject.Find("Gun").GetComponent<Gun>();
		custom = GameObject.Find("Scripts").GetComponent<CustomPlayClipAtPoint>();
		reset = GameObject.Find("Scripts").GetComponent<Reset>();
		currentH = HEALTH;
	}	

	private void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag.Equals("Fire"))
			TakeDamage(1000f, false, false);		// Instantly die if you touch fire
	}

	public void TakeDamage (float damage, bool push, bool right) {
		if (damage == 1000 && !isDead)	// Fire
			Die();
		else if (!playerCtrl.isGhost && !isDead) {
			currentH -= damage;
			if (push && right)
				GetComponent<Rigidbody2D>().AddForce(new Vector2(10f, 0), ForceMode2D.Impulse);
			else if (push && !right)
				GetComponent<Rigidbody2D>().AddForce(new Vector2(-10f, 0), ForceMode2D.Impulse);
			if (currentH <= 0f) {
				Die();
			} else {
				custom.PlayClipAt(injuryClip, transform.position); 	// Only one sound when you die
				reset.helmetLight.intensity -= damage/40;
			}
		}
	}

	private void Die () {
		isDead = true;
		if (playerCtrl.isGhost)
			playerCtrl.BackToNormal();
		if (playerCtrl.isRight)
			anim.SetTrigger("DeathRight");
		else
			anim.SetTrigger("DeathLeft");
		custom.PlayClipAt(deathClip, transform.position);
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		reset.helmet.rotation = Quaternion.Euler(20f, 0f, 0f);
		gun.allowedToShoot = false;
		playerCtrl.allowedToGhost = false;
		ripSprite.enabled = true;
		ripAnim.enabled = true;
		StartCoroutine(Revive());
	}

	private IEnumerator Revive () {
    	yield return new WaitForSeconds(5f);
    	GetComponent<PolygonCollider2D>().enabled = true;
    	currentH = HEALTH/2;
    	GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    	gun.allowedToShoot = true;
    	playerCtrl.allowedToGhost = true;
    	ripSprite.enabled = false;
    	reset.ResetScene();
    	ripAnim.enabled = false;
    	isDead = false;
	}
}
