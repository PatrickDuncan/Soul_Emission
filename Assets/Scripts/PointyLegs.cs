using UnityEngine;
using System.Collections;

public class PointyLegs : MonoBehaviour {

	public bool isRight = false;
	private bool allowedToAttack = true;
	private bool allowedToStairs = true;
	public bool attacking = false;
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 1f;				// The fastest the player can travel in the x axis.
	public float health = 45f;
	private float damageAmount = 10f;
	private Vector2 center;
	private Vector2 playerPos;

	private Animator anim;					// Reference to the Animator component.
	private Transform player;
	private Rigidbody2D rb;
	private PlayerHealth playerH;

	void Awake() {
		anim = transform.root.gameObject.GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		rb = GetComponent<Rigidbody2D>();
		center = rb.centerOfMass;
		playerH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}

	void Update () {
		playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
		if ((playerPos.x > transform.position.x && !isRight) || (playerPos.x < transform.position.x && isRight))
			Flip();
		if (allowedToAttack && Mathf.Abs(playerPos.x - transform.position.x) < 2.4f && Mathf.Abs(playerPos.y - transform.position.y) < 1f) {
			anim.SetTrigger("Attack");
			attacking = true;
			StartCoroutine(PlayerHurt());
			StartCoroutine(WaitToAttack());
		}
		else if (allowedToAttack && Mathf.Abs(playerPos.x - transform.position.x) > 2.4f) {
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
		for (int i=1; i<10; i++) {
			try {
				if (col.gameObject.tag == "Stairs"+i && !GameObject.FindGameObjectWithTag("Stairs"+i).GetComponent<PolygonCollider2D>().isTrigger && allowedToStairs) {
					if (isRight) {
						transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Clamp(transform.localEulerAngles.z, 34f, 0f));
						rb.centerOfMass = new Vector2(center.x - 1.7f, center.y - 0.3f);
					}
					else {
						transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Clamp(transform.localEulerAngles.z, 326f, 361f));
						rb.centerOfMass = new Vector2(center.x + 0.1f, center.y - 0.3f);
					}
					rb.constraints = RigidbodyConstraints2D.None;
					rb.mass = 27f;		//Allows the enemy to move up the staris quicker
					break;		//Can only collide with one stair at a time
				}
			}
			catch (UnityEngine.UnityException) {
				break;
			}
		}
	}

	void OnCollisionExit2D (Collision2D col) {
		for (int i=1; i<10; i++) {
			try {
				if (col.gameObject.tag == "Stairs"+i && !GameObject.FindGameObjectWithTag("Stairs"+i).GetComponent<PolygonCollider2D>().isTrigger) {
					transform.localEulerAngles = new Vector3(0f,0f,0f);
					rb.constraints = RigidbodyConstraints2D.FreezeRotation;
					rb.mass = 40f;
					StartCoroutine(WaitForStairs());
					break;
				}
			}
			catch (UnityEngine.UnityException) {
				break;
			}
		}
	}

	void Move () {
		float h;
		//If a Poiny Legs is to the left or right of a hero
		if (playerPos.x > transform.position.x)
			h = 1f;
		else
			h = -1f; 
		if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
		if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
	}

	void Flip () {
		isRight = !isRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void TakeDamage () {
		health -= damageAmount;
		//When it dies disable all unneeded game objects and switch to death animation/sprite
		if (health <= 0f) {
			anim.SetTrigger("Death");
			rb.Sleep();
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
			GetComponent<PolygonCollider2D>().isTrigger = true;
			enabled = false;
		}
	}

	//Wait to attack again.
	private IEnumerator WaitToAttack () {
        allowedToAttack = false;
        yield return new WaitForSeconds(3.5f);
        allowedToAttack = true;
    }

    //Wait to collide with stairs *glitch fix*
    private IEnumerator WaitForStairs () {
    	allowedToStairs = false;
    	yield return new WaitForSeconds(3f);
    	allowedToStairs = true;
    }

    //Allows you to dodge the attack
    private IEnumerator PlayerHurt () {
    	yield return new WaitForSeconds(0.32f);
    	if (Mathf.Abs(playerPos.x - transform.position.x) < 2.4f)
    		playerH.TakeDamage(10f);
    	attacking = false;
    }
}
