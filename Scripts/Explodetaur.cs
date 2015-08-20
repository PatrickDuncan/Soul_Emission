using UnityEngine;
using System.Collections;

public class Explodetaur : MonoBehaviour, IEnemy {

	public bool isRight;					// For determining which way the explodetaur is currently facing.	
	private bool allowedToAttack = true;	// If explodetaur is allowed to attack.
	public bool attacking;					// If explodetaur is currently swinging its arms to attack.
	private readonly float MOVEFORCE = 500f;	// Amount of force added to move the player left and right.
	private readonly float MAXSPEED = 2f;	// The fastest the player can travel in the x axis.
	public float health = 14f;				// The health points for this instance of the explodetaur prefab.
	private Vector2 playerPos;				// The player's position.
	public AudioClip swingClip;				// Clip for when explodetaur attacks.
	public AudioClip deathClip;				// CLip for when explodetaur meets its end.

	private Animator anim;					// Reference to the Animator component.
	private Transform player;				// Reference to the Player's transform.
	private Rigidbody2D rigid;				// Reference to the Rigidbody2D component.
	private PlayerHealth playerH;			// Reference to the PlayerHealth script.
	private CustomPlayClipAtPoint custom;	// Reference to the CustomPlayClipAtPoint script.

	private void Awake () {
		anim = GetComponent<Animator>();
		player = GameObject.FindWithTag("Player").transform;
		rigid = GetComponent<Rigidbody2D>();
		playerH = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
		custom = GameObject.Find("Scripts").GetComponent<CustomPlayClipAtPoint>();
	}

	private void Update () {
		playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
		if ((playerPos.x > transform.position.x && !isRight) || (playerPos.x < transform.position.x && isRight))
			Flip();
		if (allowedToAttack && !playerH.isDead && Functions.DeltaMax(playerPos.x, transform.position.x, 2.8f) && Functions.DeltaMax(playerPos.y, transform.position.y, 2f)) {
			anim.SetTrigger("Attack");
			attacking = true;
			StartCoroutine(PlayerHurt());
			StartCoroutine(WaitToAttack());
			custom.PlayClipAt(swingClip, transform.position);
		}
		else if (allowedToAttack && Functions.DeltaMin(playerPos.x, transform.position.x, 2.8f) && Functions.DeltaMax(playerPos.y, transform.position.y, 2f)) {
			anim.SetTrigger("Walk");
			attacking = false;
			Move();
		}
		else {
			anim.SetTrigger("Idle");
			attacking = false;
		}		
	}

	public void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag.Equals("Fire"))
			TakeDamage(1000f);		// Instantly die if you touch fire
	}

	private void Move () {
		float sign;
		// If it is to the left or right of a hero
		if (playerPos.x > transform.position.x)
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
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void TakeDamage (float damage) {
		health -= damage;
		// When it dies disable all unneeded game objects and switch to death animation/sprite
		if (health <= 0f) {
			anim.SetTrigger("Death");
			custom.PlayClipAt(deathClip, transform.position);
			rigid.Sleep();
			rigid.constraints = RigidbodyConstraints2D.FreezeAll;
			GetComponent<PolygonCollider2D>().enabled = false;
			enabled = false;
		} else {
			anim.SetTrigger("Hurt");
		}
	}

	// Wait to attack again.
	private IEnumerator WaitToAttack () {
        allowedToAttack = false;
        yield return new WaitForSeconds(3f);
        allowedToAttack = true;
    }

    // Allows you to dodge the attack
    private IEnumerator PlayerHurt () {
    	yield return new WaitForSeconds(0.32f);
    	if (Functions.DeltaMax(playerPos.x, transform.position.x, 2.8f))
    		playerH.TakeDamage(10f, true, isRight);
    }
}
