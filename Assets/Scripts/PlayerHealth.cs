using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;

public class PlayerHealth : MonoBehaviour {
	public readonly float HEALTH = 10f;		// The player's health maximum.
	[HideInInspector]			
	private float currentH;					// The player's current health.
	[HideInInspector]
	public bool isDead = false;				// If the player is dead.

	private Animator ripAnim;				// Reference to Rip's Animator
	private SpriteRenderer ripSprite;		// Reference to Rip's Sprite Renderer
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator on the player.
	private Gun gun;						// Reference to the Gun class.
	private Positions positions;			// Reference to the Positions class.

	public AudioClip injuryClip;			// Clip for when the player gets injured.
	public AudioClip deathClip;				// Clip for when the player dies.

	private void Awake () {
		ripAnim = GameObject.FindGameObjectWithTag("Rip").GetComponent<Animator>();
		ripSprite = GameObject.FindGameObjectWithTag("Rip").GetComponent<SpriteRenderer>();
		playerCtrl = GetComponent<PlayerControl>();
		anim = GetComponent<Animator>();
		gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
		positions = GetComponent<Positions>();
		currentH = HEALTH;
	}
	
	public void TakeDamage (float damageAmount) {
		if (damageAmount == 1000 && !isDead)	//Fire
			Die();
		else if (!playerCtrl.isGhost && !isDead) {
			currentH -= damageAmount;
			//playerCtrl.helmetLight.intensity -= 0.2f;
			GetComponent<Rigidbody2D>().AddForce(new Vector2(-10f, 0), ForceMode2D.Impulse);
			if (currentH <= 0f) {
				Die();
			} else
				AudioSource.PlayClipAtPoint(injuryClip, transform.position); 	//Only one sound when you die
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
		AudioSource.PlayClipAtPoint(deathClip, transform.position);
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		playerCtrl.helmet.rotation = Quaternion.Euler(20f, 0f, 0f);
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
    	ResetPositions();
    	ripAnim.enabled = false;
    	isDead = false;
	}

	//Reset the scene and bring the players HEALTH to half of the maximum
	private void ResetPositions () {
    	transform.position = positions.player;
    	transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    	playerCtrl.isRight = positions.isRight;
    	playerCtrl.resetHelmet();
    	playerCtrl.helmetLight.intensity = 1.5f;
    	// Loop through all the pointy legs in the scene and reset their positions.
    	int	j = positions.pointyStart; 
    	print(j);
    	for (int i=0; i<positions.pointy.Length; i++) {
	    	GameObject.FindWithTag("PointyLegs" + j).transform.position = positions.pointy[i];
	    	if (GameObject.FindWithTag("PointyLegs" + j).GetComponent<PointyLegs>().health > 0f)
	    		GameObject.FindWithTag("PointyLegs" + j).GetComponent<PointyLegs>().health = 45f;
	    	j++;
	    }
	    j = positions.fourEyesStart;
	    for (int i=0; i<positions.fourEyes.Length; i++) {
	    	GameObject.FindWithTag("FourEyes" + j).transform.position = positions.fourEyes[i];
	    	if (GameObject.FindWithTag("FourEyes" + j).GetComponent<FourEyes>().health > 0f)
	    		GameObject.FindWithTag("FourEyes" + j).GetComponent<FourEyes>().health = 45f;
	    	j++;
	    }
    }
}
