using UnityEngine;
using System.Collections;
using System;

public class FourEyes : MonoBehaviour {

	public bool isRight;					// For determining which way the four eyes is currently facing.	
	public bool hasFallen;					// For determining if the four eyes has fallen down.	
	public bool allowedToMove;				// For determining if the four eyes is allowed to move.	
	private bool allowedToAttack = true;	// If four eyes is allowed to attack.
	public bool allowedToDestroy;			// If the bullet can be destroyed when collided.
	private readonly float MOVEFORCE = 500f;	// Amount of force added to move the player left and right.
	private readonly float MAXSPEED = 0.35f;	// The fastest the player can travel in the x axis.
	public float health = 100f;				// The health points for this instance of the four eyes prefab.
	private Vector2 playerPos;				// The player's position.
	public AudioClip deathClip;				// CLip for when four eyes meets its end.
	public AudioClip fallClip;				// CLip for when four eyes hits the ground.

	private Animator anim;					// Reference to the Animator component.
	private Transform player;				// Reference to the Player's transform.
	private Rigidbody2D rigid;				// Reference to the Rigidbody2D component.
	private PlayerHealth playerH;			// Reference to the PlayerHealth script.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	private void Awake () {
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		rigid = GetComponent<Rigidbody2D>();
		playerH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
		custom = GameObject.Find("Background").GetComponent<CustomPlayClipAtPoint>();
	}

	private void Update () {
		playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
		if ((playerPos.x > transform.position.x && !isRight) || (playerPos.x < transform.position.x && isRight))
			Flip();
		if (allowedToMove && hasFallen && !playerH.isDead && Mathf.Abs(playerPos.y - transform.position.y) < 5f) {
			anim.SetTrigger("Walk");
			Move();
		}
		else if (!hasFallen && Mathf.Abs(playerPos.x - transform.position.x) < 13f  && playerPos.y < transform.position.y && Mathf.Abs(playerPos.y - transform.position.y) < 5f) {
			hasFallen = true;
			rigid.gravityScale = 1.8f;
			StartCoroutine(WaitForFall());
		}
		else if (!allowedToMove && !hasFallen && Mathf.Abs(playerPos.y - transform.position.y) >= 10f){
			anim.SetTrigger("Idle");
		}		
	}

	private void OnCollisionStay2D (Collision2D col) {
		if (allowedToAttack && col.gameObject.tag.Equals("Player")) {
			allowedToAttack = false;
			playerH.TakeDamage(1f, false, false);
			StartCoroutine(WaitToAttack());
		}
	}

	private void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag.Equals("Fire"))
			TakeDamage(1000f);		// Instantly die if you touch fire
	}

	private void Move () {
		float h;
		// If a Poiny Legs is to the left or right of a hero
		if (playerPos.x > transform.position.x)
			h = 1f;
		else
			h = -1f; 
		if (h * GetComponent<Rigidbody2D>().velocity.x < MAXSPEED)
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * MOVEFORCE);
		if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > MAXSPEED)
			// ... set the player's velocity to the MAXSPEED in the x axis.
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * MAXSPEED, GetComponent<Rigidbody2D>().velocity.y);
	}

	private void Flip () {
		isRight = !isRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void TakeDamage (float damageAmount) {
		health -= damageAmount;
		// When it dies disable all unneeded game objects and switch to death animation/sprite
		if (health <= 0f) {
			anim.SetTrigger("Death");
			try {
				custom.PlayClipAt(deathClip, transform.position);
			} catch (Exception e) {
				print(e);
			} 
			rigid.Sleep();
			rigid.constraints = RigidbodyConstraints2D.FreezeAll;
			GetComponent<PolygonCollider2D>().enabled = false;
			enabled = false;
		} else {
			anim.SetTrigger("Hurt");
		}
	}

	public void CanShoot () {
		allowedToDestroy = true;
	}

	// Wait to attack again.
	private IEnumerator WaitToAttack () {
        yield return new WaitForSeconds(0.5f);
        allowedToAttack = true;
    }

    private IEnumerator WaitForFall () {
    	yield return new WaitForSeconds(0.75f);
		custom.PlayClipAt(fallClip, transform.position);
    	allowedToMove = true;
		GetComponent<PolygonCollider2D>().enabled = true;
		anim.SetTrigger("Drop");
		rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
