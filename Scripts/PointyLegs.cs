using UnityEngine;
using System.Collections;
using System;

public class PointyLegs : MonoBehaviour {

	public bool isRight;					// For determining which way the pointy legs is currently facing.	
	private bool allowedToAttack = true;	// If pointy legs is allowed to attack.
	public bool attacking;					// If pointy legs is currently swinging its arms to attack.
	private readonly float MOVEFORCE = 365f;	// Amount of force added to move the player left and right.
	private readonly float MAXSPEED = 1.5f;	// The fastest the player can travel in the x axis.
	public float health = 45f;				// The health points for this instance of the pointy legs prefab.
	private float maxVal;					// Maximum value used in the DeltaMax function.
	private Vector2 playerPos;				// The player's position.
	public AudioClip swingClip;				// Clip for when pointy legs attacks.
	public AudioClip deathClip;				// CLip for when pointy legs meets its end.
	public Sprite deathSprite;				// Final image in the death animation. 

	private Transform theTransform;			// Reference to the transform.
	private Animator anim;					// Reference to the Animator component.
	private Transform player;				// Reference to the Player's transform.
	private Rigidbody2D rigid;				// Reference to the Rigidbody2D component.
	private PlayerHealth playerH;			// Reference to the PlayerHealth script.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	private void Awake () {
		theTransform = transform;
		anim = GetComponent<Animator>();
		GameObject gO  = GameObject.FindWithTag("Player");
		player = gO.transform;
		rigid = GetComponent<Rigidbody2D>();
		playerH = gO.GetComponent<PlayerHealth>();
		custom = GameObject.FindWithTag("Scripts").GetComponent<CustomPlayClipAtPoint>();
	}

	private void Update () {
		playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
		if ((playerPos.x > theTransform.position.x && !isRight) || (playerPos.x < theTransform.position.x && isRight))
			Flip();
		if (playerH.playerCtrl.isRight) 
			maxVal = 2.8f;
		else
			maxVal = 3.2f;
		if (allowedToAttack && !playerH.isDead && Functions.DeltaMax(playerPos.x, theTransform.position.x, maxVal) && Functions.DeltaMax(playerPos.y, theTransform.position.y, 2f)) {
			allowedToAttack = false;
			anim.SetTrigger("Attack");
			attacking = true;
			StartCoroutine(PlayerHurt());
			StartCoroutine(WaitToAttack());
			custom.PlayClipAt(swingClip, theTransform.position);
		}
		else if (allowedToAttack && Functions.DeltaMin(playerPos.x, theTransform.position.x, maxVal) && Functions.DeltaMax(playerPos.y, theTransform.position.y, 2f)) {
			anim.SetTrigger("Walk");
			attacking = false;
			Move();
		}
		else {
			anim.SetTrigger("Idle");
			attacking = false;
		}		
	}

	private void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag.Equals("Fire"))
			TakeDamage(1000f);		// Instantly die if you touch fire
	}

	private void Move () {
		float sign;
		// If it is to the left or right of a hero
		if (playerPos.x > theTransform.position.x)
			sign = 1f;
		else
			sign = -1f; 
		if (sign * rigid.velocity.x < MAXSPEED)
			rigid.AddForce(Vector2.right * sign * MOVEFORCE);
		if (Mathf.Abs(rigid.velocity.x) > MAXSPEED)
			// Set the player's velocity to the MAXSPEED in the x axis.
			rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x) * MAXSPEED, rigid.velocity.y);
	}

	private void Flip () {
		isRight = !isRight;
		Vector3 theScale = theTransform.localScale;
		theScale.x *= -1;
		theTransform.localScale = theScale;
	}

	public void TakeDamage (float damage) {
		if (health > 0) {
			health -= damage;
			StopCoroutine(WaitToAttack());
			StopCoroutine(PlayerHurt());
			// When it dies disable all unneeded game objects and switch to death animation/sprite
			if (health <= 0f)
				StartCoroutine(Death());
			else
				anim.SetTrigger("Hurt");
		}
	}

	public IEnumerator Death () {
		custom.PlayClipAt(deathClip, theTransform.position);
		anim.SetTrigger("Death");
		yield return new WaitForSeconds(0.40f);    
		DeathState();
	}
	
	public void DeathState () {
    	GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		gameObject.layer = LayerMask.NameToLayer("Death");		// Death layer.
		// This should happen in the animation, but if the game lags...
		GetComponent<SpriteRenderer>().sprite = deathSprite;
		GetComponent<Animator>().enabled = false;
		enabled = false;	
    }
	// Wait to attack again.
	private IEnumerator WaitToAttack () {
        yield return new WaitForSeconds(2.9f);
        allowedToAttack = true;
    }

    // Allows you to dodge the attack
    private IEnumerator PlayerHurt () {
    	yield return new WaitForSeconds(0.32f);
    	if (health > 0 && Functions.DeltaMax(playerPos.x, theTransform.position.x, maxVal) && Functions.DeltaMax(playerPos.y, theTransform.position.y, 2f))
    		playerH.TakeDamage(10f, true, isRight);
    }
}
