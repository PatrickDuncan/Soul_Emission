using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;

public class PlayerHealth : MonoBehaviour {

	public readonly float HEALTH = 50f;		// The player's health maximum.
	[HideInInspector]			
	private float currentH;					// The player's current health.
	[HideInInspector]
	public bool isDead;						// If the player is dead.

	private Animator ripAnim;				// Reference to Rip's Animator
	private SpriteRenderer ripSprite;		// Reference to Rip's Sprite Renderer
	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator on the player.
	private Gun gun;						// Reference to the Gun class.
	private Positions positions;			// Reference to the Positions class.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	public AudioClip injuryClip;			// Clip for when the player gets injured.
	public AudioClip deathClip;				// Clip for when the player dies.
	public AudioClip kissClip;				// Normal background song
	public AudioClip expanseClip;			// Secondary background song

	private void Awake () {
		ripAnim = GameObject.Find("Rip").GetComponent<Animator>();
		ripSprite = GameObject.Find("Rip").GetComponent<SpriteRenderer>();
		playerCtrl = GetComponent<PlayerControl>();
		anim = GetComponent<Animator>();
		gun = GameObject.Find("Gun").GetComponent<Gun>();
		positions = GameObject.Find("Background").GetComponent<Positions>();
		custom = GameObject.Find("Background").GetComponent<CustomPlayClipAtPoint>();
		currentH = HEALTH;
	}

	private void Start () {
		ResetPosition();
	}
	
	private void OnLevelWasLoaded(int level) {
        positions = GameObject.Find("Background").GetComponent<Positions>();
        ResetPosition();
        if (level == 2)
        	GetComponent<AudioSource>().clip = expanseClip;
        else
        	GetComponent<AudioSource>().clip = kissClip;
        GetComponent<AudioSource>().Play();
    }

	public void TakeDamage (float damageAmount, bool push, bool right) {
		if (damageAmount == 1000 && !isDead)	// Fire
			Die();
		else if (!playerCtrl.isGhost && !isDead) {
			currentH -= damageAmount;
			if (push && right)
				GetComponent<Rigidbody2D>().AddForce(new Vector2(10f, 0), ForceMode2D.Impulse);
			else if (push && !right)
				GetComponent<Rigidbody2D>().AddForce(new Vector2(-10f, 0), ForceMode2D.Impulse);
			if (currentH <= 0f) {
				Die();
			} else {
				custom.PlayClipAt(injuryClip, transform.position); 	// Only one sound when you die
				playerCtrl.helmetLight.intensity -= damageAmount/40;
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
    	Reset();
    	ripAnim.enabled = false;
    	isDead = false;
	}

	// Reset the scene and bring the players HEALTH to half of the maximum
	private void Reset () {
		try {
	    	transform.rotation = Quaternion.Euler(0f, 0f, 0f);
	    	playerCtrl.helmetLight.intensity = 1.575f;
	    	ResetPosition();
	    	playerCtrl.resetHelmet();
	    	// Loop through all the pointy legs in the scene and reset their positions.
	    	int	j = positions.pointyStart; 
	    	for (int i=0; i<positions.pointy.Length; i++) {
		    	GameObject.Find("Pointy Legs " + j).transform.position = positions.pointy[i];
		    	if (GameObject.Find("Pointy Legs " + j).GetComponent<PointyLegs>().health > 0f)
		    		GameObject.Find("Pointy Legs " + j).GetComponent<PointyLegs>().health = 45f;
		    	j++;
		    }
		    j = positions.fourEyesStart;
		    for (int i=0; i<positions.fourEyes.Length; i++) {
		    	GameObject.Find("Four Eyes " + j).transform.position = positions.fourEyes[i];
		    	if (GameObject.Find("Four Eyes " + j).GetComponent<FourEyes>().health > 0f)
		    		GameObject.Find("Four Eyes " + j).GetComponent<FourEyes>().health = 100f;
		    	j++;
		    }
		} catch (Exception e) {
			print(e);
		}
    }

    public void ResetPosition () {
    	transform.position = positions.player;
    	playerCtrl.isRight = positions.isRight;
    }
}
