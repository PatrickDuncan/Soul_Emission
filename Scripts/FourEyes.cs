using UnityEngine;
using System.Collections;
using System;

public class FourEyes : MonoBehaviour, IEnemy {

	public bool isRight;					// For determining which way the four eyes is currently facing.	
	public bool hasFallen;					// For determining if the four eyes has fallen down.	
	public bool allowedToMove;				// For determining if the four eyes is allowed to move.	
	private bool allowedToAttack = true;	// If four eyes is allowed to attack.
	public bool allowedToDestroy;			// If the bullet can be destroyed when collided.
	private readonly float MOVEFORCE = 500f;	// Amount of force added to move the player left and right.
	private readonly float MAXSPEED = 0.9f;	// The fastest the player can travel in the x axis.
	public float health = 100f;				// The health points for this instance of the four eyes prefab.
	private Vector2 playerPos;				// The player's position.
	public AudioClip deathClip;				// CLip for when four eyes meets its end.
	public AudioClip fallClip;				// CLip for when four eyes hits the ground.
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
		if (allowedToMove && hasFallen && !playerH.isDead && Functions.DeltaMax(playerPos.y, theTransform.position.y, 5f)) {
			anim.SetTrigger("Walk");
			Move();
		}
		// The player has to be under it, but there must be a limit to the y because it has to drop when the player is on the same floor.
		else if (!hasFallen && Functions.DeltaMax(playerPos.x, theTransform.position.x, 13f)) {
			if (playerPos.y < theTransform.position.y && Functions.DeltaMax(playerPos.y, theTransform.position.y, 5f)) {
				hasFallen = true;
				rigid.gravityScale = 1.8f;
				StartCoroutine(WaitForFall());
			}
		}
		else if (!allowedToMove && !hasFallen && Functions.DeltaMin(playerPos.y, theTransform.position.y, 10f)) {
			anim.SetTrigger("Idle");
		}		
	}

	private void OnCollisionStay2D (Collision2D col) {
		if (allowedToAttack && col.gameObject.tag.Equals("Player")) {
			allowedToAttack = false;
			playerH.TakeDamage(2f, false, false);
			StartCoroutine(WaitToAttack());
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
		if (health > 0) {
			health -= damage;
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
		yield return new WaitForSeconds(0.4f);
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		gameObject.layer = LayerMask.NameToLayer("Death");		// Death layer.
		// This should happen in the animation, but if the game lags...
		GetComponent<SpriteRenderer>().sprite = deathSprite;
		GetComponent<Animator>().enabled = false;
		enabled = false;
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
		custom.PlayClipAt(fallClip, theTransform.position);
    	allowedToMove = true;
		GetComponent<PolygonCollider2D>().enabled = true;
		anim.SetTrigger("Drop");
		rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
