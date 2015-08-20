using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public bool isRight = true;				// For determining which way the player is currently facing.
	public bool isGhost = false;			// For determining if the player is using ghost powers.
	public bool allowedToGhost = true;		// For determining if the player is using ghost powers.
	public bool isBeam = false;				// If the player is using the beam.
	public bool isNormal = true;			// If the player's layer mask is not Ghost;
	public const float MOVEFORCE = 365f;	// Amount of force added to move the player left and right.
	private float maxSpeed = 1.5f;			// The fastest the player can travel in the x axis.
	public float previousIntensity = 5f;	// The light intensity before using ghost power.

	private Animator anim;					// Reference to the Animator component				
	private PlayerHealth playerH;			// Reference to the PlayerHealth script
	private Rigidbody2D rigid;				// Reference to the Rigidbody2D component
	private Lift lift;						// Reference to the Lift script.
	private Reset reset;					// Reference to the Reset script.


	private void Awake () {
		anim = GetComponent<Animator>();
		playerH = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
		lift = GameObject.FindWithTag("Beam").GetComponent<Lift>();
		rigid = GetComponent<Rigidbody2D>();
		reset = GameObject.Find("Scripts").GetComponent<Reset>();
		if (!isRight)
			reset.ResetHelmet();
	}

	private void Update () {
	    if (Input.GetButtonDown("Ghost") && allowedToGhost && lift.allowedToBeam) {
			// Makes sure that the player is not in the shooting animation (left or right) or hovering before ghosting.
	    	if (Functions.GetPath(anim) != 485325471 && Functions.GetPath(anim) != -1268868314) { 
	    		if (rigid.gravityScale == 1.8f) {
		    		allowedToGhost = false;
		    		Ghost(); 
		    	}
	    	}
	    }
	    // Stops glitch where the player would get stuck above the enemy after ghost mode.
	    if (!isGhost && !isNormal && EnemiesFarAway()) {
	    	isNormal = true;
	    	gameObject.layer = LayerMask.NameToLayer("Default");
	    }
	}

	private void FixedUpdate ()	{
		if (!playerH.isDead) {
			float h = Input.GetAxis("Horizontal");
			Physics(h);
			// Touch Input
			if (Input.touchCount == 1 && Input.touches[0].position.x < Screen.width/2 && Input.touches[0].position.y < Screen.height/2) {
		     	if (Input.touches[0].position.x < Screen.width/4)
		     		Physics(-1);	         	
		        else if (Input.touches[0].position.x > Screen.width/4)
		         	Physics(1);  	
		    }
		}
	}

	private bool EnemiesFarAway () {
		// Loops through all enemies to make sure that they're not colliding (by comparing the x values).
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
			Vector3 enemyPos = enemy.transform.position;
			if (!isRight && enemy.name.Equals("Pointy Legs")) {
				// Return false if any one enemy is too close.
				if (Functions.DeltaMax(enemyPos.x, transform.position.x, 2.2f) && Functions.DeltaMax(enemyPos.y, transform.position.y, 2f))
					return false;
			} 
			else {
				if (Functions.DeltaMax(enemyPos.x, transform.position.x, 3.6f) && Functions.DeltaMax(enemyPos.y, transform.position.y, 2f))
					return false;
			}
		}
		return true;
	}

	private void Physics (float h) {
		// Makes sure that the player is not in the shooting animation (left or right) before moving
		if (!isBeam && Functions.GetPath(anim) != 485325471 && Functions.GetPath(anim) != -1268868314) {
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet
			if (h * rigid.velocity.x < maxSpeed)
				rigid.AddForce(Vector2.right * h * MOVEFORCE);
			if (Mathf.Abs(rigid.velocity.x) > maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x) * maxSpeed, rigid.velocity.y);
	     	if ((h > 0 && !isRight) || (h < 0 && isRight))
				Flip();	
		}
	}

	private void Ghost () {	
		isGhost = true;
		if (isRight)
			anim.SetTrigger("GhostRight");
		else
			anim.SetTrigger("GhostLeft");
		rigid.gravityScale = 0f;
		gameObject.layer = LayerMask.NameToLayer("Ghost");
		isNormal = false;
		previousIntensity = reset.helmetLight.intensity;
		reset.helmetLight.intensity = 4;
		GetComponent<AudioSource>().pitch = 3f;
		maxSpeed = 3f;
		rigid.velocity = new Vector2(rigid.velocity.x, 0);		// Alllows you to stop in the mid air.
		StartCoroutine(GhostTime());
	}

	private void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag.Equals("Door") && GameObject.FindWithTag(col.gameObject.tag).GetComponent<Light>().enabled) {
			// If right door move to next scene, if left move to previous
			int level = Application.loadedLevel;
			Application.LoadLevel(level + 1);
		}
	}

	private void Flip () {
		isRight = !isRight;
		reset.ResetHelmet();
	}

	private IEnumerator GhostTime () {
    	yield return new WaitForSeconds(3.5f);
    	if (!playerH.isDead) {
	    	if (isRight)
				anim.SetTrigger("IdleRight");
			else
				anim.SetTrigger("IdleLeft");
			BackToNormal();
			StartCoroutine(WaitForGhost());
		}
	}

	public void BackToNormal () {
		rigid.gravityScale = 1.8f;
    	GetComponent<AudioSource>().pitch = 0.4f;
    	reset.helmetLight.intensity = previousIntensity;
		isGhost = false;
		maxSpeed = 1.5f;
	}

	private IEnumerator WaitForGhost () {
    	yield return new WaitForSeconds(10f);
    	allowedToGhost = true;
	}
}
