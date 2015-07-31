using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public bool isRight = true;				// For determining which way the player is currently facing.
	public bool isGhost = false;			// For determining if the player is using ghost powers.
	public bool allowedToGhost = true;		// For determining if the player is using ghost powers.
	public readonly float MOVEFORCE = 365f;	// Amount of force added to move the player left and right.
	private float maxSpeed = 1.5f;			// The fastest the player can travel in the x axis.
	public float previousIntensity = 5f;	// The light intensity before using ghost power.
	public Quaternion defaultLight;			// Default position of the helmet light

	private Animator anim;					// Reference to the Animator component				
	public Transform helmet;				// Reference to the helmet object's transfrom
	public Light helmetLight;				// Reference to the helmet object's light
	private PlayerHealth playerH;			// Reference to the PlayerHealth script
	private Rigidbody2D rigid;				// Reference to the Rigidbody2D component

	private void Awake () {
		helmet = GameObject.FindGameObjectWithTag("Helmetlight").transform;
		helmetLight = GameObject.FindGameObjectWithTag("Helmetlight").GetComponent<Light>();
		anim = GetComponent<Animator>();
		defaultLight = Quaternion.Euler(16f, 106f, 220f);
		playerH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
		rigid = GetComponent<Rigidbody2D>();
		if (!isRight)
			resetHelmet();
	}

	private void Update () {
	    if (Input.GetButtonDown("Fire2") && allowedToGhost) {
	    	allowedToGhost = false;
	    	Ghost();  
	    }
	}

	private void FixedUpdate ()	{
		if (!playerH.isDead) {
			float h = Input.GetAxis("Horizontal");
			Physics(h);
			//Touch Input
			if (Input.touchCount == 1 && Input.touches[0].position.x < Screen.width/2 && Input.touches[0].position.y < Screen.height/2) {
		     	if (Input.touches[0].position.x < Screen.width/4)
		     		Physics(-1);	         	
		        else if (Input.touches[0].position.x > Screen.width/4)
		         	Physics(1);  	
		    }
		}
	}

	private void Physics (float h) {
		//fullPathHash gets the current animation clip
		//Makes sure that the player is not in the shooting animation (left or right) before moving
		if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash != 485325471 && anim.GetCurrentAnimatorStateInfo(0).fullPathHash != -1268868314) {
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
		previousIntensity = helmetLight.intensity;
		helmetLight.intensity = 4;
		GetComponent<AudioSource>().pitch = 3f;
		maxSpeed = 3f;
		StartCoroutine(GhostTime());
	}

	private void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag.Equals("Fire"))
			playerH.TakeDamage(1000f);		//Instantly die if you touch fire
		if (col.gameObject.tag.Contains("Door") && GameObject.FindGameObjectWithTag(col.gameObject.tag).GetComponent<Light>().enabled) {
			//If right door move to next scene, if left move to previous
			int i = Application.loadedLevel;
			//Get the name of the door's sprite
			string facing = GameObject.FindGameObjectWithTag(col.gameObject.tag).GetComponent<SpriteRenderer>().sprite.ToString();
			if (facing.Contains("Right"))
				Application.LoadLevel(i + 1);
			else if (facing.Contains("Left"))
				Application.LoadLevel(i - 1);
		}
	}

	private void Flip () {
		isRight = !isRight;
		resetHelmet();
	}

	public void resetHelmet () {
		if (isRight) {
			if (isGhost)
				anim.SetTrigger("GhostRight");
			else 
				anim.SetTrigger("IdleRight");
			helmet.rotation = defaultLight;
			helmet.position = new Vector3(-1.54f + transform.position.x, 2.36f + transform.position.y, -0.42f);
		}
		else {
			if (isGhost)
				anim.SetTrigger("GhostLeft");
			else 
				anim.SetTrigger("IdleLeft");
			helmet.RotateAround(Vector3.zero, Vector3.up, 148f);
			helmet.position = new Vector3(1.54f + transform.position.x, 2.36f + transform.position.y, -0.42f);
		}
	}

	private IEnumerator GhostTime () {
    	yield return new WaitForSeconds(4f);
    	if (isRight)
			anim.SetTrigger("IdleRight");
		else
			anim.SetTrigger("IdleLeft");
    	rigid.gravityScale = 1.8f;
    	gameObject.layer = LayerMask.NameToLayer("Default");
    	GetComponent<AudioSource>().pitch = 0.4f;
    	helmetLight.intensity = previousIntensity;
		isGhost = false;
		maxSpeed = 1.5f;
		StartCoroutine(WaitForGhost());
	}

	private IEnumerator WaitForGhost () {
    	yield return new WaitForSeconds(10f);
    	allowedToGhost = true;
	}
}
