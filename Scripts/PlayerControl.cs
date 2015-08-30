using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public bool isRight = true;				// For determining which way the player is currently facing.
	public bool isGhost = false;			// For determining if the player is using ghost powers.
	public bool allowedToGhost = true;		// For determining if the player is using ghost powers.
	public bool isBeam = false;				// If the player is using the beam.
	public bool isNormal = true;			// If the player's layer mask is not Ghost;
	public bool allowedToBeam = true;		// If the player can use the beam.
	public bool allowedToShoot = true;		// Makes sure that the deltatime between the last shot is not too short.
	public const float MOVEFORCE = 365f;	// Amount of force added to move the player left and right.
	public float maxSpeed = 1.5f;			// The fastest the player can travel in the x axis.
	public float previousIntensity = 5f;	// The light intensity before using ghost power.
	private GameObject[] enemies;			// List of all the enemy tagged game objects.

	private Transform theTransform;			// Reference to the Transform.
	private Animator anim;					// Reference to the Animator component				
	private PlayerHealth playerH;			// Reference to the PlayerHealth script
	private Rigidbody2D rigid;				// Reference to the Rigidbody2D component
	private Lift lift;						// Reference to the Lift script.
	private Reset reset;					// Reference to the Reset script.
	private HelpfulTips tips;				// Reference to the HelpfulTips script.

	private void Awake () {
		theTransform = transform;
		anim = GetComponent<Animator>();
		playerH = GetComponent<PlayerHealth>();
		rigid = GetComponent<Rigidbody2D>();
		reset = GameObject.FindWithTag("Scripts").GetComponent<Reset>();
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		tips = GameObject.FindWithTag("Tips").GetComponent<HelpfulTips>();
		if (!isRight)
			reset.ResetHelmet();
	}

	private void Update () {
		if (Functions.GetPath(anim) == 485325471 && Functions.GetPath(anim) == -1268868314) { 
			allowedToGhost = false;
			allowedToShoot = false;
		}
	    if (Input.GetButtonDown("Ghost") && allowedToGhost && allowedToBeam) {
			// Makes sure that the player is not in the shooting animation (left or right) or hovering before ghosting.
    		if (rigid.gravityScale > 0f) {
	    		allowedToGhost = false;
	    		Ghost(); 
	    	}
	    }
	    // Stops glitch where the player would get stuck above the enemy after ghost mode.
	    if (!isGhost && !isNormal && EnemiesFarAway()) {
	    	isNormal = true;
	    	gameObject.layer = LayerMask.NameToLayer("Player");
	    }
	    // Helpful tips
	    if (Application.loadedLevel == 1) {
	    	Vector3 pos = theTransform.position;
			if (pos.x > -18f && pos.x < 0f && pos.y < -6.5f)
				tips.Show(0);
			else if (pos.x > 21f && pos.x < 37f && pos.y < -6.5f)
				tips.Show(1);
			else if (pos.x > 16f && pos.y > 6.9f)
				tips.Show(2);
			else if (pos.x < -25f && pos.y > -0.5f && pos.y < 1f)
				tips.Show(3);
			else
				tips.Show(-1);
		} else if (Application.loadedLevel == 6) {
			Vector3 pos = theTransform.position;
			if (pos.x > 6f && pos.x < 18f && pos.y < -6.5f && pos.y > -7.5f)
				tips.Show(0);
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

	private void OnLevelWasLoaded (int level) {
		// Resets the enemies array on level load.
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}

	private void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag.Equals("Door") && col.gameObject.GetComponentInChildren<Light>().enabled) {
			// If right door move to next scene, if left move to previous
			int level = Application.loadedLevel;
			//***Load Level***//
			Application.LoadLevel(level + 1); 
		}
	}
	
	private void OnCollisionStay2D (Collision2D col) {
		// Stuck on head fix.
		if (col.gameObject.tag.Equals("Enemy")) {
			//Layer 0 is "Default"
		    if (gameObject.layer == 0 && theTransform.position.y > col.gameObject.transform.position.y)
		    	StartCoroutine(StuckOnHead());
		}
	}

	private bool EnemiesFarAway () {
		// Loops through all enemies to make sure that they're not colliding (by comparing the x values).
		Vector3 enemyPos;
		foreach (GameObject enemy in enemies) {
			enemyPos = enemy.transform.position;
			if (!isRight && enemy.name.Equals("Pointy Legs")) {
				// Return false if any one enemy is too close.
				if (Functions.DeltaMax(enemyPos.x, theTransform.position.x, 2.2f) && Functions.DeltaMax(enemyPos.y, theTransform.position.y, 2f))
					return false;
			} 
			else {
				if (Functions.DeltaMax(enemyPos.x, theTransform.position.x, 3.6f) && Functions.DeltaMax(enemyPos.y, theTransform.position.y, 2f))
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
		reset.helmetLight.intensity = 6f;
		GetComponent<AudioSource>().pitch = 3f;
		maxSpeed = 3f;
		rigid.velocity = new Vector2(rigid.velocity.x, 0);		// Alllows you to stop in the mid air.
		StartCoroutine(GhostTime());
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

	private IEnumerator StuckOnHead () {
    	gameObject.layer = LayerMask.NameToLayer("Ghost");
    	yield return new WaitForSeconds(2f);
    	gameObject.layer = LayerMask.NameToLayer("Player");
	}
}
