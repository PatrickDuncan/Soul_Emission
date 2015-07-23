using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	[HideInInspector]
	public bool isRight = true;				// For determining which way the player is currently facing.
	public string stairsTag = "none";		// If interacting with the stairs in any way.
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public Quaternion defaultLight;			// Default position of the helmet light

	private Animator anim;					
	public Transform helmet;
	public Quaternion rotation;
	private PlayerHealth playerH;

	private void Awake () {
		helmet = GameObject.FindGameObjectWithTag("Helmetlight").transform;
		anim = GetComponent<Animator>();
		defaultLight = Quaternion.Euler(16f, 106f, 220f);
		playerH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}

	private void FixedUpdate ()	{
		float h = Input.GetAxis("Horizontal");
		Physics(h);
		//Touch Input
		if (Input.touchCount == 1) {
	     	if (Input.touches[0].position.x < Screen.width/2) {
	     		Physics(-1);	         	
	         }
	         else if (Input.touches[0].position.x > Screen.width/2) {
	         	Physics(1);  	
	     	}
	    }
	}

	private void Physics (float h) {
		//fullPathHash gets the current animation clip
		//Makes sure that the player is not in the shooting animation (left or right) before moving
		if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash != 485325471 && anim.GetCurrentAnimatorStateInfo(0).fullPathHash != -1268868314) {
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet
			if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
			if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
	     	if ((h > 0 && !isRight) || (h < 0 && isRight))
				Flip();	
		}
	}

	private void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Fire")
			playerH.TakeDamage(1000f);		//Instantly die if you touch fire
		if (col.gameObject.tag.Contains("Stairs"))
			stairsTag = "tag";
	}

	private void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag.Contains("Stairs"))
			stairsTag = col.gameObject.tag;	
	}

	private void OnCollisionExit2D (Collision2D col) {
		if (col.gameObject.tag.Contains("Stairs"))
			stairsTag = "none";
	}

	private void OnTriggerExit2D (Collider2D col) {
		if (col.gameObject.tag.Contains("Stairs"))
			stairsTag = "none";
	}

	private void Flip () {
		isRight = !isRight;
		resetHelmet();
	}

	public void resetHelmet () {
		if (isRight) {
			anim.SetTrigger("IdleRight");
			helmet.rotation = defaultLight;
			helmet.position = new Vector3(-1.5f + transform.position.x, 2.36f + transform.position.y, -0.42f);
		}
		else {
			anim.SetTrigger("IdleLeft");
			helmet.RotateAround(Vector3.zero, Vector3.up, 148f);
			helmet.position = new Vector3(1.5f + transform.position.x, 2.36f + transform.position.y, -0.42f);
		}
	}
}
