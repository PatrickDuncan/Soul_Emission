using UnityEngine;
using System.Collections;
using System;

public class Explodetaur : MonoBehaviour, IEnemy {

	public bool isRight;					// For determining which way the explodetaur is currently facing.	
	private bool isDead;					// If the explodetaur is dead.
	private readonly float MOVEFORCE = 500f;	// Amount of force added to move the player left and right.
	private readonly float MAXSPEED = 3.1f;	// The fastest the player can travel in the x axis.
	public float health = 14f;				// The health points for this instance of the explodetaur prefab.
	private Vector2 playerPos;				// The player's position.
	public AudioClip deathClip;				// CLip for when explodetaur meets its end.
	public Sprite deathSprite;				// Final image in the death animation. 

	private Transform theTransform;			// Reference to the Transform.
	private Animator anim;					// Reference to the Animator component.
	private Transform player;				// Reference to the Player's transform.
	private Rigidbody2D rigid;				// Reference to the Rigidbody2D component.
	private PlayerHealth playerH;			// Reference to the PlayerHealth script.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	private void Awake () {
		theTransform = transform;
		anim = GetComponent<Animator>();
		GameObject gO = GameObject.FindWithTag("Player");
		player = gO.transform;
		rigid = GetComponent<Rigidbody2D>();
		playerH = gO.GetComponent<PlayerHealth>();
		custom = GameObject.FindWithTag("Scripts").GetComponent<CustomPlayClipAtPoint>();
	}

	private void Update () {
		playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
		if ((playerPos.x > theTransform.position.x && !isRight) || (playerPos.x < theTransform.position.x && isRight))
			Flip();
		if (!isDead && !playerH.isDead && Functions.DeltaMax(playerPos.x, theTransform.position.x, 2.9f) && Functions.DeltaMax(playerPos.y, theTransform.position.y, 2f)) {
			isDead = true;
			StartCoroutine(Death());		
		}
		else if (!playerH.isDead && Functions.DeltaMin(playerPos.x, theTransform.position.x, 2.9f) && Functions.DeltaMax(playerPos.y, theTransform.position.y, 2f)) {
			anim.SetTrigger("Walk");
			Move();
		}
		else {
			anim.SetTrigger("Idle");
		}		
	}

	public void OnTriggerEnter2D (Collider2D col) {
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
			// ... set the player's velocity to the MAXSPEED in the x axis.
			rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x) * MAXSPEED, rigid.velocity.y);
	}

	private void Flip () {
		isRight = !isRight;
		Vector3 theScale = theTransform.localScale;
		theScale.x *= -1;
		theTransform.localScale = theScale;
	}

	public void TakeDamage (float damage) {
		health -= damage;
		// When it dies disable all unneeded game objects and switch to death animation/sprite
		if (health <= 0f) {
			isDead = true;
			StartCoroutine(Death());
		}
		else
			anim.SetTrigger("Hurt");
	}

    public IEnumerator Death () {
    	// Do visual/audio death stuff then wait to explode and depower.
    	custom.PlayClipAt(deathClip, theTransform.position);
		anim.SetTrigger("Death");
    	yield return new WaitForSeconds(0.35f);
    	if (Functions.DeltaMax(playerPos.x, theTransform.position.x, 3.05f) && Functions.DeltaMax(playerPos.y, theTransform.position.y, 2f))
    		playerH.TakeDamage(20f, true, isRight);
    	GetComponentInChildren<Light>().enabled = false;
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
}
