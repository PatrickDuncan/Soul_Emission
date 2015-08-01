using UnityEngine;
using System.Collections;

public class PointyLegs : MonoBehaviour {

	public bool isRight = false;			// For determining which way the pointy legs is currently facing.	
	private bool allowedToAttack = true;	// If pointy legs is allowed to attack.
	public bool attacking = false;			// If pointy legs is currently swinging its arms to attack.
	public readonly float MOVEFORCE = 365f;	// Amount of force added to move the player left and right.
	public readonly float MAXSPEED = 1f;	// The fastest the player can travel in the x axis.
	public float health = 45f;				// The health points for this instance of the pointy legs prefab.
	private Vector2 playerPos;				// The player's position.
	public AudioClip swingClip;				// Clip for when pointy legs attacks.
	public AudioClip deathClip;				// CLip for when pointy legs meets its end.

	private Animator anim;					// Reference to the Animator component.
	private Transform player;				// Reference to the Player's transform.
	private Rigidbody2D rb;					// Reference to the Rigidbody2D component.
	private PlayerHealth playerH;			// Reference to the PlayerHealth script.

	void Awake () {
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		rb = GetComponent<Rigidbody2D>();
		playerH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}

	void Update () {
		playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
		if ((playerPos.x > transform.position.x && !isRight) || (playerPos.x < transform.position.x && isRight))
			Flip();
		if (allowedToAttack && !playerH.isDead && Mathf.Abs(playerPos.x - transform.position.x) < 2.4f && Mathf.Abs(playerPos.y - transform.position.y) < 1f) {
			anim.SetTrigger("Attack");
			attacking = true;
			StartCoroutine(PlayerHurt());
			StartCoroutine(WaitToAttack());
			AudioSource.PlayClipAtPoint(swingClip, transform.position);
		}
		else if (allowedToAttack && Mathf.Abs(playerPos.x - transform.position.x) > 2.4f  && Mathf.Abs(playerPos.y - transform.position.y) < 2f) {
			anim.SetTrigger("Walk");
			attacking = false;
			Move();
		}
		else {
			anim.SetTrigger("Idle");
			attacking = false;
		}		
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Fire")
			TakeDamage(1000f);		//Instantly die if it touches fire	
	}

	void Move () {
		float h;
		//If a Poiny Legs is to the left or right of a hero
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

	void Flip () {
		isRight = !isRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void TakeDamage (float damageAmount) {
		health -= damageAmount;
		//When it dies disable all unneeded game objects and switch to death animation/sprite
		if (health <= 0f) {
			anim.SetTrigger("Death");
			AudioSource.PlayClipAtPoint(deathClip, transform.position);
			rb.Sleep();
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
			GetComponent<PolygonCollider2D>().enabled = false;
			enabled = false;
		}
	}

	//Wait to attack again.
	private IEnumerator WaitToAttack () {
        allowedToAttack = false;
        yield return new WaitForSeconds(3f);
        allowedToAttack = true;
    }

    //Allows you to dodge the attack
    private IEnumerator PlayerHurt () {
    	yield return new WaitForSeconds(0.32f);
    	if (Mathf.Abs(playerPos.x - transform.position.x) < 2.4f)
    		playerH.TakeDamage(10f);
    }
}
